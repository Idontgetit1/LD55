using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class main : Node2D
{
	[Export]
	private Marker2D LeftSummonMarker;
	[Export]
	private Marker2D RightSummonMarker;

	[Export]
	private Marker2D[] FieldMarkers = new Marker2D[10];

	[Export]
	public AudioStreamPlayer ClickSound;

	[Export]
	public MagicBook Book;

	private PackedScene SummonScene = GD.Load<PackedScene>("res://Scenes/summon.tscn");
	private PackedScene RuneActivator = GD.Load<PackedScene>("res://Scenes/RuneActivator.tscn");

	private Game Game;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
		Game.Main = this;

		foreach(var field in FieldMarkers) {
			Game.FieldMarkers.Add(field);
		}

		// Summon Monster
		SummonMonster(SummonType.Wolf, true);
		SummonMonster(SummonType.Slime, false);

		// Test
		Book.CreateRuneActivator(new List<RuneType> {
			RuneType.Up,
			RuneType.Down,
			RuneType.Left,
			RuneType.Right
		}, Callable.From(() => GD.Print("Activated No 1")));

		Book.CreateRuneActivator(new List<RuneType> {
			RuneType.Up,
			RuneType.Down,
			RuneType.Left,
			RuneType.Right,
			RuneType.Left,
			RuneType.Right
		}, Callable.From(() => GD.Print("Activated No 2")));
	}

	private void SummonMonster(SummonType type, bool IsPlayer)
	{
		var summon = (summon)SummonScene.Instantiate();
		summon.Type = type;
		summon.IsPlayer = IsPlayer;
		summon.FieldIndex = -1;
		summon.FieldMarker = IsPlayer ? LeftSummonMarker : RightSummonMarker;
		summon.GlobalPosition = summon.FieldMarker.GlobalPosition;

		AddChild(summon);
		Game.AddSummon(summon);

		// Move to Free Field
		var nextFieldIndex = IsPlayer ? Game.GetNextFreeFieldLeft() : Game.GetNextFreeFieldRight();
		if (nextFieldIndex != -1) {
			summon.FieldIndex = nextFieldIndex;
			summon.FieldMarker = FieldMarkers[nextFieldIndex];
			summon.Move(Game.FieldMarkers[nextFieldIndex]);
		}
	}

	public void _OnTick() {
		GD.Print("Tick");
		
		// Remove Summons
		Game.RemoveTick();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
