using Godot;
using System;

public partial class ComboBox : Node2D
{
	private Label label;
	public float PulseSpeed = 1.0f;

	[Export] public string StartText = "";

    public override void _Ready()
    {
        label = GetNode<Label>("ComboLabel");
		label.Text = StartText;
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
        var startScale = Scale; // Normale Größe
        var endScale = Scale * 1.2f; // 20% größer

		tween.TweenProperty(this, "scale", endScale, PulseSpeed);
		tween.TweenProperty(this, "scale", startScale, PulseSpeed);
		tween.TweenCallback(Callable.From(Pulsieren));
		tween.Play();
    }	
}
