using System.Collections.Concurrent;

namespace ProximitySync.Data
{
    public class PlayerManager : IPlayerManager
    {
        private readonly ConcurrentDictionary<string, Player> _players = [];
        private readonly ConcurrentDictionary<string, DateTime> _playersLastUpdate = [];


        public PlayerManager()
        {
            MonitorAndRemoveInactivePlayers(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        }


        public int Count { get => _players.Count; }


        public ICollection<Player> GetPlayers()
        {
            return _players.Values;
        }

        /// <summary>
        /// Adds or updates the player
        /// </summary>
        public void AddPlayer(Player player)
        {
            _players.AddOrUpdate(player.Name, (string key) => player, (string key, Player p) => player);
            _playersLastUpdate[player.Name] = DateTime.Now;
        }

        public void RemovePlayer(string playerName)
        {
            _players.TryRemove(playerName, out _);
            _playersLastUpdate.TryRemove(playerName, out _);
        }

        public bool Contains(Player player) => Contains(player.Name);

        public bool Contains(string playerName)
        {
            return _players.ContainsKey(playerName);
        }

        /// <summary>
        /// Updates or add the player
        /// </summary>
        public void Update(Player request)
        {
            _players.AddOrUpdate(request.Name, request, (key, oldValue) => request);
            _playersLastUpdate[request.Name] = DateTime.Now;
        }

        public void Clear()
        {
            _players.Clear();
        }

        /// <summary>
        /// Monitors and removes inactive players from the game.
        /// </summary>
        /// <param name="inactivityCheckInterval">The interval at which player activity is checked</param>
        /// <param name="inactivityThreshold">The duration of inactivity after which a player is considered inactive and removed</param>
        private void MonitorAndRemoveInactivePlayers(TimeSpan inactivityCheckInterval, TimeSpan inactivityThreshold)
        {
            Task.Run(async () =>
            {
                DateTime currentTime;
                while (true)
                {
                    currentTime = DateTime.Now;
                    await Task.Delay(inactivityCheckInterval);
                    foreach (var playerUpdate in _playersLastUpdate)
                    {
                        if (currentTime - playerUpdate.Value > inactivityThreshold)
                        {
                            RemovePlayer(playerUpdate.Key);
                        }
                    }
                }
            });
        }
    }
}
