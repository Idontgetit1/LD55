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

	public void Init(List<RuneType> runeTypes, SummonType type, Callable onActivationCallback) {
		Type = type;
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

	public void onRunePressed(RuneType runeType) {
		if (Game.WasAnotherRuneActivatorActivatedPreviously(this)) {
			lastPressedRune = -1;
			foreach (var rune in runeSprites) {
				makeRuneNormal(runeSprites.IndexOf(rune));
			}
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
		}
	}

	public void ActivateCalledFromGame() {
		onActivationCallback.Call();
	}

	private void makeRuneGlow(int index) {
		runeSprites[index].Modulate = new Color(212f/255f, 175f/255f, 55f/255f, 1f);
	}

	private void makeRuneNormal(int index) {
		runeSprites[index].Modulate = new Color(1, 1, 1, 0.5f);
	}

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
	}

	public override void _Process(double delta)
	{
	}
}
