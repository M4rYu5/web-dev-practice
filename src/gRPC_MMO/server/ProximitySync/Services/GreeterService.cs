using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProximitySync;

namespace ProximitySync.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }


    public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        while (true)
        {
            await Task.Delay(500);
            await responseStream.WriteAsync(new HelloReply() { Message = "Hello" });
        }
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
