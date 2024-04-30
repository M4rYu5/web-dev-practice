using Godot;
using ProximitySync;
using System;

namespace Game;

public partial class PlayerNode : Node3D
{
	public void Update(float x, float z)
	{
		Position = new Vector3(x, Position.Y, z);
	}
}
