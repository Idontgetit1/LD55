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


	public main Main;

	public const int MIDDLE_MARKER_INDEX_LEFT = 4;
	public const int MIDDLE_MARKER_INDEX_RIGHT = 5;

	public bool SummoningSuccessful = false;


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
			if (activator.lastPressedRune <= 0) {
				return false;
			}
		}
		return true;
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

	public void ActivationSuccessful(RuneActivator activator) {
		SummoningSuccessful = true;

		// test if Mana is sufficient
	}

    public override void _Process(double delta)
    {
		RuneType runePressed = RuneType.None;
		if (Input.IsActionJustPressed("up")) {
			Main.ClickSound.Play();
			runePressed = RuneType.Up;
		}
		if (Input.IsActionJustPressed("down")) {
			Main.ClickSound.Play();
			runePressed = RuneType.Down;
		}
		if (Input.IsActionJustPressed("left")) {
			Main.ClickSound.Play();
			runePressed = RuneType.Left;
		}
		if (Input.IsActionJustPressed("right")) {
			Main.ClickSound.Play();
			runePressed = RuneType.Right;
		}

		if (runePressed != RuneType.None) {
			Main.RuneSummoningCircleLeft.AddRune(runePressed);
			foreach (var activator in RuneActivators) {
				activator.onRunePressed(runePressed);
			}
			// If no activator is active, summoning failed
			if (!IsRuneActivatorInAction() && !SummoningSuccessful) {
				Main.RuneSummoningCircleLeft.ActivationFailedAnimation();
			}

			SummoningSuccessful = false;
		}

    }
}
