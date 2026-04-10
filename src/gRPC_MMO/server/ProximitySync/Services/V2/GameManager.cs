using Google.Protobuf.Collections;
using Grpc.Core;
using ProximitySync.Data;
using System.Buffers;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ProximitySync.Services.V2
{

    /// <summary>
    /// Main purpose for this class is to test the performance of using multiple threads to write the response to clients.
    /// </summary>
    public class GameManager
    {
        private readonly TimeSpan deltaTarget = TimeSpan.FromMilliseconds(500);
        private readonly UpdateWorker[] updateWorkers = new UpdateWorker[5];

        private int nextUpdateWorkerIndexToAddTo = 0;


        public GameManager(IPlayerManager _pm, ILogger<GameManager> logger)
        {
            for (int i = 0; i < updateWorkers.Length; i++)
            {
                updateWorkers[i] = new UpdateWorker(deltaTarget, _pm, logger, i.ToString());
            }
        }

        public Task Connected(IServerStreamWriter<Players> responseStream, CancellationToken cancellation)
        {
            var count = Interlocked.Increment(ref nextUpdateWorkerIndexToAddTo) - 1;
            var index = count % updateWorkers.Length;
            Console.WriteLine($"added connection: {count}, to worker: #{index}");
            return updateWorkers[index].AddConnection(responseStream, cancellation);
        }



        private class UpdateWorker
        {
            private readonly ILogger _logger;
            private readonly IPlayerManager _pm;
            private readonly List<ClientInfo> connections = [];
            private readonly List<ClientInfo> connectionsToRemove = [];
            private readonly object connectionsToAddLock = new();
            private readonly List<ClientInfo> connectionsToAdd = [];
            private readonly TimeSpan _deltaTarget;

            private readonly FieldInfo repeatedField_array;
            private readonly FieldInfo repeatedField_count;

            private readonly string _workerName;

            public UpdateWorker(TimeSpan deltaTarget, IPlayerManager pm, ILogger logger, string workerName = "")
            {
                _logger = logger;
                _pm = pm;
                _workerName = workerName;
                _deltaTarget = deltaTarget;
                Thread gameLoop = new(async () => await ProcessQueue());
                gameLoop.Start();

                Type listType = typeof(RepeatedField<Player>);
                repeatedField_array = listType.GetField("array", BindingFlags.NonPublic | BindingFlags.Instance)!;
                repeatedField_count = listType.GetField("count", BindingFlags.NonPublic | BindingFlags.Instance)!;
            }



            /// <summary>
            ///  Await this so the connection won't get disconnected.
            /// </summary>
            public Task AddConnection(IServerStreamWriter<Players> responseStream, CancellationToken cancellation)
            {
                var connection = new ClientInfo(responseStream, cancellation);
                lock (connectionsToAddLock)
                {
                    connectionsToAdd.Add(connection);
                }
                return connection.TaskCompletionSource.Task;
            }



            private async Task ProcessQueue()
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                while (true)
                {
                    var elapsed = stopwatch.Elapsed;
                    var delta = _deltaTarget - elapsed;
                    stopwatch.Restart();
                    if (delta > TimeSpan.Zero)
                    {
                        await Task.Delay(delta);
                    }
                    try
                    {
                        await UpdateClients();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error in processing queue.");
                    }
                }
            }

            private async Task UpdateClients()
            {
                lock (connectionsToAddLock)
                {
                    foreach (var connection in connectionsToAdd)
                    {
                        connections.Add(connection);
                    }
                    connectionsToAdd.Clear();
                }

                // ArrayPool<Player>.Shared.Rent(50);
                var p = _pm.GetPlayersAsArray();
                // for _pm.GetPlayersAsArray();
                // up to .2ms at 250 entities and 250 connections, PlayerManager (V1) with concurrency
                // up to 1ms at 100 connections, even with 1100 entities with PlayerManagerV3 (no concurrency) it's up to 2ms
                var print = true;
                Stopwatch stopwatch = Stopwatch.StartNew();
                foreach (var connection in connections)
                // about .5 ms per connection with PlayerManagerV3 (1.1k entities and 100 connections, no concurrency overhead)
                // about .5 ms per connection with PlayerManager (V1) (100 entities and 100 connections, with concurrency updater)
                // about .5 ms per connection with bumps up to 40 ms with PlayerManager (V1) (250 entities and 250 connections)
                {
                    var players = new Players();
                    repeatedField_array.SetValue(players.Players_, p);
                    repeatedField_count.SetValue(players.Players_, p.Length);
                    if (connection.Cancellation.IsCancellationRequested)
                    {
                        connectionsToRemove.Add(connection);
                        continue;
                    }
                    await connection.ResponseStream.WriteAsync(players); // most of the time is spent here

                    print = false;
                }
                double elapse = stopwatch.Elapsed.TotalMicroseconds;
                // if (elapse > 250*1000) 
                Console.WriteLine($"→→→{_workerName}→: {elapse/1000} ms");

                foreach (var connection in connectionsToRemove)
                {
                    connection.TaskCompletionSource.SetResult();
                    connections.Remove(connection);
                }

                connectionsToRemove.Clear();
            }
        }



        //public ClientInfo()
        //{
        //    TaskCompletionSource = new TaskCompletionSource();
        //}
        public readonly record struct ClientInfo(IServerStreamWriter<Players> ResponseStream, CancellationToken Cancellation)
        {
            public TaskCompletionSource TaskCompletionSource { get; } = new();
        }
    }
}
