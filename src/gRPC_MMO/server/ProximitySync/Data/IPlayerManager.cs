
namespace ProximitySync.Data
{
    public interface IPlayerManager
    {
        int Count { get; }

        void AddPlayer(Player player);
        void Clear();
        bool Contains(Player player);
        bool Contains(string playerName);
        ICollection<Player> GetPlayers();
        Player[] GetPlayersAsArray(); // just for testing (for now)
        void RemovePlayer(string playerName);
        void Update(Player request);
    }
}