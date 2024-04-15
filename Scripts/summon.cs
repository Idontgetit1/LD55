using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public enum SummonType {
	FlameWolf,
	Slime,
	Slimeloon,
	Tree,
	GrassBoi,
	IceMouse,
	MagmaPuddle,
	Cherry,
	Bat
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

	[Export] public Sprite2D SummonSprite;

	private PackedScene HPUpScene = GD.Load<PackedScene>("res://Scenes/HPUp.tscn");
	private PackedScene AtkUpScene = GD.Load<PackedScene>("res://Scenes/AtkUp.tscn");

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");

		GetNode<Label>("NameTag").Text = Type.ToString();

		// Set stats
		Stats = TypeStats.GetStats(Type);

		Scale *= Stats.BaseScale;

		// Set Texture if present
		if (Stats.TexturePath != null) {
			SummonSprite.Texture = GD.Load<Texture2D>(Stats.TexturePath);
		}

		if (IsPlayer) {
			SummonSprite.FlipH = true && Stats.Mirrored;
		}
	}

	// Burn Effect
	public int BurnDamage = 0;
	public int BurnDuration = 0;
	private int BurnCounter = 0;
	public int BurnEvery = 0;
	public bool Burning = false;

	// Freeze Effect
	public int FrozenFor = 0;

	// Heal Effect
	public int HealCounter = 0;

	// Strength Effect
	public bool Strengthened = false;

	public void _OnTick() {
		if (Burning) {
			BurnCounter++;
			if (BurnCounter % BurnEvery == 0) {
				TakeDamage(BurnDamage);
			}
			if (BurnCounter >= BurnDuration && BurnDuration != -1) {
				Burning = false;
				BurnCounter = 0;
				GetNode<Node2D>("FireAnimation").Visible = false;
				// GetNode<CpuParticles2D>("FireParticles").Emitting = false;
			}
		}

		if (FieldIndex != Game.MIDDLE_MARKER_INDEX_LEFT && FieldIndex != Game.MIDDLE_MARKER_INDEX_RIGHT) {
			var nextIndex = IsPlayer ? FieldIndex + 1 : FieldIndex - 1;
			if (nextIndex < 0 || nextIndex >= Game.Fields) {
				return;
			}
			var nextSummon = Game.GetSummonAtField(nextIndex);

			if (nextSummon == null) {
				return;
			}

			if (nextSummon.Type == SummonType.Cherry) {
				AtkAnimation(Callable.From(() => {nextSummon.TakeDamage(99); GainAtk(5); GainHealth(5);}));
			}

			if (Type == SummonType.Tree) {
				HealCounter++;
				if (HealCounter % 3 == 0) {
					nextSummon.GainHealth(1);
				}
			}

			if (Type == SummonType.GrassBoi) {
				if (!nextSummon.Strengthened) {
					nextSummon.Strengthened = true;
					nextSummon.GainAtk(5);
				}
			}

			if (Type == SummonType.Bat) {
				if (!nextSummon.Strengthened) {
					nextSummon.Strengthened = true;
					nextSummon.GainAtk(1);
				}
			}
		}

		if (FrozenFor > 0) {
			FrozenFor--;
			if (FrozenFor == 0) {
				GetNode<Sprite2D>("IceBlock").Visible = false;
			}
		}

	}

	private void GainAtk(int amount) {
		Stats.AtkPower += amount;

		var atkUp = (ManaUp)AtkUpScene.Instantiate();
		atkUp.Init(amount, 2f * Stats.BaseScale);
		atkUp.Position = new Vector2(-15, -20);

		AddChild(atkUp);
	}

	private void GainHealth(int amount) {
		Stats.Health += amount;

		var hpUp = (ManaUp)HPUpScene.Instantiate();
		hpUp.Init(amount, 2f * Stats.BaseScale);
		hpUp.Position = new Vector2(15, -20);

		AddChild(hpUp);
	}

	private void AtkAnimation(Callable callback) {
		// Move forward fast and back to original position
		var Offset = new Vector2(50, 0);
		var BackOffset = new Vector2(-10, 0); // Kleine Bewegung nach hinten
		if (!IsPlayer) {
			Offset *= -1;
			BackOffset *= -1;
		}
		var BackPosition = SummonSprite.Position + BackOffset;
		var ForwardPosition = SummonSprite.Position + Offset;
		var StartPosition = SummonSprite.Position;

		var duration = 0.2f;
		var backDuration = 0.05f; // Kurze Dauer für den Rückzug
		var delay = 0.02f;

		var tween = GetTree().CreateTween();
		
		// Kurz nach hinten bewegen
		tween.TweenProperty(SummonSprite, "position", BackPosition, backDuration)
			.SetEase(Tween.EaseType.Out);

		tween.TweenInterval(delay);

		// Dann schnell nach vorne
		tween.TweenProperty(SummonSprite, "position", ForwardPosition, duration)
			.SetEase(Tween.EaseType.Out);

		tween.TweenCallback(callback);

		tween.TweenInterval(delay);
		// await ToSignal(GetTree().CreateTimer(delay), "timeout");

		// Und zurück zur Startposition
		tween.TweenProperty(SummonSprite, "position", StartPosition, duration)
			.SetEase(Tween.EaseType.In);

		tween.Play();
	}

	// Slowed Effect
	public bool Slowed = false;
	private int SlowedCounter = 0;

	public void Attack(summon target) {
		if (FrozenFor > 0) {
			return;
		}

		if (Slowed) {
			SlowedCounter++;
			if (SlowedCounter % 2 != 0) {
				return;
			}
		}

		// Move forward fast and back to original position
		var Offset = new Vector2(50, 0);
		var BackOffset = new Vector2(-10, 0); // Kleine Bewegung nach hinten
		if (!IsPlayer) {
			Offset *= -1;
			BackOffset *= -1;
		}
		var BackPosition = SummonSprite.Position + BackOffset;
		var ForwardPosition = SummonSprite.Position + Offset;
		var StartPosition = SummonSprite.Position;

		var duration = 0.2f;
		var backDuration = 0.05f; // Kurze Dauer für den Rückzug
		var delay = 0.02f;

		var tween = GetTree().CreateTween();
		
		// Kurz nach hinten bewegen
		tween.TweenProperty(SummonSprite, "position", BackPosition, backDuration)
			.SetEase(Tween.EaseType.Out);

		tween.TweenInterval(delay);

		// Dann schnell nach vorne
		tween.TweenProperty(SummonSprite, "position", ForwardPosition, duration)
			.SetEase(Tween.EaseType.Out);

		tween.TweenCallback(Callable.From(() => {
			Game.ShakeNode(Game.Main, 0.1f, 10);
			target.TakeDamage(Stats.AtkPower);
			if (Type == SummonType.FlameWolf) {
				target.BurnDamage = 1;
				target.BurnDuration = -1;
				target.BurnEvery = 3;
				target.Burning = true;
				// target.GetNode<CpuParticles2D>("FireParticles").Emitting = true;
				target.GetNode<Node2D>("FireAnimation").Visible = true;
			}
			if (Type == SummonType.MagmaPuddle) {
				// Get Next 3 Enemies
				var enemy1 = Game.GetSummonAtField(IsPlayer ? FieldIndex + 1 : FieldIndex - 1);
				var enemy2 = Game.GetSummonAtField(IsPlayer ? FieldIndex + 2 : FieldIndex - 2);
				var enemy3 = Game.GetSummonAtField(IsPlayer ? FieldIndex + 3 : FieldIndex - 3);

				if (enemy1 != null) {
					target.BurnDamage = 1;
					target.BurnDuration = -1;
					target.BurnEvery = 3;
					target.Burning = true;
					// target.GetNode<CpuParticles2D>("FireParticles").Emitting = true;
					enemy1.GetNode<Node2D>("FireAnimation").Visible = true;
				}

				if (enemy2 != null) {
					enemy2.BurnDamage = 1;
					enemy2.BurnDuration = -1;
					enemy2.BurnEvery = 3;
					enemy2.Burning = true;
					// target.GetNode<CpuParticles2D>("FireParticles").Emitting = true;
					enemy2.GetNode<Node2D>("FireAnimation").Visible = true;
				}

				if (enemy3 != null) {
					enemy3.BurnDamage = 1;
					enemy3.BurnDuration = -1;
					enemy3.BurnEvery = 3;
					enemy3.Burning = true;
					// target.GetNode<CpuParticles2D>("FireParticles").Emitting = true;
					enemy3.GetNode<Node2D>("FireAnimation").Visible = true;
				}
			}
			if (Type == SummonType.IceMouse) {
				// 5% chance
				if (new Random().Next(0, 100) < 5) {
					target.FrozenFor = 3;
					target.GetNode<Sprite2D>("IceBlock").Visible = true;
				}
			}
			if (Type == SummonType.Bat) {
				GainHealth(1);
			}

			if (target.Type == SummonType.MagmaPuddle) {
				// Hit self
				TakeDamage(1);
			}
		}));

		tween.TweenInterval(delay);
		// await ToSignal(GetTree().CreateTimer(delay), "timeout");

		// Und zurück zur Startposition
		tween.TweenProperty(SummonSprite, "position", StartPosition, duration)
			.SetEase(Tween.EaseType.In);

		tween.Play();
	}


	public void Move(int index) {

		if (index == -1) {
			GD.Print("[" + this.Name + "] Moving to -1");
			return;
		}

		GD.Print("[" + this.Name + "] Moving from " + FieldIndex + " to " + index);
		FieldMarker = Game.FieldMarkers[index];
		FieldIndex = index;
		var Position = FieldMarker.GlobalPosition;
		var startPosition = GlobalPosition;
		var endPosition = Position;
		var bounceHeight = 10; // Höhe des Bounces
		var jumpHeight = 20;
		var duration = 0.15f;

		var bounceBack = startPosition - new Vector2(0, bounceHeight);
		var midJump = (startPosition + endPosition) / 2;
		midJump.Y -= jumpHeight;

		var tween = GetTree().CreateTween();

		// Erst nach hinten bewegen
		tween.TweenProperty(this, "global_position", bounceBack, duration * 0.5)
			.SetEase(Tween.EaseType.Out);

		// Dann zum höchsten Punkt des Sprungs
		tween.TweenProperty(this, "global_position", midJump, duration)
			.SetEase(Tween.EaseType.Out);

		// Und schließlich zur Endposition
		tween.TweenProperty(this, "global_position", endPosition, duration)
			.SetEase(Tween.EaseType.In);

		tween.Play();
	}

	public void TakeDamage(int damage) {
		if (Type == SummonType.Slimeloon) {
			// 15% chance to dodge
			if (new Random().Next(0, 100) < 15) {
				return;
			}
		}

		Game.Main.HitSound.Play();
		Stats.Health -= damage;
		if (Stats.Health <= 0) {
			Die();
		} else {
			if (FieldIndex != Game.MIDDLE_MARKER_INDEX_LEFT && FieldIndex != Game.MIDDLE_MARKER_INDEX_RIGHT)
				Game.ShakeNode(this, 0.1f, 10);
		}
	}

	public void Die() {
		
		if (Type == SummonType.Slime) {
			var nextEnemy = Game.GetSummonAtField(IsPlayer ? FieldIndex + 1 : FieldIndex - 1);
			if (nextEnemy != null) {
				nextEnemy.Slowed = true;
			}
		}

		var duration = 0.1f;

		var tween = GetTree().CreateTween();
		// Smash Summon at left or right side of screen
		var endPosition = IsPlayer ? new Vector2(0, 0) : new Vector2(GetWindow().Size.X, 0);
		tween.TweenProperty(this, "global_position", endPosition, duration)
			.SetEase(Tween.EaseType.In);

		// Run Delete after animation
		tween.TweenCallback(Callable.From(() => {Game.Main.DeathSound.Play(); QueueFree();}));

		tween.Play();

		Game.SummonDied(this);
	}

	public void Delete() {
		QueueFree();
	}

}
