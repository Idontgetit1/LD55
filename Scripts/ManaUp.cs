using Godot;
using System;

public partial class ManaUp : Node2D
{
	private int Amount;
	private float Lifetime = 1.5f;
	private float Speed = 1;
	private Label label;

	public void Init(int amount) {
		label = GetNode<Label>("Label");
		Amount = amount;
		var prefix = amount > 0 ? "+" : "";
		label.Text = prefix + amount.ToString();
	}

    public override void _Ready()
    {
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(Position.X, Position.Y - 50), Lifetime)
			.SetEase(Tween.EaseType.InOut);

		tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 0), Lifetime)
			.SetEase(Tween.EaseType.InOut);

		tween.TweenCallback(Callable.From(QueueFree));
		tween.Play();
    }
}
