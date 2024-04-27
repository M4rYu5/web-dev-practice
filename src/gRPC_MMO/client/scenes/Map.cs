using Godot;
using System;
using Grpc.Net.Client;
using ProximitySync;

public partial class Map : Node3D
{



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// The port number must match the port of the gRPC server.
		using var channel = GrpcChannel.ForAddress("https://localhost:7197");
		var client = new Greeter.GreeterClient(channel);
		var reply = client.SayHello(
						new HelloRequest { Name = "GreeterClient" });
		GD.Print("Greeting: " + reply.Message);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
