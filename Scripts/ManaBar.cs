using Godot;
using System;

public partial class ManaBar : Node2D
{
	[Export] private int MaxMana = 100;
	[Export] private int CurrentMana = 100;
	[Export] private Label ManaLabel;

	private Game Game;

	public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
		Game.ManaBar = this;
	}

	public bool onRunePressed(RuneType rune)
	{
		
	}

	public void AddMana(int amount)
	{
		CurrentMana += amount;
		if (CurrentMana > MaxMana)
		{
			CurrentMana = MaxMana;
		}
	}

	public void RemoveMana(int amount)
	{
		CurrentMana -= amount;
		if (CurrentMana < 0)
		{
			CurrentMana = 0;
		}
	}

	public bool HasMana(int amount)
	{
		return CurrentMana >= amount;
	}

	public void SetMana(int amount)
	{
		CurrentMana = amount;
		if (CurrentMana > MaxMana)
		{
			CurrentMana = MaxMana;
		}
		if (CurrentMana < 0)
		{
			CurrentMana = 0;
		}
	}

	public void SetMaxMana(int amount)
	{
		MaxMana = amount;
		if (CurrentMana > MaxMana)
		{
			CurrentMana = MaxMana;
		}
	}

	public override void _Process(double delta)
	{
		ManaLabel.Text = CurrentMana + "/" + MaxMana;
	}
}
