using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private bool IsPlayer = true;

	private Sprite2D PlayerSprite;

	private PackedScene ManaUpScene = GD.Load<PackedScene>("res://Scenes/ManaUp.tscn");

	Game Game;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
		if (IsPlayer)
		{
			Game.Player = this;
		} else {
			Game.Enemy = this;
		}
		PlayerSprite = GetNode<Sprite2D>("PlayerSprite");
		var BaseScale = 1f;
		if (IsPlayer)
		{
			PlayerSprite.Texture = GD.Load<Texture2D>("res://Resources/Assets/Summoner/Summoner1.png");
			PlayerSprite.Scale *= BaseScale;
		}
		else
		{
			PlayerSprite.Texture = GD.Load<Texture2D>("res://Resources/Assets/Summoner/SummonerEnemy1.png");
			PlayerSprite.Scale *= BaseScale;
			PlayerSprite.FlipH = false;
		}
	}

	public void ManaUp(int Amount) {
		var manaUp = (ManaUp)ManaUpScene.Instantiate();
		manaUp.Init(Amount);
		manaUp.Position = IsPlayer ? new Vector2(5, -20) : new Vector2(-5, -20);
		AddChild(manaUp);
	}
}
