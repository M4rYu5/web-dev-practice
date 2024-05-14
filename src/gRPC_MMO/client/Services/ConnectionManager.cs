using Godot;
using Grpc.Core;
using Grpc.Net.Client;
using ProximitySync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MMOgRPC.Services
{
    internal class ConnectionManager
    {
        private readonly Object _lock = new();

        private readonly GrpcChannel _channel;
        private readonly ProximityUpdater.ProximityUpdaterClient _client;
        private readonly SynchronizationContext? _syncContext;

        private Player _playerState = new();
        private DateTime _playerStateLastUpdate = DateTime.MinValue;


        private ConnectionManager()
        {
            _syncContext = SynchronizationContext.Current;

            // The port number must match the port of the gRPC server.
            _channel = GrpcChannel.ForAddress("https://localhost:7197");
            _client = new ProximityUpdater.ProximityUpdaterClient(_channel);

            RunSelfUpdate(100);
            RunEntityUpdater();
        }


        public delegate void PlayersStateUpdatedEventHandler(Players players);

        public event PlayersStateUpdatedEventHandler? PlayersStateUpdated;


        public static ConnectionManager Instance { get; set; } = new();


        public void SetPlayerState(Player player)
        {
            lock (_lock)
            {
                _playerState = player;
            }
        }


        /// <summary>
        /// starts a task that sends the current player/entity's state to the server every msDelay milliseconds
        /// </summary>
        private void RunSelfUpdate(int msDelay)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        Player p;
                        lock (_lock)
                        {
                            p = _playerState;
                        }
                        if (!string.IsNullOrEmpty(p.Name))
                        {
                            _playerStateLastUpdate = DateTime.Now;
                            await _client.PlayerUpdateAsync(p);
                        }
                        // server built in delay + latency → the time might already past
                        var waitTime = TimeSpan.FromMicroseconds(msDelay) - (DateTime.Now - _playerStateLastUpdate);
                        _playerStateLastUpdate = DateTime.Now;
                        if (waitTime.Milliseconds > 0)
                        {
                            GD.Print("waiting: " +  waitTime.Milliseconds);
                            await Task.Delay(waitTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr(ex.ToString());
                    }
                }
            });
        }

        /// <summary>
        /// Starts the tasks that receive update about other entityes (players) in the world.
        /// This will call PlayersStateUpdated event.
        /// </summary>
        private void RunEntityUpdater()
        {
            Task.Run(async () =>
            {
                var positionUpdater = _client.UpdatePlayers(new Google.Protobuf.WellKnownTypes.Empty());
                await foreach (var response in positionUpdater.ResponseStream.ReadAllAsync())
                {
                    try
                    {
                        var r = response;
                        _syncContext.Post(_ => PlayersStateUpdated(r), null);
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr(ex.ToString());
                    }
                }
            });
        }

    }
}
