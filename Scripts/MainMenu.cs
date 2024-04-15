using Godot;
using System;

public partial class MainMenu : Control
{
	private SettingsMenu settingsMenu;
	private CreditsMenu creditsMenu;
	private Game Game;
	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
		settingsMenu = GetNode<SettingsMenu>("SettingsMenu");
		creditsMenu = GetNode<CreditsMenu>("CreditsMenu");
	}

	public void OnStartButtonPressed()
	{
		Game.StartGame();

		// Hide the main menu
		Visible = false;
	}

	public void OnSettingsButtonPressed()
	{
		settingsMenu.ShowMenu();
	}

	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

	public void OnCreditsButtonPressed()
	{
		creditsMenu.ShowMenu();
	}
}
