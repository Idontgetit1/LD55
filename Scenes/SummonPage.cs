using Godot;
using System;

public partial class SummonPage : Area2D
{
	SummonType Type;
	private Game Game;

	private Vector2 GrabPoint;

	private bool MouseOver = false;
	private bool Dragging = false;

	[Export] Node2D InfoPanel;
	[Export] Label NameLabel;
	[Export] Label DescriptionLabel;
	[Export] Sprite2D SummonIconSprite;
	[Export] Marker2D RuneActivatorMarker;

	public override void _Ready()
	{
		
		Game = GetNode<Game>("/root/Game");
		ZIndex = 1;
	}

	public void Init(SummonType type)
	{
		Type = type;
		NameLabel.Text = Type.ToString();
		DescriptionLabel.Text = TypeStats.GetStats(Type).Description;
		SummonIconSprite.Texture = GD.Load<Texture2D>(TypeStats.GetStats(Type).TexturePath);
	}

	public override void _Process(double delta)
	{
		if (MouseOver && Input.IsActionJustPressed("click"))
		{
			GrabPoint = GetGlobalMousePosition() - Position;
			Dragging = true;
		}

		if (Dragging)
		{
			Position = GetGlobalMousePosition() - GrabPoint;
		}

		if (Dragging && Input.IsActionJustReleased("click"))
		{
			Dragging = false;
		}
	}

	public void _OnMouseOver()
	{
		InfoPanel.Visible = true;
		MouseOver = true;
		ZIndex = 2;
	}

	public void _OnMouseExit()
	{
		InfoPanel.Visible = false;
		MouseOver = false;
		ZIndex = 1;
	}

	public void _OnInputEvent(Node viewport, InputEvent @event, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			GetViewport().SetInputAsHandled();
		}
	}
}
