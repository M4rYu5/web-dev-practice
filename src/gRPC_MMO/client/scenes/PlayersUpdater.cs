using Godot;
using System;
using Grpc.Net.Client;
using ProximitySync;
using System.Threading;
using Grpc.Core;
using MMOgRPC.Services;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Frozen;

namespace Game;

public partial class PlayersUpdater : Node
{

	private PackedScene _playerPackedScene = (PackedScene)GD.Load("res://scenes/player.tscn");


	public override void _EnterTree()
	{

		ConnectionManager.Instance.PlayersStateUpdated += PlayersStateUpdated;
	}

	public override void _ExitTree()
	{
		ConnectionManager.Instance.PlayersStateUpdated -= PlayersStateUpdated;
	}


	private void PlayersStateUpdated(Players updates)
	{
		try
		{
			var playersNodes = GetChildren().Cast<PlayerNode>().ToDictionary(x => x.Name);
			HashSet<string> updatedNodes = [];
			foreach (var player in updates.Players_)
			{
				var name = player.Name;
				if (playersNodes.ContainsKey(name))
				{
					playersNodes[name].Update((float)player.Position.X, (float)player.Position.Y);
					updatedNodes.Add(name);
				}
				else
				{
					PlayerNode newPlayer = _playerPackedScene.Instantiate<PlayerNode>();
					newPlayer.Name = name;
					newPlayer.Update((float)player.Position.X, (float)player.Position.Y);
					AddChild(newPlayer);
				}

			}
			foreach (var oldPlayer in playersNodes.Values.Where(x => !updatedNodes.Contains(x.Name)))
			{
				oldPlayer.QueueFree();
			}
		}
		catch (Exception ex)
		{
			GD.Print(ex.ToString());
		}
	}


}
