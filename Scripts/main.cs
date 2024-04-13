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
	private AudioStreamPlayer ClickSound;

	private PackedScene SummonScene = GD.Load<PackedScene>("res://Scenes/summon.tscn");

	private Game Game;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");

		// Get 10 Positions between Left and Right Marker
		var left = LeftSummonMarker.GlobalPosition;
		var right = RightSummonMarker.GlobalPosition;
		var diff = right - left;
		var step = diff / Game.Fields;

		for (int i = 0; i <= Game.Fields; i++)
		{
			// Add marker
			var marker = new Marker2D();
			marker.GlobalPosition = left + step * i;

			AddChild(marker);
			Game.FieldMarkers.Add(marker);
		}

		// Summon Monster
		SummonMonster(SummonType.Wolf, true);
		SummonMonster(SummonType.Slime, false);
	}

	private void SummonMonster(SummonType type, bool IsPlayer)
	{
		var summon = (summon)SummonScene.Instantiate();
		summon.Type = type;
		summon.IsPlayer = IsPlayer;
		summon.FieldIndex = IsPlayer ? 0 : Game.Fields - 1;
		summon.FieldMarker = Game.FieldMarkers[summon.FieldIndex];
		summon.GlobalPosition = summon.FieldMarker.GlobalPosition;

		AddChild(summon);
		Game.AddSummon(summon);
	}

	public void _OnTick() {
		GD.Print("Tick");


		// Acting (Execute Buffer from last Tick)
		foreach (var summon in Game.Summons)
		{
			summon.Act();
		}

		// Planning
		// Plan Movement
		foreach (var summon in Game.Summons)
		{
			summon.PlanMove();
		}

		// Plan Attack
		foreach (var summon in Game.Summons)
		{
			summon.PlanAttack();
		}
		
		// Remove Summons
		Game.RemoveTick();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("up")) {
			ClickSound.Play();
		}
		if (Input.IsActionJustPressed("down")) {
			ClickSound.Play();
		}
		if (Input.IsActionJustPressed("left")) {
			ClickSound.Play();
		}
		if (Input.IsActionJustPressed("right")) {
			ClickSound.Play();
		}
	}
}
