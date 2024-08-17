using Godot;
using System;

public partial class Frank : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	[Export]
	public float Floatiness = 1;
	[Export]
	public float Slidiness = 1;
	[Export]
	public float AirInertia = 1;
	[Export]
	public float AirMobility = 1;
	[Export]
	public float MaxFloorSpeed = 1;
	[Export]
	public float Acceleration = 1;
	[Export]
	public float ScalingRate = 1;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (Input.IsActionJustReleased("scale_up")) {
			GD.Print("Scaling up", Scale);

			Scale = new Vector2(
				Mathf.MoveToward(Scale.X, Scale.X*1.2f, ScalingRate),
				Mathf.MoveToward(Scale.Y, Scale.Y*1.2f, ScalingRate));
		}

		if (Input.IsActionJustReleased("scale_down")) {
			Scale = new Vector2(
				Mathf.MoveToward(Scale.X, Scale.X*0.8f, ScalingRate),
				Mathf.MoveToward(Scale.Y, Scale.Y*0.8f, ScalingRate));
		}

		if (Input.IsActionPressed("ui_accept"))
		{
			velocity.Y -= 400 * Floatiness * (float)delta;
		}
		
		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");

		// Grounded
		if (IsOnFloor()) {
			if (direction != Vector2.Zero)
			{
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed, Acceleration);
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed / Slidiness);
			}

			// Cap run speed
			if(Math.Abs(velocity.X) > MaxFloorSpeed)
			{
				GD.Print("cappin");
				velocity.X = Mathf.MoveToward(Velocity.X, MaxFloorSpeed * direction.X, Speed);
			}
		} else {
			// In air, should be more harder to change direction
			if (direction != Vector2.Zero)
			{
				GD.Print("in air, moving");
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed, AirMobility);
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed / AirInertia);
			}
			// Cap horizontal air speed
			if(Math.Abs(velocity.X) > MaxFloorSpeed)
			{
				GD.Print("cappin");
				velocity.X = Mathf.MoveToward(Velocity.X, MaxFloorSpeed * direction.X, Speed);
			}
		}
		

		Velocity = velocity;
		MoveAndSlide();
	}
}
