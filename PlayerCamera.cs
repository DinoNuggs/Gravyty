using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
	Vector2 FrankScale;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		FrankScale = GetParent().GetParent<CharacterBody2D>().Scale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		FrankScale = GetParent<CharacterBody2D>().Scale;
		Zoom = new Vector2(1/FrankScale.X, 1/FrankScale.Y);
		
	}
}
