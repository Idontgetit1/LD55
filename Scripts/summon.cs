using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public enum SummonType {
	Wolf,
	Slime,
	Spider,
	Whatever,
	Placeholder1,
	Placeholder2
}

public partial class summon : CharacterBody2D
{
	private Game Game;

	// Summon Attributes
	public SummonType Type;
	public bool IsPlayer;
	public int FieldIndex;
	public Marker2D FieldMarker;

	// Stats
	public Stats Stats;

	// Buffer for next Tick
	public bool ShouldMoveNextTick = false;
	public bool ShouldAttackNextTick = false;
	public Vector2 NextPosition;
	public int NextFieldIndex;
	public Marker2D NextFieldMarker;
	public int DamageTaken = 0;

	// Stuff for Attack Animation
	public bool IsAttacking = false;
	public Vector2 ForwardPosition;
	public Vector2 StartPosition;
	[Export] public Sprite2D SummonSprite;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");

		GetNode<Label>("NameTag").Text = Type.ToString();

		// Set Sprite
		var spritePath = "res://Resources/Assets/Summons/";
		// switch (Type) {
		// 	case SummonType.Wolf:
		// 		spritePath += "Wolf.png";
		// 		break;
		// 	case SummonType.Slime:
		// 		spritePath += "Slime.png";
		// 		break;
		// 	case SummonType.Spider:
		// 		spritePath += "Spider.png";
		// 		break;
		// 	case SummonType.Whatever:
		// 		spritePath += "Whatever.png";
		// 		break;
		// 	case SummonType.Placeholder1:
		// 		spritePath += "Placeholder1.png";
		// 		break;
		// 	case SummonType.Placeholder2:
		// 		spritePath += "Placeholder2.png";
		// 		break;
		// }

		// SummonSprite.Texture = GD.Load<Texture2D>(spritePath);

		// Set stats
		Stats = TypeStats.GetStats(Type);

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
			Move(NextPosition);

		}

		if (ShouldAttackNextTick) {
			ShouldAttackNextTick = false;
			AttackAnimation();
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
				ShouldAttackNextTick = true;

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

	public void AttackAnimation() {
		// Move forward fast and back to original position
		IsAttacking = true;
		var Offset = new Vector2(50, 0);
		if (!IsPlayer) {
			Offset *= -1;
		}
		ForwardPosition = SummonSprite.Position + Offset;
		StartPosition = SummonSprite.Position;

		var duration = 0.2f;
		var delay = 0.02f;

		var tween = GetTree().CreateTween();
		tween.TweenProperty(SummonSprite, "position", ForwardPosition, duration)
			.SetEase(Tween.EaseType.Out);

		tween.TweenInterval(delay);

		tween.TweenProperty(SummonSprite, "position", StartPosition, duration)
			.SetEase(Tween.EaseType.In);

		tween.Play();
	}

	public void Move(Vector2 Position) {
		var startPosition = GlobalPosition;
		var endPosition = Position;
		var jumpHeight = 20;
		var duration = 0.15f;

		var midJump = (startPosition + endPosition) / 2;
		midJump.Y -= jumpHeight;

		var tween = GetTree().CreateTween();

		tween.TweenProperty(this, "global_position", midJump, duration)
			.SetEase(Tween.EaseType.Out);

		tween.TweenProperty(this, "global_position", endPosition, duration)
			.SetEase(Tween.EaseType.In);

		tween.Play();
	}

	public Marker2D GetMarker() {
		// Placeholder
		return null;
	}

	public void Die() {

		var duration = 0.1f;

		var tween = GetTree().CreateTween();
		// Smash Summon at left or right side of screen
		var endPosition = IsPlayer ? new Vector2(-100, 0) : new Vector2(100, 0);
		tween.TweenProperty(this, "global_position", endPosition, duration)
			.SetEase(Tween.EaseType.In);

		// Run Delete after animation
		tween.TweenCallback(Callable.From(QueueFree));

		tween.Play();

		Game.MarkForDeletion(this);
	}

	public void Delete() {
		QueueFree();
	}

}
