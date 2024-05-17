using Grpc.Core;
using ProximitySync.Data;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Diagnostics;
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
                updateWorkers[i] = new UpdateWorker(deltaTarget, _pm, logger);
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
            private readonly TimeSpan deltaTarget;

            public UpdateWorker(TimeSpan deltaTarget, IPlayerManager _pm, ILogger logger)
            {
                _logger = logger;
                this._pm = _pm;
                this.deltaTarget = deltaTarget;
                Thread gameLoop = new(async () => await ProcessQueue());
                gameLoop.Start();
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
                    var delta = deltaTarget - elapsed;
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

                foreach (var connection in connections)
                {
                    var players = new Players();
                    players.Players_.AddRange(_pm.GetPlayers());
                    if (connection.Cancellation.IsCancellationRequested)
                    {
                        connectionsToRemove.Add(connection);
                        continue;
                    }
                    await connection.ResponseStream.WriteAsync(players);
                }

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
