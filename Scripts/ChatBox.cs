using Godot;
using System;

public partial class ChatBox : Node2D
{
	[Export] public string Text = "Hello, World!";
	[Export] new public string Name = "Player";
	[Export] public float LetterSpeed = 0.1f;

	Label label;


	public async void StartShowingText() {
		GD.Print("Start showing Text.");
		foreach (char c in Text) {
			label.Text += c;
			await ToSignal(GetTree().CreateTimer(LetterSpeed), "timeout");
		}
	}

	public new void Show() {
		GD.Print("Showing ChatBox.");
		label.Text = "";
		Visible = true;
		StartShowingText();
	}

	public new void Hide() {
		Visible = false;
		label.Text = "";
	}

	public override void _Ready()
	{
		label = GetNode<Label>("SpeechBubble/Label");
		Visible = false;
	}
}
