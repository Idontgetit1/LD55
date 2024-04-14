using Godot;
using System;
using System.Collections.Generic;

public partial class MagicBook : Node2D
{
	private PackedScene RuneActivator = GD.Load<PackedScene>("res://Scenes/RuneActivator.tscn");

	private Game Game;

	[Export]
	private Marker2D RuneStarter;
	private int ActivatorIndex = 0;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");

		// Testing
		// var activator = (RuneActivator)RuneActivator.Instantiate();
		// var runes = new List<RuneType> {
		// 	RuneType.Up,
		// 	RuneType.Down,
		// 	RuneType.Left,
		// 	RuneType.Right
		// };
		// activator.Init(runes, Callable.From(() => GD.Print("Activated")));
		// activator.Position = RuneStarter.Position;
		// AddChild(activator);
		// Game.RuneActivators.Add(activator);
	}

	public void CreateRuneActivator(List<RuneType> runes, Callable onActivationCallback)
	{
		var spacing = 4;
		var activator = (RuneActivator)RuneActivator.Instantiate();
		activator.Init(runes, onActivationCallback);
		activator.Position = RuneStarter.Position + new Vector2(0, ActivatorIndex * spacing);
		AddChild(activator);
		Game.RuneActivators.Add(activator);
		ActivatorIndex++;
	}

	public override void _Process(double delta)
	{
	}
}
