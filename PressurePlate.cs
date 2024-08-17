using Godot;
using System;

public partial class PressurePlate : Area2D
{
	[Export] public float weightThreshold = 0;
	[Signal] public delegate void ButtonPressedEventHandler();
	private bool wasPressed = false;
	private bool wasReleased = false;
	private PhysicsBody2D bodySteppingOnButton = null;
	private Frank frank;
	private float height;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		height = GetNode<Sprite2D>("Sprite2D").Texture.GetHeight();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(wasPressed && frank.GetWeight() > weightThreshold) {
			Scale = new Vector2(Scale.X, Scale.Y  * 0.25f);
			GD.Print("Emitting ", SignalName.ButtonPressed);
			EmitSignal(SignalName.ButtonPressed);
			wasPressed = false;
		}

		if(wasReleased) {
			Scale = new Vector2(1, 1);
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
		wasReleased = true;
	}

	public void OnWTF() {
		GD.Print("pleasse");
	}
}
