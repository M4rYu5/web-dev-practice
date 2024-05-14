using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProximitySync;
using ProximitySync.Data;
using ProximitySync.Services.V2;

namespace ProximitySync.Services;


public class ProximityServiceV2(ILogger<ProximityService> logger) : ProximityUpdater.ProximityUpdaterBase
{
    private readonly PlayerManager _pm = PlayerManager.Instance;

    public override async Task UpdatePlayers(Empty request, IServerStreamWriter<Players> responseStream, ServerCallContext context)
    {
        await GameManager.Connected(responseStream, context.CancellationToken);
    }


    public override async Task<UpdateResponse> PlayerUpdate(Player request, ServerCallContext context)
    {
        // Note 2024-05-01: as of now we don't handle authentication. 
        // We're assuming that the update is coming from the actual player.
        if (!_pm.Contains(request.Name))
        {
            _pm.AddPlayer(request);
        }
        else
        {
            _pm.Update(request);
        }

        await Task.Delay(500);

        return new UpdateResponse();
    }

}
