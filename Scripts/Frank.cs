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

	private RayCast2D headClearanceRay;
	private AnimatedSprite2D animator;
	private float targetScale;
	private float weight;
	private bool canScaleUp = true;
	private bool leftBlocked = false;
	private bool rightBlocked = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		headClearanceRay = GetNode<RayCast2D>("HeadRay");
		animator = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		targetScale = Scale.X;
		weight = Scale.Length()*Scale.Length() * mass;
	}
	public override void _PhysicsProcess(double delta)
	{
		// GD.Print("Current Weight: " + weight);
		Vector2 velocity = Velocity;
		float scaleModifier = (float) Math.Log10(Scale.X/1.414) + 1;
		//float scaleModifier = Scale.X/1.4142135f;
		
		weight = Scale.Length()*Scale.Length() * mass;
		canScaleUp = true;
		
		if (headClearanceRay.IsColliding()) {
			//Cant scale if bonking above his head
			canScaleUp = false;
			targetScale = Scale.X;
		}

		if ( leftBlocked && rightBlocked ) {
			//Cant scale if frank is squeezed on both sides
			canScaleUp = false;
			targetScale = Scale.X;
		}

		if (Input.IsActionJustReleased("scale_up")) {
			if (canScaleUp) {
				targetScale *= 1.2f;
			}
			
		}
		if (Input.IsActionJustReleased("scale_down")) {
			targetScale *= 0.8f;
		}

		targetScale = Math.Clamp(targetScale, mininumScale, maximumScale);
		GD.Print("Scale: ", Scale, "\tTarget scale: ", targetScale, "\tScale modifier: ", scaleModifier);

		if (targetScale > Scale.X && canScaleUp) {
			Scale += new Vector2(ScalingRate * (float)delta, ScalingRate * (float)delta);
		}

		if (targetScale < Scale.X) {
			Scale -= new Vector2(ScalingRate * (float)delta, ScalingRate * (float)delta);
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
			animator.SpeedScale = 1/scaleModifier;
			if (direction != Vector2.Zero)
			{
				animator.Play("walk");

				if (direction.X < 0) {
					animator.FlipH = true;
				} else {
					animator.FlipH = false;
				}
				
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed * scaleModifier, Acceleration * scaleModifier);
			}
			else
			{
				animator.Play("idle");
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
			animator.Play("airborne");
			if ( Velocity.X < 0 ) {
				animator.FlipH = true;
			} else {
				animator.FlipH = false;
			}
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

	public void OnScaleChange(float val) {
		GD.Print(val);
		float ratioForScale = val/100;
		targetScale = Math.Clamp(ratioForScale * 1.414f * maximumScale, mininumScale, maximumScale);
	}

	public void OnLeftDetected(Node2D body) {
		if(body.Name != "Frank") {
			leftBlocked = true;
		}
	}

	public void OnLeftExitDetected(Node2D body) {
		if(body.Name != "Frank") {
			leftBlocked = false;
		}
	}

	public void OnRightDetected(Node2D body) {
		if(body.Name != "Frank") {
			rightBlocked = true;
		}
	}

	public void OnRightExitDetected(Node2D body) {
		if(body.Name != "Frank") {
			rightBlocked = false;
		}
	}

	public float GetWeight() {
		return weight;
	}
}
