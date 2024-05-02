using Godot;
using ProximitySync;
using System;

namespace Game;

public partial class PlayerNode : Node3D
{
    /// <summary>
    /// When true this node will be controlled by the player.
    /// </summary>
    [Export]
    private bool isUser = false;


    private Vector3 velocity = new();
    private float speed = 5.0f;
    private Camera3D camera;


    public override void _Ready()
    {
        camera = GetNode<Camera3D>("Camera3D");
    }


    public override void _PhysicsProcess(double delta)
    {
        if (!isUser)
            return;


        var direction = new Vector3();

        if (Input.IsActionPressed("player_w"))
            direction.Z += 1;
        if (Input.IsActionPressed("player_a"))
            direction.X += 1;
        if (Input.IsActionPressed("player_s"))
            direction.Z -= 1;
        if (Input.IsActionPressed("player_d"))
            direction.X -= 1;

        direction = direction.Normalized();

        velocity = direction * speed;

        // not extending kinematic body
        //velocity = MoveAndSlide(velocity);

        // Adjust the position directly since we don't have MoveAndSlide
        TranslateObjectLocal((velocity * (float)delta));
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.Right && mouseButtonEvent.IsPressed())
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
            else if (mouseButtonEvent.ButtonIndex == MouseButton.Right && !mouseButtonEvent.IsPressed())
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
        }

        if (@event is InputEventMouseMotion mouseMotionEvent && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            RotateY(Mathf.DegToRad(-mouseMotionEvent.Relative.X * 0.1f));
        }
    }

    /// <summary>
    /// Updates the player (from server)
    /// </summary>
    public void Update(float x, float z)
    {
        if (isUser)
            return;
        Position = new Vector3(x, Position.Y, z);
    }

}
