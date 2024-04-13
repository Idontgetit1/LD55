using Godot;
using System;

public enum SummonType {
	Wolf,
	Slime
}

public partial class summon : CharacterBody2D
{
	private Game Game;

	// Summon Attributes
	public SummonType Type;
	public bool IsPlayer;
	public int FieldIndex;
	public Marker2D FieldMarker;

	// Buffer for next Tick
	public bool ShouldMoveNextTick = false;
	public Vector2 NextPosition;
	public int NextFieldIndex;
	public Marker2D NextFieldMarker;
	public int TakingDamage = 0;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");

		GetNode<Label>("NameTag").Text = "summon";
	}

	public override void _Process(double delta)
	{
	}

	public void Tick() {
		// Placeholder
	}

	public bool ShouldMove() {
		// Placeholder
		return false;
	}

	public void Move(Vector2 Position) {
		// Placeholder
	}

	public Marker2D GetMarker() {
		// Placeholder
		return null;
	}

	public void GetReadyToAttack() {
		// Placeholder
	}

	public void Attack() {
		// Placeholder
	}

	public void TakeDamage(int Damage) {
		// Placeholder
	}

	public void Die() {
		// Placeholder for cool death animation
		QueueFree();
	}
}
