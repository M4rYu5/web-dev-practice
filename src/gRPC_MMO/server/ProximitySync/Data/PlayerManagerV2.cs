using System.Collections.Concurrent;

namespace ProximitySync.Data
{


    /// <summary>
    /// Note: This class is used to test if by changing ConcurrentDictionary with a custom implementation the amount of concurrent players to update will increase.
    /// </summary>
    public class PlayerManagerV2 : IPlayerManager
    {
        public static readonly PlayerManagerV2 Instance = new();

        private readonly ReaderWriterLockSlim _lock = new();
        private readonly Dictionary<string, Player> _players = [];


        //private readonly ConcurrentDictionary<string, DateTime> _playersLastUpdate = [];


        private PlayerManagerV2()
        {
            //MonitorAndRemoveInactivePlayers(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        }


        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                var c = _players.Count;
                _lock.ExitReadLock();
                return c;
            }
        }


        public Player[] GetPlayers()
        {
            _lock.EnterReadLock();
            var list = _players.Values.ToArray();
            _lock.ExitReadLock();
            return list;
        }

        /// <summary>
        /// Adds or updates the player
        /// </summary>
        public void AddPlayer(Player player)
        {
            _lock.EnterWriteLock();
            _players[player.Name] = player;
            _lock.ExitWriteLock();

            //_players.AddOrUpdate(player.Name, (string key) => player, (string key, Player p) => player);
            //_playersLastUpdate[player.Name] = DateTime.Now;
        }

        public void RemovePlayer(string playerName)
        {
            _lock.EnterWriteLock();
            _players.Remove(playerName);
            _lock.ExitWriteLock();

            //_players.TryRemove(playerName, out _);
            //_playersLastUpdate.TryRemove(playerName, out _);
        }

        public bool Contains(Player player) => Contains(player.Name);

        public bool Contains(string playerName)
        {
            _lock.EnterReadLock();
            var b = _players.ContainsKey(playerName);
            _lock.ExitReadLock();
            return b;
        }

        /// <summary>
        /// Updates or add the player
        /// </summary>
        public void Update(Player request)
        {
            _lock.EnterWriteLock();
            _players[request.Name] = request;
            _lock.ExitWriteLock();

            //_players.AddOrUpdate(request.Name, request, (key, oldValue) => request);
            //_playersLastUpdate[request.Name] = DateTime.Now;
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            _players.Clear();
            _lock.ExitWriteLock();
        }

        ///// <summary>
        ///// Monitors and removes inactive players from the game.
        ///// </summary>
        ///// <param name="inactivityCheckInterval">The interval at which player activity is checked</param>
        ///// <param name="inactivityThreshold">The duration of inactivity after which a player is considered inactive and removed</param>
        //private void MonitorAndRemoveInactivePlayers(TimeSpan inactivityCheckInterval, TimeSpan inactivityThreshold)
        //{
        //    Task.Run(async () =>
        //    {
        //        DateTime currentTime;
        //        while (true)
        //        {
        //            currentTime = DateTime.Now;
        //            await Task.Delay(inactivityCheckInterval);
        //            foreach (var playerUpdate in _playersLastUpdate)
        //            {
        //                if (currentTime - playerUpdate.Value > inactivityThreshold)
        //                {
        //                    RemovePlayer(playerUpdate.Key);
        //                }
        //            }
        //        }
        //    });
        //}
    }
}
