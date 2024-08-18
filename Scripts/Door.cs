using Godot;
using System;

public partial class Door : AnimatableBody2D
{
	[Export] public float doorSpeed = 10f;

	private bool open = false;
	private float height;
	private CollisionShape2D collider;
	private AnimationPlayer animation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		height = collider.Shape.GetRect().Size.Y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void OnOpenSignal() {
		if(!open){ animation.Play("SlideUp"); }
		open = true;
	}
}
