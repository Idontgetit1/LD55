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
	public Dictionary<int, summon> SummonsByFieldIndex = new Dictionary<int, summon>();

	// Game
	public int Tick = 0;


	// Rune Activation Stuff
	public List<RuneActivator> RuneActivators = new List<RuneActivator>();


	public main Main;

	private const int MiddleMarkerIndexLeft = 4;
	private const int MiddleMarkerIndexRight = 5;

	// Methods to get the closest Free Field to the Middle Point
	public int GetNextFreeFieldLeft()
	{
		for (int i = MiddleMarkerIndexLeft; i >= 0; i--) {
			if (!SummonsByFieldIndex.ContainsKey(i)) {
				return i;
			}
		}
		return -1;
	}

	public int GetNextFreeFieldRight()
	{
		for (int i = MiddleMarkerIndexRight; i < Fields; i++) {
			if (!SummonsByFieldIndex.ContainsKey(i)) {
				return i;
			}
		}
		return -1;
	}
	
	public void AddSummon(summon Summon)
	{
		Summons.Add(Summon);
		SummonsByFieldIndex.Add(Summon.FieldIndex, Summon);
	}

	public void MarkForDeletion(summon Summon)
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
			foreach (var activator in RuneActivators) {
				activator.onRunePressed(runePressed);
			}
		}
    }
}
