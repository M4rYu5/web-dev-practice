using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProximitySync;
using ProximitySync.Data;

namespace ProximitySync.Services;


public class ProximityService : ProximityUpdater.ProximityUpdaterBase
{
    private readonly PlayerManager _pm = PlayerManager.Instance;

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
            players.Players_.AddRange(_pm.GetPlayers());
            await responseStream.WriteAsync(players);
        }
    }
}
