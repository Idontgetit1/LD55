using Godot;
using System;

public partial class SettingsMenu : Node2D
{
	private TextureButton EasyButton;
	private TextureButton MediumButton;
	private TextureButton HardButton;
	private Sprite2D DifficultyMarker;

	private Game Game;

	public void ShowMenu() {
		Visible = true;

		// Move self to Vec(0, 0)
		// Tween
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(0, 0), 0.5f)
			.SetEase(Tween.EaseType.InOut);
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

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");

		EasyButton = GetNode<TextureButton>("Control/EasyButton");
		MediumButton = GetNode<TextureButton>("Control/MediumButton");
		HardButton = GetNode<TextureButton>("Control/HardButton");

		DifficultyMarker = GetNode<Sprite2D>("DifficultyMarker");

		Visible = false;
		HideMenu();
	}

	public void MoveMarkerToVector2(Vector2 position) {
		var tween = GetTree().CreateTween();
		tween.TweenProperty(DifficultyMarker, "position", position + new Vector2(0, 35), 0.5f)
			.SetEase(Tween.EaseType.InOut);
	}

	public void OnEasyButtonPressed() {
		MoveMarkerToVector2(new Vector2(DifficultyMarker.Position.X, EasyButton.Position.Y));
		Game.Difficulty = 1;
	}

	public void OnMediumButtonPressed() {
		MoveMarkerToVector2(new Vector2(DifficultyMarker.Position.X, MediumButton.Position.Y));
		Game.Difficulty = 2;
	}

	public void OnHardButtonPressed() {
		MoveMarkerToVector2(new Vector2(DifficultyMarker.Position.X, HardButton.Position.Y));
		Game.Difficulty = 3;
	}

	public void OnBackButtonPressed() {
		HideMenu();
	}
}
