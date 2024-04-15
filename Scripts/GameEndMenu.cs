using Godot;
using System;

public partial class GameEndMenu : Node2D
{
	public void ShowMenu() {
		Visible = true;

		// Move self to Vec(0, 0)
		// Tween
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(0, 0), 0.5f)
			.SetEase(Tween.EaseType.InOut);
		
		tween.TweenCallback(Callable.From(() => {
			GetTree().Paused = true;
		}));
		tween.Play();
	}

	public void HideMenu() {
		// Move self to Vec(0, -1000)
		// Tween
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(0, -1000), 0.5f)
			.SetEase(Tween.EaseType.InOut);
		
		tween.TweenCallback(Callable.From(() => Visible = false));
		tween.Play();
	}

	[Export] public Label StatusLabel;
	[Export] public Label EnemiesKilledLabel;

	private Game Game;

	public void SetStatus(string status) {
		StatusLabel.Text = status;
	}

	public void SetEnemiesKilled(int enemiesKilled) {
		EnemiesKilledLabel.Text = enemiesKilled.ToString();
	}

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		Game = GetNode<Game>("/root/Game");
		Visible = false;
		HideMenu();
	}

	public void OnMainMenuButtonPressed() {
		Game.MainMenu();
	}
}
