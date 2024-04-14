using Godot;
using System;

public partial class Backboard : Sprite2D
{
	PackedScene SummonPageScene = GD.Load<PackedScene>("res://Scenes/SummonPage.tscn");
	public override void _Ready()
	{
		AddAllPages();
	}

	private void AddAllPages() {

		var MaxY = 375;
		var MaxX = 800;
		var MinY = 25;
		var MinX = 25;

		// Add semi randomly
		var random = new Random();
		var types = Enum.GetValues(typeof(SummonType));
		foreach (SummonType type in types)
		{
			var summonPage = (SummonPage)SummonPageScene.Instantiate();
			summonPage.Init(type);
			summonPage.Position = new Vector2(random.Next(MinX, MaxX), random.Next(MinY, MaxY));
			AddChild(summonPage);
		}
	}
}
