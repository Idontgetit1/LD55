using Godot;
using System;

public partial class PauseMenu : Node2D
{
	private Game Game;
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		Game = GetNode<Game>("/root/Game");
		Visible = false;
	}

	public void OnExitButtonPressed() {
		GetTree().Paused = false;
		GetTree().Quit();
	}

	public void OnMainMenuButtonPressed() {
		GetTree().Paused = false;
		Game.MainMenu();
	}
}
