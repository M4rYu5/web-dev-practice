using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProximitySync;

namespace ProximitySync.Services;

public class ProximityService : ProximityUpdater.ProximityUpdaterBase
{
    private readonly ILogger<ProximityService> _logger;
    public ProximityService(ILogger<ProximityService> logger)
    {
        _logger = logger;
    }


    public override async Task UpdatePlayers(Empty request, IServerStreamWriter<Players> responseStream, ServerCallContext context)
    {
        while (true)
        {
            await Task.Delay(500);
            var players = new Players();
            players.Players_.Add(new Player() { Name = "Test 1", Position = new Position2D { X = 0, Y = 0 } });
            await responseStream.WriteAsync(players);
        }
    }
}
