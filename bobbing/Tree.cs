using Godot;
using System;

public partial class Tree : Sprite2D
{
	public float CurrentBobbing = 0f;
	public Vector2 StartPosition;
	
	// Ready
	public override void _Ready()
	{
		StartPosition = Position;
	}

	// Bobbing effect
	public override void _Process(double delta)
	{
		var BobbingStrength = 50f;
		var BobbingSpeed = 0.5f;

		CurrentBobbing += BobbingSpeed * (float)delta;
		var bobbing = (float)Math.Sin(CurrentBobbing) * BobbingStrength;
		Position = new Vector2(bobbing + StartPosition.X, Position.Y);
	}
}
