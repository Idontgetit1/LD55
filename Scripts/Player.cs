using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] private bool IsPlayer = true;

	private Sprite2D PlayerSprite;

	public override void _Ready()
	{
		PlayerSprite = GetNode<Sprite2D>("PlayerSprite");
		var BaseScale = 2f;
		if (IsPlayer)
		{
			PlayerSprite.Texture = GD.Load<Texture2D>("res://Resources/Assets/Summoner/Summoner.png");
			PlayerSprite.Scale *= BaseScale;
		}
		else
		{
			PlayerSprite.Texture = GD.Load<Texture2D>("res://Resources/Assets/Summoner/SummonerEnemy.png");
			PlayerSprite.Scale *= BaseScale;
			PlayerSprite.FlipH = false;
		}
	}
}
