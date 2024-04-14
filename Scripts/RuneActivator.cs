using Godot;
using System;
using System.Collections.Generic;

public enum RuneType {
	Up,
	Down,
	Left,
	Right,
	None
}

public partial class RuneActivator : Node2D
{
	private const string RunePath = "res://Resources/Assets/Runes/";
	private Dictionary<RuneType, string> RunePaths = new Dictionary<RuneType, string>{
		{ RuneType.Up, "UpRune.png" },
		{ RuneType.Down, "DownRune.png" },
		{ RuneType.Left, "LeftRune.png" },
		{ RuneType.Right, "RightRune2.png" }
	};

	private List<RuneType> runes = new List<RuneType>();
	private List<Sprite2D> runeSprites = new List<Sprite2D>();
	public int lastPressedRune = -1;

	private Callable onActivationCallback;

	private Game Game;

	public SummonType Type;

	private SummonPage Page;

	public void Init(List<RuneType> runeTypes, SummonType type, SummonPage summonPage, Callable onActivationCallback) {
		Type = type;
		Page = summonPage;
		var position = new Vector2(0, 0);
		var spacing = 4;
		foreach (var runeType in runeTypes) {
			var rune = new Sprite2D();
			rune.Scale *= 0.2f;
			rune.Texture = GD.Load<Texture2D>(RunePath + RunePaths[runeType]);
			rune.Position = position;
			runes.Add(runeType);
			runeSprites.Add(rune);
			AddChild(rune);
			position.X += spacing;
		}
		this.onActivationCallback = onActivationCallback;
	}

	public bool onRunePressed(RuneType runeType) {
		if (Game.WasAnotherRuneActivatorActivatedPreviously(this)) {
			lastPressedRune = -1;
			foreach (var rune in runeSprites) {
				makeRuneNormal(runeSprites.IndexOf(rune));
			}


			return true;
		}

		
		if (runes[lastPressedRune + 1] == runeType) {
			lastPressedRune++;
			makeRuneGlow(lastPressedRune);
			if (lastPressedRune == runes.Count - 1) {
				Game.ActivationSuccessful(this);
				lastPressedRune = -1;
				foreach (var rune in runeSprites) {
					makeRuneNormal(runeSprites.IndexOf(rune));
				}
			}
		} else {
			lastPressedRune = -1;
			foreach (var rune in runeSprites) {
				makeRuneNormal(runeSprites.IndexOf(rune));
			}

			if (!Game.IsRuneActivatorInAction()) {
				GD.Print("Failed");
				Game.FailActivation();
				return false;
			}
		}

		return true;
	}

	public void ActivateCalledFromGame() {
		onActivationCallback.Call();
	}

	private void makeRuneGlow(int index) {
		runeSprites[index].Modulate = new Color(0.831f, 0.686f, 0.216f);
		Page.Highlight(true);
	}

	private void makeRuneNormal(int index) {
		runeSprites[index].Modulate = new Color(1, 1, 1);
		Page.Highlight(false);
	}

	public void MakeAllRunesNormal() {
		foreach (var rune in runeSprites) {
			makeRuneNormal(runeSprites.IndexOf(rune));
		}
	}

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
	}

	public override void _Process(double delta)
	{
	}
}
