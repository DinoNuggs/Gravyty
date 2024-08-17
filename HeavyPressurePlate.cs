using Godot;
using System;

public partial class HeavyPressurePlate : PressurePlate
{
    [Export] public float weightThreshold = 100;
    
    public override void _Process(double delta) {
        base._Process(delta);
    }
}
