using System.Collections.Concurrent;

namespace ProximitySync.Data
{


    /// <summary>
    /// Note: This class is used to test if by removing it's functionality and returning a static list every time.
    /// </summary>
    public class PlayerManagerV3 : IPlayerManager
    {
        public static readonly PlayerManagerV3 Instance = new();

        private readonly List<Player> _players = [];


        //private readonly ConcurrentDictionary<string, DateTime> _playersLastUpdate = [];


        private PlayerManagerV3()
        {
            for (int i = 0; i < 1000; i++)
            {
                _players.Add(new Player());
            }
            //MonitorAndRemoveInactivePlayers(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        }


        public int Count { get => _players.Count; }


        public List<Player> GetPlayers()
        {
            return _players;
        }

        /// <summary>
        /// Adds or updates the player
        /// </summary>
        public void AddPlayer(Player player)
        {

            //_players.AddOrUpdate(player.Name, (string key) => player, (string key, Player p) => player);
            //_playersLastUpdate[player.Name] = DateTime.Now;
        }

        public void RemovePlayer(string playerName)
        {

            //_players.TryRemove(playerName, out _);
            //_playersLastUpdate.TryRemove(playerName, out _);
        }

        public bool Contains(Player player) => Contains(player.Name);

        public bool Contains(string playerName)
        {
            return true;
        }

        /// <summary>
        /// Updates or add the player
        /// </summary>
        public void Update(Player request)
        {

            //_players.AddOrUpdate(request.Name, request, (key, oldValue) => request);
            //_playersLastUpdate[request.Name] = DateTime.Now;
        }

        public void Clear()
        {
            
        }


    }
}
