using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProximitySync;
using ProximitySync.Data;

namespace ProximitySync.Services;


public class ProximityService(ILogger<ProximityService> logger) : ProximityUpdater.ProximityUpdaterBase
{
    private readonly PlayerManager _pm = PlayerManager.Instance;

    public override async Task UpdatePlayers(Empty request, IServerStreamWriter<Players> responseStream, ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(500);
                //DevTesting(_pm.GetPlayers());
                var players = new Players();
                players.Players_.AddRange(_pm.GetPlayers());
                if (context.CancellationToken.IsCancellationRequested)
                {
                    break;
                }
                await responseStream.WriteAsync(players);
            }
            catch (InvalidOperationException ex)
            {
                logger.Log(LogLevel.Error, ex, "Client disconnected");
                break;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex, "Something went wrong in updating the players");
            }
        }
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



    private void DevTesting(IEnumerable<Player> currentPlayers)
    {
        if (_pm.Count > 5000)
        {
            _pm.RemovePlayer(currentPlayers.ElementAt(Random.Shared.Next(0, _pm.Count - 1)).Name);
        }
        //for (int i = 0; i < currentPlayers.Length - 1; i++)
        foreach (var player in currentPlayers)
        //foreach (var player in currentPlayers.Where(x => Random.Shared.NextDouble() > .6))
        {
            if (Random.Shared.NextDouble() > .6)
                continue;
            player.Position.X += (Random.Shared.NextDouble() - .5) * 2;
            player.Position.Y += (Random.Shared.NextDouble() - .5) * 2;

            if (player.Position.X < -200 || player.Position.Y < -200 || player.Position.X > 200 || player.Position.Y > 200)
            {
                player.Position.X = 0;
                player.Position.Y = 0;
            }
        }
        _pm.AddPlayer(new Player()
        {
            Name = Random.Shared.Next().ToString(),
            Position = new Position2D()
            {
                X = Random.Shared.NextDouble() * 200 - 100,
                Y = Random.Shared.NextDouble() * 200 - 100,
            }
        });
        // starts with 500 entities
        while (_pm.Count < 500)
        {
            _pm.AddPlayer(new Player()
            {
                Name = Random.Shared.Next().ToString(),
                Position = new Position2D()
                {
                    X = Random.Shared.NextDouble() * 200 - 100,
                    Y = Random.Shared.NextDouble() * 200 - 100,
                }
            });
        }
    }
}
