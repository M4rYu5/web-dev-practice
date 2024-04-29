using Godot;
using System;
using Grpc.Net.Client;
using ProximitySync;
using System.Threading;
using Grpc.Core;

public partial class Map : Node3D
{



	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		// The port number must match the port of the gRPC server.
		using var channel = GrpcChannel.ForAddress("https://localhost:7197");
		var client = new ProximityUpdater.ProximityUpdaterClient(channel);

		//var reply = client.SayHello(new HelloRequest { Name = "GreeterClient" });
		//await foreach (var response in reply.ResponseStream.ReadAllAsync())
		//{
		//	GD.Print("Greeting: " + response.Message);
		//}

		var positionUpdater = client.UpdatePlayers(new Google.Protobuf.WellKnownTypes.Empty());
		await foreach (var response  in positionUpdater.ResponseStream.ReadAllAsync())
		{
			GD.Print(System.Text.Json.JsonSerializer.Serialize(response.Players_));
		}


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
