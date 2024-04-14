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
	public RuneSummoningCircle RuneSummoningCircleLeft;
	[Export]
	public RuneSummoningCircle RuneSummoningCircleRight;

	[Export]
	private Marker2D[] FieldMarkers = new Marker2D[10];

	[Export]
	public AudioStreamPlayer ClickSound;
	[Export] public AudioStreamPlayer HitSound;
	[Export] public AudioStreamPlayer DeathSound;

	// [Export]
	// public MagicBook Book;

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
		// SummonMonster(SummonType.FlameWolf, true);
		SummonMonster(SummonType.Slime, false);

		SummonMonster(SummonType.Tree, false);
		SummonMonster(SummonType.IceMouse, false);
		SummonMonster(SummonType.Slime, false);
		SummonMonster(SummonType.Slime, false);

		// Set Rune Activators
		// SetSummonActivatorRunes();
	}

	private void SetSummonActivatorRunes() {
		// Summon Wolf
		// Book.CreateRuneActivator(new List<RuneType> {
		// 	RuneType.Up,
		// 	RuneType.Down,
		// 	RuneType.Left,
		// 	RuneType.Right
		// }, "Wolf", SummonType.FlameWolf, Callable.From(() => {SummonMonster(SummonType.FlameWolf, true); Game.ActivatedRunes(true);}));

		// // Summon Slime
		// Book.CreateRuneActivator(new List<RuneType> {
		// 	RuneType.Down,
		// 	RuneType.Right,
		// 	RuneType.Right,
		// 	RuneType.Up,
		// 	RuneType.Down
		// }, "Slime", SummonType.Slime, Callable.From(() => {SummonMonster(SummonType.Slime, true); Game.ActivatedRunes(true);}));
	}

	public void SummonMonster(SummonType type, bool IsPlayer)
	{
		var canSpawn = IsPlayer ? Game.GetNextFreeFieldLeft() != -1 : Game.GetNextFreeFieldRight() != -1;
		if (!canSpawn) {
			GD.Print("No free field to summon");
			return;
		}

		var summon = (summon)SummonScene.Instantiate();
		summon.Type = type;
		summon.IsPlayer = IsPlayer;
		summon.FieldIndex = IsPlayer ? -1 : -2;
		summon.FieldMarker = IsPlayer ? LeftSummonMarker : RightSummonMarker;
		summon.GlobalPosition = summon.FieldMarker.GlobalPosition;

		AddChild(summon);
		Game.AddSummon(summon);

		// Move to Free Field
		var nextFieldIndex = IsPlayer ? Game.GetNextFreeFieldLeft() : Game.GetNextFreeFieldRight();
		if (nextFieldIndex != -1) {
			summon.FieldIndex = nextFieldIndex;
			summon.FieldMarker = FieldMarkers[nextFieldIndex];
			summon.Move(nextFieldIndex);
		}
	}

	public void _OnTick() {
		GD.Print("Tick");

		// Remove Summons
		Game.RemoveTick();

		foreach (var summon in Game.Summons) {
			summon._OnTick();
		}
		
		// Move Summons
		MoveSummonsToMiddle();

	}

	public void _OnAtk() {
		// If two monsters in middle
		var monsterLeft = Game.GetSummonAtField(Game.MIDDLE_MARKER_INDEX_LEFT);
		var monsterRight = Game.GetSummonAtField(Game.MIDDLE_MARKER_INDEX_RIGHT);

		if (monsterLeft != null && monsterRight != null) {
			monsterLeft.Attack(monsterRight);
			monsterRight.Attack(monsterLeft);
		}
	}

	public void MoveSummonsToMiddle() {
		// Move Summons to next Free Field in Middle
		// First Left (Count from middle outwards)
		for (int i = Game.MIDDLE_MARKER_INDEX_LEFT; i >= 0; i--) {
			var summon = Game.GetSummonAtField(i);
			if (summon != null) {
				var index = Game.GetNextFreeFieldLeft();
				// Check if new index is larger than current index
				if (index > i) {
					summon.Move(index);
				}
			}
		}

		// Then Right
		for (int i = Game.MIDDLE_MARKER_INDEX_RIGHT; i < Game.Fields; i++) {
			var summon = Game.GetSummonAtField(i);
			if (summon != null) {
				var index = Game.GetNextFreeFieldRight();
				// Check if new index is smaller than current index
				if (index < i) {
					summon.Move(index);
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
