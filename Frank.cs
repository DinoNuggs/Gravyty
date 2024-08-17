using Godot;
using System;

public partial class Frank : CharacterBody2D
{
	public float Speed = 300.0f;
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
	[Export] public float mininumScale = 0.1f;
	[Export] public float maximumScale = 10f;
	[Export] public float mass = 10f;

	private CollisionObject2D headClearanceBox;
	private RayCast2D headClearanceRay;
	private float targetScale;
	private float weight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		headClearanceBox = GetNode<CollisionObject2D>("HeadClearanceBox");
		headClearanceRay = GetNode<RayCast2D>("RayCast2D");
		targetScale = Scale.Length();
		weight = Scale.Length()*Scale.Length() * mass;
	}
	public override void _PhysicsProcess(double delta)
	{
		// GD.Print("Current Weight: " + weight);
		Vector2 velocity = Velocity;
		float scaleModifier = Scale.Length()/1.4142135f;
		weight = Scale.Length()*Scale.Length() * mass;
		
		targetScale = Math.Clamp(targetScale, mininumScale, maximumScale);

		if (targetScale > Scale.Length() && !headClearanceRay.IsColliding()) {
			Scale += new Vector2(ScalingRate * (float)delta, ScalingRate * (float)delta);
		}

		if (targetScale < Scale.Length()) {
			Scale -= new Vector2(ScalingRate * (float)delta, ScalingRate * (float)delta);
		}

		if (Input.IsActionJustReleased("scale_up")) {
			if (headClearanceRay.IsColliding()) {
				targetScale = Scale.Length(); // Remove any excess scalign from buffer
				return;
			}
			targetScale *= 1.2f;
		}
		if (Input.IsActionJustReleased("scale_down")) {
			targetScale *= 0.8f;
		}

		if (Input.IsActionPressed("ui_accept"))
		{
			velocity.Y -= 400 * Floatiness * scaleModifier * (float)delta;
		}
		
		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity * scaleModifier;
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * scaleModifier * (float)delta;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");

		// Grounded
		if (IsOnFloor()) {
			if (direction != Vector2.Zero)
			{
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed * scaleModifier, Acceleration * scaleModifier);
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * scaleModifier / Slidiness);
			}

			// Cap run speed
			if(Math.Abs(velocity.X) > MaxFloorSpeed)
			{
				GD.Print("cappin");
				velocity.X = Mathf.MoveToward(Velocity.X, MaxFloorSpeed * direction.X * scaleModifier, Speed);
			}
		} else {
			// In air, should be harder to change direction
			if (direction != Vector2.Zero)
			{
				GD.Print("in air, moving");
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed * scaleModifier, AirMobility);
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * scaleModifier / AirInertia);
			}
			// Cap horizontal air speed
			if(Math.Abs(velocity.X) > MaxFloorSpeed)
			{
				GD.Print("cappin");
				velocity.X = Mathf.MoveToward(Velocity.X, MaxFloorSpeed * direction.X * scaleModifier, Speed);
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void OnHeadClearanceBoxEnter() {
		GD.Print("bonking");
	}

	public float GetWeight() {
		return weight;
	}
}
