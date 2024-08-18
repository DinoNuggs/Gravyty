using Godot;
using System;
using System.Drawing;

public partial class falling_object : RigidBody2D
{
	[Export]
	public float Throwforce = 500;
	[Export]
	public float weightMultiplier = 1;
	[Export]
	public float pullForce = 500;
	private RigidBody2D _obj;
	private Vector2 mouseClickLocation;
	private Vector2 mouseReleaseLocation;
	private Vector2 currMouseLocation;
	private bool throwMode = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Mass *= weightMultiplier;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currMouseLocation = GetViewport().GetMousePosition();
		LinearDamp = 0;

		if ( throwMode ) {
			Vector2 dir = (currMouseLocation - GlobalPosition).Normalized();
			ApplyForce(dir * pullForce );
			LinearDamp = 75/(currMouseLocation - GlobalPosition).Length();
			GD.Print(LinearDamp);
		}
	}

	private void ButtonPressed() {
		throwMode = true;
		GravityScale = 0.0f;

		mouseClickLocation = GetViewport().GetMousePosition();
	}

	private void ButtonReleased() {
		throwMode = false;
		GravityScale = 0.6f;

		mouseReleaseLocation = GetViewport().GetMousePosition();
		ApplyImpulse(
			(mouseReleaseLocation - GlobalPosition).Normalized() * 
			Math.Min(Throwforce,
			(mouseReleaseLocation - GlobalPosition).Length()*10f
			));
	}
}
