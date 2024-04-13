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

	private List<Marker2D> MovementMarkers = new List<Marker2D>();
	private List<summon> Summons = new List<summon>();

	private PackedScene SummonScene = GD.Load<PackedScene>("res://Scenes/summon.tscn");

	public override void _Ready()
	{
		// Debug Get 10 Positions between Left and Right Marker
		var left = LeftSummonMarker.GlobalPosition;
		var right = RightSummonMarker.GlobalPosition;
		var diff = right - left;
		var step = diff / 8;

		for (int i = 0; i <= 8; i++)
		{
			// Add placeholder texture
			var sprite = new Sprite2D();
			sprite.Texture = new PlaceholderTexture2D();
			sprite.Position = left + step * i;
			AddChild(sprite);

			// Add marker
			var marker = new Marker2D();
			marker.GlobalPosition = left + step * i;
			AddChild(marker);
			MovementMarkers.Add(marker);
		}

		// Summon Monster
		SummonMonster(SummonType.Wolf, true);
		SummonMonster(SummonType.Slime, false);
	}

	private void SummonMonster(SummonType type, bool IsPlayer)
	{
		var SummonMarker = MovementMarkers[0];
		if (!IsPlayer) {
			SummonMarker = MovementMarkers[MovementMarkers.Count - 1];
		}

		var summon = (summon)SummonScene.Instantiate();
		summon.GlobalPosition = SummonMarker.GlobalPosition;
		summon.Type = type;
		summon.IsPlayer = IsPlayer;
		AddChild(summon);
		Summons.Add(summon);
	}

	public void _OnTick() {
		GD.Print("Tick");
		foreach (var summon in Summons) {
			summon.Tick();
			if (summon.ShouldMove()) {
				var index = MovementMarkers.IndexOf(summon.SummonMarker);
				var nextIndex = index + (summon.IsPlayer ? 1 : -1);
				if (nextIndex >= 0 && nextIndex < MovementMarkers.Count) {
					summon.SummonMarker = MovementMarkers[nextIndex];
					summon.Move(summon.SummonMarker.GlobalPosition);
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
