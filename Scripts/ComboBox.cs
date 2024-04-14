using Godot;
using System;

public partial class ComboBox : Node2D
{
	private Label label;
	public float PulseSpeed = 1.0f;

    public override void _Ready()
    {
        label = GetNode<Label>("ComboLabel");
        Pulsieren();
    }

	public void SetCombo(int Combo) {
		if (Combo == 0) {
			label.Text = "";
			return;
		}
		label.Text = "x" + Combo.ToString();
	}

	public void SetCombo(string Combo) {
		label.Text = Combo;
	}

    private void Pulsieren()
    {
		var tween = GetTree().CreateTween();
        var startScale = new Vector2(1, 1); // Normale Größe
        var endScale = new Vector2(1.2f, 1.2f); // 20% größer

		tween.TweenProperty(this, "scale", endScale, PulseSpeed);
		tween.TweenProperty(this, "scale", startScale, PulseSpeed);
		tween.TweenCallback(Callable.From(Pulsieren));
		tween.Play();
    }	
}
