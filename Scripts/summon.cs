using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public enum SummonType {
	FlameWolf,
	Slime,
	Slimeloon,
	Tree,
	GrassBoi,
	IceMouse,
	MagmaPuddle
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

	public override void _Process(double delta)
	{
	}

	public async void Attack(summon target) {
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
			Game.Main.HitSound.Play();
		}));

		tween.TweenInterval(delay);
		// await ToSignal(GetTree().CreateTimer(delay), "timeout");

		// Und zurück zur Startposition
		tween.TweenProperty(SummonSprite, "position", StartPosition, duration)
			.SetEase(Tween.EaseType.In);

		tween.Play();

	}


	public void Move(int index) {

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
		Stats.Health -= damage;
		if (Stats.Health <= 0) {
			Die();
		} else {
			// Game.ShakeNode(SummonSprite, 0.1f, 5);
		}
	}

	public void Die() {

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
