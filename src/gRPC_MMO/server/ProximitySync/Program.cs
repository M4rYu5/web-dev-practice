using ProximitySync.Data;
using ProximitySync.Services;
using ProximitySync.Services.V2;
using System.Diagnostics;



// CONFIG

const int GRPC_IMPLEMENTATION_VERSION = 2;
PlayerManager _pmVersion = new(); // can be changed to another implementation eg. PlayerManagerV3



// APP

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPlayerManager>(_pmVersion);
if (GRPC_IMPLEMENTATION_VERSION == 2)
{
    builder.Services.AddSingleton<GameManager>();
}

builder.Services.AddGrpc(x =>
{
    x.EnableDetailedErrors = false;
    x.MaxSendMessageSize = null;
    x.MaxReceiveMessageSize = null;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
#pragma warning disable CS0162 // Unreachable code detected
if (GRPC_IMPLEMENTATION_VERSION == 1)
{
    app.MapGrpcService<ProximityService>();
}
else if (GRPC_IMPLEMENTATION_VERSION == 2)
{
    app.MapGrpcService<ProximityServiceV2>();
}
#pragma warning restore CS0162 // Unreachable code detected


app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();