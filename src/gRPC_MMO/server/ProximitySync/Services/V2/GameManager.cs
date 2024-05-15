using Grpc.Core;
using ProximitySync.Data;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace ProximitySync.Services.V2
{

    /// <summary>
    /// Main purpose for this class is to test the performance of using multiple threads to write the response to clients.
    /// </summary>
    public static class GameManager
    {
        private static readonly PlayerManager _pm = PlayerManager.Instance;

        private static readonly TimeSpan deltaTarget = TimeSpan.FromMicroseconds(500);
        private static readonly UpdateWorker[] updateWorkers = new UpdateWorker[5];

        private static int nextUpdateWorkerIndexToAddTo = 0;


        static GameManager()
        {
            for (int i = 0; i < updateWorkers.Length; i++)
            {
                updateWorkers[i] = new UpdateWorker(deltaTarget);
            }
        }

        public static Task Connected(IServerStreamWriter<Players> responseStream, CancellationToken cancellation)
        {
            var index = Interlocked.Increment(ref nextUpdateWorkerIndexToAddTo) - 1;
            Console.WriteLine(index);
            return updateWorkers[index % updateWorkers.Length].AddConnection(responseStream, cancellation);
        }



        private class UpdateWorker
        {
            private readonly List<ClientInfo> connections = [];
            private readonly List<ClientInfo> connectionsToRemove = [];
            private readonly object connectionsToAddLock = new();
            private readonly List<ClientInfo> connectionsToAdd = [];
            private readonly TimeSpan deltaTarget;

            public UpdateWorker(TimeSpan deltaTarget)
            {
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
                DateTime lastUpdate = DateTime.Now;
                TimeSpan delta;
                while (true)
                {
                    delta = deltaTarget - (DateTime.Now - lastUpdate);
                    lastUpdate = DateTime.Now;
                    if (delta > TimeSpan.Zero)
                    {
                        Thread.Sleep(delta);
                    }
                    try
                    {
                        await UpdateClients();
                    }
                    catch (Exception) { }
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

                foreach(var connection in connections)
                {
                    var players = new Players();
                    players.Players_.AddRange(_pm.GetPlayers());
                    if (connection.Cancellation.IsCancellationRequested)
                    {
                        connectionsToRemove.Add(connection);
                        connection.TaskCompletionSource.SetResult();
                        return;
                    }
                    await connection.ResponseStream.WriteAsync(players);
                }

                foreach(var connection in connectionsToRemove)
                {
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
