using Godot;
using System;
using System.Collections.Generic;

public partial class RuneSummoningCircle : Node2D
{
	[Export]
	private float RotationSpeed = 0.1f;

	private float Angle = 0;
	private const float Radius = 50;

	private List<Sprite2D> RuneSprites = new List<Sprite2D>();

	private string RunePath = "res://Resources/Assets/Runes/";

	[Export] private AudioStreamPlayer SpellFailSound;
	[Export] private AudioStreamPlayer SpellSuccessSound;
	[Export] private AudioStreamPlayer RuneAddSound;
	

	private Dictionary<RuneType, string> RunePaths = new Dictionary<RuneType, string>{
		{ RuneType.Up, "UpRune.png" },
		{ RuneType.Down, "DownRune.png" },
		{ RuneType.Left, "LeftRune.png" },
		{ RuneType.Right, "RightRune2.png" }
	};

	public override void _Ready()
	{

	}

	public void AddRune(RuneType runeType)
	{
		RuneAddSound.Play();

		// Add Rune Sprite to circle and update all previous runes
		var rune = new Sprite2D();
		rune.Scale *= 1f;
		rune.Texture = GD.Load<Texture2D>(RunePath + RunePaths[runeType]);

		// Add the rune to the list and the scene
		RuneSprites.Add(rune);
		AddChild(rune);

		// Calculate the angle based on the number of existing runes
		float angleIncrement = Mathf.Pi * 2 / RuneSprites.Count;
		float currentAngle = 0;

		// Update the position of all runes
		for (int i = 0; i < RuneSprites.Count; i++)
		{
			var currentRune = RuneSprites[i];
			currentRune.Position = new Vector2(Radius * Mathf.Cos(currentAngle), Radius * Mathf.Sin(currentAngle));
			currentAngle += angleIncrement;
		}
	}

	public void ClearRunes()
	{
		foreach (var rune in RuneSprites)
		{
			rune.QueueFree();
		}
		RuneSprites.Clear();
	}

	public async void SummonAnimation() {
		SpellSuccessSound.Play();

		// Move sprites to the center and then remove them
		var lokalCenterPosition = new Vector2(0, 0);
		var duration = 0.5f;

		foreach (var rune in RuneSprites)
		{
			var tween = GetTree().CreateTween();
			tween.TweenProperty(rune, "position", lokalCenterPosition, duration)
				.SetEase(Tween.EaseType.In);
			tween.Play();
		}

		await ToSignal(GetTree().CreateTimer(duration), "timeout");
		ClearRunes();
	}

	public async void ActivationFailedAnimation() {
		SpellFailSound.Play();
		// Move sprited outwards and make them red and remove them
		var lokalCenterPosition = new Vector2(0, 0);
		var duration = 0.5f;
		var outwardsRadius = 100;

		foreach (var rune in RuneSprites)
		{
			var VectorToCenter = (lokalCenterPosition - rune.Position).Normalized();
			var targetPosition = rune.Position + VectorToCenter * -outwardsRadius;

			var tween = GetTree().CreateTween();
			tween.TweenProperty(rune, "position", targetPosition, duration)
				.SetEase(Tween.EaseType.In);
			tween.Play();

			// Make the rune red
			rune.Modulate = new Color(1, 0, 0, 1);
		}

		// Remove the runes after the animation
		await ToSignal(GetTree().CreateTimer(duration), "timeout");
		ClearRunes();
	}

	public override void _Process(double delta)
	{
		Rotation += RotationSpeed * (float)delta;

		// Negate the local rotation to keep the runes upright
		for (int i = 0; i < RuneSprites.Count; i++)
		{
			RuneSprites[i].Rotation = -Rotation;
		}
	}
}
