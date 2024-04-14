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
	[Export] Sprite2D HighlightSprite;
	[Export] Label ManaCostLabel;
	[Export] Label HealthLabel;
	[Export] Label DamageLabel;

	PackedScene ActivatorScene = GD.Load<PackedScene>("res://Scenes/RuneActivator.tscn");

	public override void _Ready()
	{
		
		Game = GetNode<Game>("/root/Game");
		ZIndex = 1;

		var activator = (RuneActivator)ActivatorScene.Instantiate();
		activator.Init(TypeStats.GetStats(Type).Code.runes, Type, this, Callable.From(() => { Game.Main.SummonMonster(Type, true); Game.ActivatedRunes(true); }));
		activator.Scale *= 2.5f;
		RuneActivatorMarker.AddChild(activator);
		Game.RuneActivators.Add(activator);
		ManaCostLabel.Text = TypeStats.GetStats(Type).ManaCost.ToString();
		HealthLabel.Text = TypeStats.GetStats(Type).Health.ToString();
		DamageLabel.Text = TypeStats.GetStats(Type).AtkPower.ToString();
	}

	public void Init(SummonType type)
	{
		Type = type;
		NameLabel.Text = Type.ToString();
		DescriptionLabel.Text = TypeStats.GetStats(Type).Description;
		SummonIconSprite.Texture = GD.Load<Texture2D>(TypeStats.GetStats(Type).TexturePath);
		SummonIconSprite.Scale *= TypeStats.GetStats(Type).BaseScale;

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
			InfoPanel.Visible = false;
		}

		if (Dragging && Input.IsActionJustReleased("click"))
		{
			Dragging = false;
		}

		if (MouseOver) {
			InfoPanel.Position = GetLocalMousePosition();
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

	public void Highlight(bool highlight)
	{
		// HighlightSprite.Visible = highlight;
		if (highlight) {
			Modulate = new Color(0.988f, 1f, 0.6f);
		} else {
			Modulate = new Color(1, 1, 1);
		}
	}
}
