using Godot;
using System;

public partial class Cherry : Sprite2D
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
		var BobbingStrength = .5f;
		var BobbingSpeed = 15f;

		CurrentBobbing += BobbingSpeed * (float)delta;
		var bobbing = (float)Math.Sin(CurrentBobbing) * BobbingStrength;
		Position = new Vector2(Position.X, bobbing + StartPosition.Y);
	}
}
