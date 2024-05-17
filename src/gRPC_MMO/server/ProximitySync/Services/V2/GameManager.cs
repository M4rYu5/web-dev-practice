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
    public class GameManager
    {
        private readonly IPlayerManager _pm;
        private readonly TimeSpan deltaTarget = TimeSpan.FromMilliseconds(500);
        private readonly UpdateWorker[] updateWorkers = new UpdateWorker[5];

        private int nextUpdateWorkerIndexToAddTo = 0;


        public GameManager(IPlayerManager _pm)
        {
            this._pm = _pm;
            for (int i = 0; i < updateWorkers.Length; i++)
            {
                updateWorkers[i] = new UpdateWorker(deltaTarget, _pm);
            }
        }

        public Task Connected(IServerStreamWriter<Players> responseStream, CancellationToken cancellation)
        {
            var index = Interlocked.Increment(ref nextUpdateWorkerIndexToAddTo) - 1;
            Console.WriteLine(index);
            return updateWorkers[index % updateWorkers.Length].AddConnection(responseStream, cancellation);
        }



        private class UpdateWorker
        {

            private readonly IPlayerManager _pm;
            private readonly List<ClientInfo> connections = [];
            private readonly List<ClientInfo> connectionsToRemove = [];
            private readonly object connectionsToAddLock = new();
            private readonly List<ClientInfo> connectionsToAdd = [];
            private readonly TimeSpan deltaTarget;

            public UpdateWorker(TimeSpan deltaTarget, IPlayerManager _pm)
            {
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
                    else
                    {
                        await connection.ResponseStream.WriteAsync(players);
                    }
                }

                foreach(var connection in connectionsToRemove)
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
