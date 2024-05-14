using ProximitySync.Services;
using System.Diagnostics;

const int GRPC_IMPLEMENTATION_VERSION = 2;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

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
