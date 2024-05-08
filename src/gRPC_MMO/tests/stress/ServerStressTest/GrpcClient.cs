using Grpc.Core;
using Grpc.Net.Client;
using ProximitySync;

internal class GrpcClient
{
    private readonly Player player;
    private readonly GrpcChannel _channel;
    private readonly ProximityUpdater.ProximityUpdaterClient _client;
    private readonly bool tracking;
    private DateTime _playerStateLastUpdate = DateTime.MinValue;

    public GrpcClient(string address, bool tracking = false)
    {
        // The port number must match the port of the gRPC server.
        this.tracking = tracking;
        _channel = GrpcChannel.ForAddress(address);
        _client = new ProximityUpdater.ProximityUpdaterClient(_channel);

        player = new Player
        {
            Name = Random.Shared.Next().ToString(),
            Position = new Position2D(),
        };

        RunSelfUpdate(100);
        RunEntityUpdater();
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
                    Player p = player;
                    {
                        _playerStateLastUpdate = DateTime.Now;
                        if (tracking)
                        {
                            DateTime t = DateTime.Now;
                            await _client.PlayerUpdateAsync(p);
                            Console.WriteLine("Getting the update took: " + (DateTime.Now - t).TotalMilliseconds + " ms");
                        }
                        else
                        {
                            await _client.PlayerUpdateAsync(p);
                        }
                    }
                    // server built in delay + latency → the time might already past
                    var waitTime = TimeSpan.FromMicroseconds(msDelay) - (DateTime.Now - _playerStateLastUpdate);
                    _playerStateLastUpdate = DateTime.Now;
                    if (waitTime.Milliseconds > 0)
                    {
                        await Task.Delay(waitTime);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        });
    }


    public event Action<Players?>? PlayersStateUpdated;

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
                    PlayersStateUpdated?.Invoke(r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        });
    }

}