using System.Diagnostics;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProximitySync;
using ProximitySync.Data;
using ProximitySync.Services.V2;

namespace ProximitySync.Services;


public class ProximityServiceV2(ILogger<ProximityService> logger, IPlayerManager _pm, GameManager _gm) : ProximityUpdater.ProximityUpdaterBase
{
    private readonly UpdateResponse updateResponse = new UpdateResponse();

    public override async Task UpdatePlayers(Empty request, IServerStreamWriter<Players> responseStream, ServerCallContext context)
    {
        await _gm.Connected(responseStream, context.CancellationToken);
    }


    private volatile bool isFirstSet = false;
    private volatile string name = "";
    private long last_Time = Stopwatch.GetTimestamp();

    public override async Task<UpdateResponse> PlayerUpdate(IAsyncStreamReader<Player> requestStream, ServerCallContext context)
    {
        // Note 2024-05-01: as of now we don't handle authentication. 
        // We're assuming that the update is coming from the actual player.

        // Console.WriteLine("received");
        // return new UpdateResponse();

        // return updateResponse;


        Stopwatch stopwatch = Stopwatch.StartNew();
        await foreach (var player in requestStream.ReadAllAsync()){
            stopwatch.Restart();
            if (name == player.Name){
                Console.WriteLine($"Player {player.Name} updated in {Stopwatch.GetElapsedTime(last_Time)}");
                last_Time = Stopwatch.GetTimestamp();
            }
            if(!isFirstSet){
                name = player.Name;
                isFirstSet = true;
            }


            if (!_pm.Contains(player.Name))
            {
                _pm.AddPlayer(player);
            }
            else
            {
                _pm.Update(player);
            }
            Console.WriteLine($"Added player in {stopwatch.Elapsed}");

            await Task.Delay(50);
        }

        return updateResponse;
    }

}
