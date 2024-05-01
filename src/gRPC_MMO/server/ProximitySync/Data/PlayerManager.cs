using System.Collections.Concurrent;

namespace ProximitySync.Data
{
    public class PlayerManager
    {
        public static readonly PlayerManager Instance = new();

        private readonly ConcurrentDictionary<string, Player> _players = [];


        private PlayerManager() { }


        public int Count {  get => _players.Count; }


        public ICollection<Player> GetPlayers()
        {
            return _players.Values;
        }

        public void AddPlayer(Player player)
        {
            _players.AddOrUpdate(player.Name, (string key) => player, (string key, Player p) => player);
        }

        public void RemovePlayer(string playerName)
        {
            _players.TryRemove(playerName, out _);
        }

        public bool Contains(Player player) => Contains(player.Name);

        public bool Contains(string playerName)
        {
            return _players.ContainsKey(playerName);
        }

        public void Update(Player request)
        {
            _players.AddOrUpdate(request.Name, request, (key, oldValue) => request);
        }

        public void Clear()
        {
            _players.Clear();
        }

        internal Player[] ToArray()
        {
            return _players.Values.ToArray();
        }
    }
}
