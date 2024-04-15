using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[Export] private bool IsPlayer = true;

	private Sprite2D PlayerSprite;

	private PackedScene ManaUpScene = GD.Load<PackedScene>("res://Scenes/ManaUp.tscn");
	private PackedScene HPUpScene = GD.Load<PackedScene>("res://Scenes/HPUp.tscn");

	Game Game;

	public int Health = 100;

	// Enemy Stuff
	public int ActionsPerMinute = 120;
	public float PauseBetweenActivations = 1f;
	public int Mana = 100;
	public const int MAX_MANA = 100;
	private int MaxCombo = 20;


	// Tutorial

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
		if (IsPlayer)
		{
			Game.Player = this;
		} else {
			Game.Enemy = this;
		}
		PlayerSprite = GetNode<Sprite2D>("PlayerSprite");
		var BaseScale = 1f;
		if (IsPlayer)
		{
			PlayerSprite.Texture = GD.Load<Texture2D>("res://Resources/Assets/Summoner/Summoner1.png");
			PlayerSprite.Scale *= BaseScale;
		}
		else
		{
			PlayerSprite.Texture = GD.Load<Texture2D>("res://Resources/Assets/Summoner/SummonerEnemy1.png");
			PlayerSprite.Scale *= BaseScale;
			PlayerSprite.FlipH = false;
		}



		// enemy stuff
		if (!IsPlayer) {
			if (Game.TutorialActive) {
				return;
			}

			if (Game.Difficulty == 1) {
				ActionsPerMinute = 60;
			} else if (Game.Difficulty == 2) {
				ActionsPerMinute = 120;
			} else if (Game.Difficulty == 3) {
				ActionsPerMinute = 240;
			}

			var EnemyTimer = GetNode<Timer>("Timer");
			EnemyTimer.WaitTime = 60f / ActionsPerMinute;
			EnemyTimer.Connect("timeout", Callable.From(EnemyAction));
			EnemyTimer.Start();

			MaxCombo = 20*(ActionsPerMinute/60);
		}
	}

	// Enemy Stuff
	SummonType? NextSummon = null;
	List<RuneType> RuneSequence = new List<RuneType>();
	private int ManaCombo = 0;
	public void EnemyAction() {
		if (NextSummon == null) {
			// Choose random summon
			var random = new Random();
			var types = Enum.GetValues(typeof(SummonType));
			NextSummon = (SummonType)types.GetValue(random.Next(types.Length));
			RuneSequence = TypeStats.GetStats(NextSummon.Value).Code.runes;
		} else {
			if (Mana >= TypeStats.GetStats(NextSummon.Value).ManaCost) {
				ManaCombo = 0;
				var RuneCircle = Game.Main.RuneSummoningCircleRight;
				RuneCircle.AddRune(RuneSequence[0]);
				RuneSequence.RemoveAt(0);
				if (RuneSequence.Count == 0) {
					// Summon Monster
					Game.Main.SummonMonster(NextSummon.Value, false);
					Mana -= TypeStats.GetStats(NextSummon.Value).ManaCost;
					RuneCircle.SummonAnimation();
					NextSummon = null;
				}
			} else {
				Mana += ManaCombo;
				ManaUp(ManaCombo);
				ManaCombo++;
				if (ManaCombo > MaxCombo) {
					ManaCombo = 0;
				}
			}
		}
	}

	public async void TutorialSummonEntity(SummonType summonType) {
		var runes = TypeStats.GetStats(summonType).Code.runes;
		var RuneCircle = Game.Main.RuneSummoningCircleRight;
		foreach (var rune in runes) {
			RuneCircle.AddRune(rune);
			await ToSignal(GetTree().CreateTimer(1f), "timeout");
		}
		Game.Main.SummonMonster(summonType, false);
		RuneCircle.SummonAnimation();

		// Game.Main.NextTutorial();
		Game.Main.TutorialAllowFight = true;
	}

	public void ManaUp(int Amount) {
		var manaUp = (ManaUp)ManaUpScene.Instantiate();
		manaUp.Init(Amount);
		manaUp.Position = IsPlayer ? new Vector2(5, -20) : new Vector2(-5, -20);
		AddChild(manaUp);
	}

    public void TakeDamage(int atkPower)
    {
		Health -= atkPower;
		HealthUp(-atkPower);

		if (IsPlayer) {
			Game.Main.PlayerHealthLabel.Text = Health.ToString();
		} else {
			Game.Main.EnemyHealthLabel.Text = Health.ToString();
		}

		Game.Main.HitSound.Play();
		Game.ShakeNode(this, 0.5f, 10f);
		if (Health <= 0)
		{
			Health = 0;
			if (IsPlayer) {
				Game.GameOver();
			} else {
				Game.Win();
			}
		}
    }

	public void HealthUp(int Amount) {
		var hpUp = (ManaUp)HPUpScene.Instantiate();
		hpUp.Init(Amount);
		hpUp.Position = IsPlayer ? new Vector2(5, -20) : new Vector2(-5, -20);
		AddChild(hpUp);
	}
}
