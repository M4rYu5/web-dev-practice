using System.Diagnostics;
using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
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
            Stopwatch stopwatch = Stopwatch.StartNew();
            using var call = _client.PlayerUpdate();
            while (true)
            {
                // try
                {
                    await Task.Delay(100);
                    Player p = player;
                    stopwatch.Restart();
                    call.RequestStream.WriteAsync(p);
                    if (tracking)
                        Console.WriteLine($"↑Pushing the update took: {stopwatch.Elapsed}");


                    // Player p = player;
                    // {
                    //     stopwatch.Restart();
                    //     await _client.PlayerUpdateAsync(p);
                    //     if (tracking)
                    //     {
                    //         Console.WriteLine("↑Pushing the update took: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
                    //     }
                    // }
                    // // server built in delay + latency → the time might already past
                    // var elapsed = stopwatch.Elapsed;
                    // var waitTime = TimeSpan.FromMilliseconds(msDelay) - elapsed;
                    // stopwatch.Restart();
                    // if (waitTime.TotalMilliseconds > 0)
                    // {
                    //     await Task.Delay(waitTime);
                    // }
                }
                // catch (Exception ex)
                // {
                //     Console.WriteLine(ex.ToString());
                // }
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
            var positionUpdater = _client.UpdatePlayers(new Empty());
            Stopwatch stopwatch = Stopwatch.StartNew();
            await foreach (var response in positionUpdater.ResponseStream.ReadAllAsync())
            {
                try
                {
                    if (tracking)
                        Console.WriteLine($"↓Getting the update took: {stopwatch.Elapsed}; Received: {response.Players_.Count}");
                    var r = response;
                    PlayersStateUpdated?.Invoke(r);
                    stopwatch.Restart();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    stopwatch.Restart();
                }
            }
        });


    }
}