using Godot;
using System;

public partial class Rocket : StaticBody2D
{
	private AnimationPlayer animator;
	private bool launched = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animator = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnLaunchSignal() {
		if( !launched ) {
			animator.Play("launch");
		}
	}
}
