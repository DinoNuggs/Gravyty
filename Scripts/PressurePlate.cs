using Godot;
using System;

public partial class PressurePlate : Area2D
{
	[Export] public float weightThreshold = 0;
	[Export] public bool oneShot = false;
	[Signal] public delegate void ButtonPressedEventHandler();
	private bool wasPressed = false;
	private bool wasReleased = false;
	private PhysicsBody2D bodySteppingOnButton = null;
	private AnimatedSprite2D spriteAnimator;
	private AnimationPlayer physicsAnimator;
	private Frank frank;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spriteAnimator = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		physicsAnimator = GetNode<AnimationPlayer>("StaticBody2D/AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(wasPressed && frank.GetWeight() > weightThreshold) {
			spriteAnimator.Play("pressed");
			physicsAnimator.Play("pressed");
			GD.Print("Emitting");
			EmitSignal(SignalName.ButtonPressed);
			wasPressed = false;
		}

		if(wasReleased) {
			spriteAnimator.Play("notpressed");
			physicsAnimator.Play("RESET");
			wasReleased = false;
		}
	}

	public void OnAreaEntered(Area2D area) {
		GD.Print(area.Name);
		bodySteppingOnButton = area.GetParent<PhysicsBody2D>();
		frank = area.GetParentOrNull<Frank>();
		wasPressed = true;
	}

	public void OnAreaExited(Area2D area) {
		bodySteppingOnButton = null;
		if(oneShot){
			return;
		}
		
		wasReleased = true;
	}

	public void OnWTF() {
		GD.Print("pleasse");
	}
}
