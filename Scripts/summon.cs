using Godot;
using System;

public enum SummonType {
	Wolf,
	Slime
}

public partial class summon : CharacterBody2D
{
	[Export]
	public int MoveEvery = 1;
	[Export]
	public int Damage = 1;
	[Export]
	public int Health = 1;
	[Export]
	public int Area = 1;
	[Export]
	public SummonType Type = SummonType.Wolf;
	[Export]
	public bool IsPlayer = false;
	public Marker2D SummonMarker;

	private int TickCounter = 0;
	private bool Moving = false;
	private Vector2 MovingTo = new Vector2();

	public override void _Ready()
	{
		string Name = Type.ToString();

		if (!IsPlayer) {
			Name = "E:" + Name;
		}

		GetNode<Label>("NameTag").Text = Name;
	}

	public override void _Process(double delta)
	{
		if (Moving) {
			var diff = MovingTo - GlobalPosition;
			var dir = diff.Normalized();
			var speed = 500 * (float)delta;

			if (diff.Length() > speed) {
				GlobalPosition += dir * speed;
			} else {
				GlobalPosition = MovingTo;
				Moving = false;
			}
		}
	}

	public void Tick() {
		TickCounter++;
	}

	public bool ShouldMove() {
		return TickCounter % MoveEvery == 0;
	}

	public void Move(Vector2 Position) {
		Moving = true;
		MovingTo = Position;
	}

	public Marker2D GetMarker() {
		return SummonMarker;
	}

	public void GetReadyToAttack() {
		// Placeholder
	}

	public void Attack() {
		// Placeholder
	}

	public void TakeDamage(int Damage) {
		Health -= Damage;
		if (Health <= 0) {
			Die();
		}
	}

	public void Die() {
		// Placeholder for cool death animation
		QueueFree();
	}
}
