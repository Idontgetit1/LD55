using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
	// Fields
	public const int Fields = 10;
	public List<Marker2D> FieldMarkers = new List<Marker2D>();
	
	// Summons
	public List<summon> Summons = new List<summon>();
	public List <summon> SummonsToRemove = new List<summon>();

	// Game
	public int Tick = 0;


	// Rune Activation Stuff
	public List<RuneActivator> RuneActivators = new List<RuneActivator>();

	public List<RuneType> CurrentRuneSequence = new List<RuneType>();


	public main Main;

	public const int MIDDLE_MARKER_INDEX_LEFT = 4;
	public const int MIDDLE_MARKER_INDEX_RIGHT = 5;

	public bool SummoningSuccessful = true;


	// Mana
	public ManaBar ManaBar;

	// Methods to get the closest Free Field to the Middle Point
	public int GetNextFreeFieldLeft()
	{
		// Count from middle outwards
		for (int i = MIDDLE_MARKER_INDEX_LEFT; i >= 0; i--) {
			if (GetSummonAtField(i) == null) {
				return i;
			}
		}

		return -1;
	}

	public int GetNextFreeFieldRight()
	{
		// Count from middle outwards
		for (int i = MIDDLE_MARKER_INDEX_RIGHT; i < Fields; i++) {
			if (GetSummonAtField(i) == null) {
				return i;
			}
		}

		return -1;
	}

	public summon GetSummonAtField(int fieldIndex)
	{
		foreach (var summon in Summons) {
			if (summon.FieldIndex == fieldIndex) {
				return summon;
			}
		}
		return null;
	}

	public void ActivatedRunes(bool IsPlayer) {
		var runeSummoningCircle = IsPlayer ? Main.RuneSummoningCircleLeft : Main.RuneSummoningCircleRight;

		runeSummoningCircle.SummonAnimation();
	}
	
	public void AddSummon(summon Summon)
	{
		Summons.Add(Summon);
	}

	public void SummonDied(summon Summon)
	{
		SummonsToRemove.Add(Summon);
	}

	public void RemoveTick()
	{
		foreach (var summon in SummonsToRemove) {
			Summons.Remove(summon);
		}

		SummonsToRemove.Clear();
	}

	public bool WasAnotherRuneActivatorActivatedPreviously(RuneActivator currentRuneActivator) {
		foreach (var activator in RuneActivators) {
			if (activator == currentRuneActivator) {
				continue;
			}
			if (activator.lastPressedRune >= 1) {
				return true;
			}
		}
		return false;
	}

	public bool IsRuneActivatorInAction() {
		foreach (var activator in RuneActivators) {
			if (activator.lastPressedRune != -1) {
				GD.Print("Activator in Action");
				return true;
			}
		}
		return false;
	}

	public void FailActivation() {
		var circle = Main.RuneSummoningCircleLeft;
		circle.ActivationFailedAnimation();

		foreach(var activator in RuneActivators) {
			activator.lastPressedRune = -1;
			activator.MakeAllRunesNormal();
		}
	}

	public bool ActivationSuccessful(RuneActivator activator) {
		SummoningSuccessful = true;

		// test if Mana is sufficient
		int manaCost = TypeStats.GetStats(activator.Type).ManaCost;
		if (ManaBar.HasMana(manaCost)) {
			ManaBar.RemoveMana(manaCost);
			// ManaBar.ActivateManaAnimation();

			// Summon Monster
			activator.ActivateCalledFromGame();

			GD.Print("Using " + manaCost + " Mana to summon " + activator.Type);
			return true;
		} else {
			// ManaBar.NotEnoughManaAnimation();

			SummoningSuccessful = false;
			GD.Print("Not enough Mana");
			return false;
		}
	}

	private int CompareRuneSequencesBeginsWith(List<RuneType> input, List<RuneType> fullSequence) {
		if (input.Count > fullSequence.Count) {
			return -1;
		}

		bool isEqual = false;
		if (input.Count == fullSequence.Count) {
			isEqual = true;
		}

		for (int i = 0; i < input.Count; i++) {
			if (input[i] != fullSequence[i]) {
				return -1;
			}
		}

		return isEqual ? 0 : 1;
	}

    public override void _Process(double delta)
    {
		RuneType runePressed = RuneType.None;

		if (Input.IsActionPressed("SwitchMeditate")) {
			if (Input.IsActionJustPressed("up")) {
				runePressed = RuneType.Up;
				Main.ClickSound.Play();
			}
			if (Input.IsActionJustPressed("down")) {
				runePressed = RuneType.Down;
				Main.ClickSound.Play();
			}
			if (Input.IsActionJustPressed("left")) {
				runePressed = RuneType.Left;
				Main.ClickSound.Play();
			}
			if (Input.IsActionJustPressed("right")) {
				runePressed = RuneType.Right;
				Main.ClickSound.Play();
			}

			if (runePressed != RuneType.None) {
				ManaBar.onRunePressed(runePressed);
			}
			return;
		}

		if (Input.IsActionJustPressed("up")) {
			Main.ClickSound.PitchScale = 10.5f;
			Main.ClickSound.Play();
			Main.ClickSound.PitchScale = 1.0f;
			runePressed = RuneType.Up;
		}
		if (Input.IsActionJustPressed("down")) {
			Main.ClickSound.PitchScale = 0.5f;
			Main.ClickSound.Play();
			Main.ClickSound.PitchScale = 1.0f;
			runePressed = RuneType.Down;
		}
		if (Input.IsActionJustPressed("left")) {
			Main.ClickSound.PitchScale = 10.25f;
			Main.ClickSound.Play();
			Main.ClickSound.PitchScale = 1.0f;
			runePressed = RuneType.Left;
		}
		if (Input.IsActionJustPressed("right")) {
			Main.ClickSound.PitchScale = 0.75f;
			Main.ClickSound.Play();
			Main.ClickSound.PitchScale = 1.0f;
			runePressed = RuneType.Right;
		}

		if (runePressed != RuneType.None) {

			CurrentRuneSequence.Add(runePressed);

			Main.RuneSummoningCircleLeft.AddRune(runePressed);
			bool sequenceFound = false;
			foreach (var activator in RuneActivators) {
				// var success = activator.onRunePressed(runePressed);
				var compare = CompareRuneSequencesBeginsWith(CurrentRuneSequence, activator.runes);
				if (compare == 0) {
					var success = ActivationSuccessful(activator);
					if (success) {
						sequenceFound = true;
						CurrentRuneSequence.Clear();
						break;
					}
					compare = -1;
				}
				if (compare == -1) {
					// Rune sequence does not match
					activator.MakeAllRunesNormal();
				}
				if (compare == 1) {
					// Rune sequence is a prefix of the full sequence
					activator.MarkFirstNRunesAsPressed(CurrentRuneSequence.Count);
					sequenceFound = true;
				}
			}
			// If no activator is active, summoning failed
			GD.Print("Test");
			if (!sequenceFound) {
				Main.RuneSummoningCircleLeft.ActivationFailedAnimation();
				CurrentRuneSequence.Clear();
			}

			SummoningSuccessful = true;
		}

    }

	private Dictionary<Node, bool> _shakingNodes = new Dictionary<Node, bool>();

	public async void ShakeNode(Node2D node, float duration, float intensity) {
		if (_shakingNodes.ContainsKey(node)) {
			return;
		}
		_shakingNodes[node] = true;
		var originalPosition = node.GlobalPosition;
		var timer = new Timer();
		AddChild(timer);
		timer.WaitTime = duration;
		timer.OneShot = true;
		timer.Start();
		while (!timer.IsStopped()) {
			node.Position = originalPosition + new Vector2((float)GD.RandRange(-intensity, intensity), (float)GD.RandRange(-intensity, intensity));
			await ToSignal(GetTree().CreateTimer(0.05f), "timeout"); // Warte kurz zwischen den Vibrationen
		}
		node.GlobalPosition = originalPosition;
		GD.Print("Shake done");
		_shakingNodes.Remove(node);
	}

}
