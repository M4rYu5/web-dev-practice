namespace ProximitySync.Data
{
    public class PlayerManager
    {
        public static readonly PlayerManager Instance = new();

        private readonly Dictionary<string, Player> _players = [];


        private PlayerManager() { }


        public IEnumerable<Player> GetPlayers()
        {
            return _players.Values;
        }

        public void AddPlayer(Player player)
        {
            if(_players.ContainsKey(player.Name))
            {
                _players[player.Name] = player;
            }
            _players.Add(player.Name, player);
        }

        public void RemovePlayer(string playerName)
        {
            if(_players.ContainsKey(playerName))
            {
                _players.Remove(playerName);
            }
        }

        public bool Contains(Player player) => Contains(player.Name);

        public bool Contains(string playerName)
        {
            return _players.ContainsKey(playerName);
        }
    }
}
