using Godot;
using System;

public partial class CreditsMenu : Node2D
{
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
		Visible = false;
		HideMenu();
	}

	public void OnBackButtonPressed() {
		HideMenu();
	}
}
