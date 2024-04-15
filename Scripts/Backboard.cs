using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Backboard : Sprite2D
{
	PackedScene SummonPageScene = GD.Load<PackedScene>("res://Scenes/SummonPage.tscn");

	[Export] private float BaseScale = 3.2f;
	private List<SummonPage> summonPages = new List<SummonPage>();
	private LinkedList<SummonPage> hoveredPages = new LinkedList<SummonPage>();

	public bool Dragging = false;

	public int HoveredPagez = 0;
	public override void _Ready()
	{
		AddAllPages();
	}

	public void MouseOver(SummonPage page) {
		hoveredPages.AddLast(page);
		var hoveredPage = hoveredPages.OrderByDescending(x => x.ZIndex).FirstOrDefault();
		if (hoveredPage != null) {
			hoveredPage.ShowInfo();
		}
	}

	public void MouseOut(SummonPage page) {
		page.InfoPanel.Hide();
		hoveredPages.Remove(page);
		var hoveredPage = hoveredPages.OrderByDescending(x => x.ZIndex).FirstOrDefault();
		if (hoveredPage != null) {
			hoveredPage.ShowInfo();
		}
	}

	public const int MaxY = 240;
	public const int MaxX = 800;
	public const int MinY = 25;
	public const int MinX = 25;

	private void AddAllPages() {


		// Add semi randomly
		var random = new Random();
		var types = Enum.GetValues(typeof(SummonType));
		int i = 1;
		foreach (SummonType type in types)
		{
			var summonPage = (SummonPage)SummonPageScene.Instantiate();
			summonPage.Init(type);
			summonPage.Scale /= BaseScale;
			summonPage.Position = new Vector2(random.Next(MinX, MaxX) / BaseScale, random.Next(MinY, MaxY) / BaseScale);
			AddChild(summonPage);
			summonPages.Add(summonPage);

			// set Z of all pages to i
			summonPage.ZIndex = i;
			i++;
		}
	}

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left) {
			// Get Hovered Page with biggest Z Index
			var hoveredPage = hoveredPages.OrderByDescending(x => x.ZIndex).FirstOrDefault();
			if (mouseButton.Pressed) {
				if (hoveredPage != null) {
					Dragging = true;
					hoveredPage.Dragging = true;
					hoveredPage.GrabPoint = hoveredPage.GlobalPosition - mouseButton.Position;

					// Make ZIndex of hoveredPage to highest and decrease others by one (Only those that are higher than hoveredPage)
					foreach (SummonPage page in summonPages)
					{
						if (page.ZIndex > hoveredPage.ZIndex) {
							page.ZIndex -= 1;
						}
					}
					hoveredPage.ZIndex = summonPages.Count;
				}
			} else {
				if (hoveredPage != null) {
					Dragging = false;
					hoveredPage.ShowInfo();
				}
				foreach (SummonPage page in summonPages)
				{
					page.Dragging = false;
				}
			}
		}
    }
    public void SetOtherPagesNormal() {
		// set Z of all pages to 1
		foreach (Node2D child in GetChildren())
		{
			child.ZIndex = 1;
		}
	}
}
