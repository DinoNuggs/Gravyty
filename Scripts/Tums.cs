using Godot;
using System;
using System.Timers;

public partial class Tums : Area2D
{
	[Export] public PackedScene winScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnFrankEntered(Area2D area) {
		// you win
		CharacterBody2D frank = area.GetParentOrNull<Frank>();
		
		if (frank.Name == "Frank") { GetTree().ChangeSceneToPacked(winScene); }
	}
}
