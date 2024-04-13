using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

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
	public int AttackPower = 1;
	public int Health = 3;

	// Buffer for next Tick
	public bool ShouldMoveNextTick = false;
	public Vector2 NextPosition;
	public int NextFieldIndex;
	public Marker2D NextFieldMarker;
	public int DamageTaken = 0;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");

		GetNode<Label>("NameTag").Text = "summon";
	}

	public override void _Process(double delta)
	{
	}

	public void Act() {
		// Acting
		if (ShouldMoveNextTick) {
			ShouldMoveNextTick = false;
			FieldIndex = NextFieldIndex;
			FieldMarker = NextFieldMarker;
			GlobalPosition = NextPosition;
		}

		if (DamageTaken > 0) {
			TakeDamage(DamageTaken);
			DamageTaken = 0;
		}
	}

    public void PlanMove()
    {
		// Check if is at end of field, if not move forward (depending on IsPlayer)
		if (IsPlayer) {
			if (FieldIndex < Game.Fields - 1) {
				NextFieldIndex = FieldIndex + 1;
				NextFieldMarker = Game.FieldMarkers[NextFieldIndex];
				NextPosition = NextFieldMarker.GlobalPosition;
				ShouldMoveNextTick = true;

				GD.Print("Player is moving from " + FieldIndex + " to " + NextFieldIndex);
			}
		} else {
			if (FieldIndex > 0) {
				NextFieldIndex = FieldIndex - 1;
				NextFieldMarker = Game.FieldMarkers[NextFieldIndex];
				NextPosition = NextFieldMarker.GlobalPosition;
				ShouldMoveNextTick = true;

				GD.Print("Enemy is moving from " + FieldIndex + " to " + NextFieldIndex);
			}
		}
    }

    public void PlanAttack()
    {
		// Check if enemy is in range (Check if another Summon has nextField or currentField Index at nextFieldIndex)
		foreach (var summon in Game.Summons) {
			if (summon == this) {
				continue;
			}
			if (summon.FieldIndex == NextFieldIndex || summon.NextFieldIndex == NextFieldIndex) {
				summon.DamageTaken += AttackPower;
				ShouldMoveNextTick = false;

				GD.Print("Enemy is attacking " + summon.Type);
				return;
			}
		}
    }

	public void TakeDamage(int Damage) {
		Health -= Damage;
		if (Health <= 0) {
			Die();
		}
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

	public void Die() {
		// Placeholder for cool death animation

		Game.MarkForDeletion(this);
	}

	public void Delete() {
		QueueFree();
	}

}
