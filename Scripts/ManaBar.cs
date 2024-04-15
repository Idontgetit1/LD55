using Godot;
using System;
using System.Collections.Generic;

public partial class ManaBar : Node2D
{
	[Export] private int MaxMana = 100;
	[Export] private int CurrentMana = 100;
	[Export] private Label ManaLabel;
	[Export] private TextureProgressBar ManaProgressBar;
	[Export] private Marker2D MinigameMarker;
	[Export] private ComboBox ComboLabel;

	private const int MANA_PER_RUNE = 1;
	private const int NO_OF_RUNES_SHOWN = 8;
	private const int MAX_COMBO = 50;
	private const int PENALTY = 5;

	private int combo = 0;

	private List<Sprite2D> runeSprites = new List<Sprite2D>();
	private List<RuneType> runes = new List<RuneType>();

	private Game Game;

	private Texture2D UpRune = GD.Load<Texture2D>("res://Resources/Assets/Runes/UpRune.png");
	private Texture2D DownRune = GD.Load<Texture2D>("res://Resources/Assets/Runes/DownRune.png");
	private Texture2D LeftRune = GD.Load<Texture2D>("res://Resources/Assets/Runes/LeftRune.png");
	private Texture2D RightRune = GD.Load<Texture2D>("res://Resources/Assets/Runes/RightRune2.png");

	private Timer ComboTimer = new Timer();

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
		Game.ManaBar = this;

		ComboTimer.OneShot = false;
		ComboTimer.WaitTime = 2f;
		ComboTimer.Connect("timeout", Callable.From(DecreaseCombo));
		AddChild(ComboTimer);
		ComboTimer.Start();

		// Create Rune Sprites
		var position = MinigameMarker.Position;
		var spacing = 30;
		for (int i = 0; i < NO_OF_RUNES_SHOWN; i++)
		{
			var rune = new Sprite2D();
			rune.Scale *= 1f;
			rune.Position = position;
			runeSprites.Add(rune);
			AddChild(rune);
			position.X += spacing;
		}

		// Create Runes
		for (int i = 0; i < NO_OF_RUNES_SHOWN; i++)
		{
			NewRuneToMinigame();
		}

		// Set Progress Bar
		ManaProgressBar.MaxValue = MaxMana;
		ManaProgressBar.Value = CurrentMana;
	}

	private void SetTimer() {
		float time = 2f - (0.1f * combo);
		ComboTimer.WaitTime = time;
		ComboTimer.Start();

		// Also set pulse speed
		var pulseSpeed = 0.5f + (0.01f * combo);
		ComboLabel.PulseSpeed = pulseSpeed;
	}

    private void DecreaseCombo()
    {
		combo--;
		if (combo < 0)
		{
			combo = 0;
		}
		ComboLabel.SetCombo(combo);
		SetTimer();
    }

    private void NewRuneToMinigame() {
		// Change Sprite Textures, move textures one place further and load a random new rune to the last sprite
		var newRuneType = GetRandomRune();
		
		for (int i = 0; i < runeSprites.Count - 1; i++)
		{
			runeSprites[i].Texture = runeSprites[i + 1].Texture;
		}
		var newRune = runeSprites[runeSprites.Count - 1];
		newRune.Texture = GetRuneTexture(newRuneType);

		// Remove the first Rune from the list and add a new one
		if (runes.Count >= NO_OF_RUNES_SHOWN) {
			runes.RemoveAt(0);
		}

		runes.Add(newRuneType);
	}

	private RuneType GetRandomRune() {
		return (RuneType)GD.RandRange(0, 3);
	}

	private Texture2D GetRuneTexture(RuneType rune) {
		switch (rune) {
			case RuneType.Up:
				return UpRune;
			case RuneType.Down:
				return DownRune;
			case RuneType.Left:
				return LeftRune;
			case RuneType.Right:
				return RightRune;
			default:
				return null;
		}
	}

	public void onRunePressed(RuneType rune)
	{
		if (Game.TutorialActive && Game.Main.TutorialChatIndex != 8) {
			return;
		}
		if (runes[0] == rune)
		{
			// Calculate the mana obtained (for max mana) and then calculate the combo for next input
			AddMana(MANA_PER_RUNE * combo);
			combo++;
			if (combo > MAX_COMBO)
			{
				combo = MAX_COMBO;
			}
			ComboLabel.SetCombo(combo);
			SetTimer();
			NewRuneToMinigame();

			if (Game.TutorialActive && Game.Main.TutorialChatIndex == 8) {
				Game.Main.NextTutorial();
			}
		}
		else
		{
			combo = 0;
			ComboLabel.SetCombo("");
			SetTimer();
			// MinigameFailedAnimation();
			RemoveMana(PENALTY);
		}
	}

	public void AddMana(int amount)
	{
		CurrentMana += amount;
		if (CurrentMana > MaxMana)
		{
			CurrentMana = MaxMana;
		}

		ManaProgressBar.Value = CurrentMana;

		Game.Player.ManaUp(amount);
	}

	public void RemoveMana(int amount)
	{
		CurrentMana -= amount;
		if (CurrentMana < 0)
		{
			CurrentMana = 0;
		}

		ManaProgressBar.Value = CurrentMana;

		Game.Player.ManaUp(-amount);
	}

	public bool HasMana(int amount)
	{
		return CurrentMana >= amount;
	}

	public void SetMana(int amount)
	{
		CurrentMana = amount;
		if (CurrentMana > MaxMana)
		{
			CurrentMana = MaxMana;
		}
		if (CurrentMana < 0)
		{
			CurrentMana = 0;
		}

		ManaProgressBar.Value = CurrentMana;
	}

	public void SetMaxMana(int amount)
	{
		MaxMana = amount;
		if (CurrentMana > MaxMana)
		{
			CurrentMana = MaxMana;
		}
	}

	public override void _Process(double delta)
	{
		ManaLabel.Text = CurrentMana + "/" + MaxMana;
	}
}
