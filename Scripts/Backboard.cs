using Godot;
using System;

public partial class Backboard : Sprite2D
{
	PackedScene SummonPageScene = GD.Load<PackedScene>("res://Scenes/SummonPage.tscn");

	[Export] private float BaseScale = 3.2f;
	public override void _Ready()
	{
		AddAllPages();
	}

	private void AddAllPages() {

		int MaxY = (int)(GetRect().Size.Y - 25);
		int MaxX = (int)(GetRect().Size.X - 25);
		var MinY = 25;
		var MinX = 25;

		// Add semi randomly
		var random = new Random();
		var types = Enum.GetValues(typeof(SummonType));
		foreach (SummonType type in types)
		{
			var summonPage = (SummonPage)SummonPageScene.Instantiate();
			summonPage.Init(type);
			summonPage.Scale /= BaseScale;
			summonPage.Position = new Vector2(random.Next(MinX, MaxX), random.Next(MinY, MaxY));
			AddChild(summonPage);
		}
	}
}
