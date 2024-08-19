using Godot;
using System;

public partial class Rocket : StaticBody2D
{
	[Export] public AudioStreamPlayer launchSound;
	private AnimationPlayer animator;
	private CpuParticles2D innerFire;
	private CpuParticles2D outerFire;
	private bool launched = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animator = GetNode<AnimationPlayer>("AnimationPlayer");
		innerFire = GetNode<CpuParticles2D>("CollisionPolygon2D/InnerFire");
		outerFire = GetNode<CpuParticles2D>("CollisionPolygon2D/OuterFire");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnLaunchSignal() {
		if( !launched ) {
			animator.Play("launch");
			launchSound.Play();
			innerFire.Emitting = true;
			outerFire.Emitting = true;
			launched = true;
		}
	}
}
