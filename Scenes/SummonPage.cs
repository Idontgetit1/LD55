using Godot;
using System;

public partial class SummonPage : Area2D
{
	SummonType Type;
	private Game Game;

	public Vector2 GrabPoint;

	public bool MouseOver = false;
	public bool Dragging = false;

	[Export] public Node2D InfoPanel;
	[Export] Label NameLabel;
	[Export] Label DescriptionLabel;
	[Export] Sprite2D SummonIconSprite;
	[Export] Marker2D RuneActivatorMarker;
	[Export] Sprite2D HighlightSprite;
	[Export] Label ManaCostLabel;
	[Export] Label HealthLabel;
	[Export] Label DamageLabel;
	[Export] Label NameLabel2;

	PackedScene ActivatorScene = GD.Load<PackedScene>("res://Scenes/RuneActivator.tscn");

	Backboard backboard;

	public override void _Ready()
	{
		
		Game = GetNode<Game>("/root/Game");
		ZIndex = 1;

		backboard = GetParent<Backboard>();

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
		NameLabel.Text = TypeStats.GetStats(Type).Name;
		NameLabel2.Text = TypeStats.GetStats(Type).Name;
		DescriptionLabel.Text = TypeStats.GetStats(Type).Description;
		SummonIconSprite.Texture = GD.Load<Texture2D>(TypeStats.GetStats(Type).TexturePath);
		SummonIconSprite.Scale *= TypeStats.GetStats(Type).BaseScale;

	}

	public override void _Process(double delta)
	{
		// if (MouseOver && Input.IsActionJustPressed("click"))
		// {
		// 	GrabPoint = GlobalPosition - GetGlobalMousePosition();
		// 	Dragging = true;
		// 	backboard.Dragging = true;
		// 	ZIndex = backboard.HoveredPagez++;
		// }

		if (Dragging)
		{
			GlobalPosition = GetGlobalMousePosition() + GrabPoint;
			if (GlobalPosition.Y > Backboard.MaxY) {
				GlobalPosition = new Vector2(GlobalPosition.X, Backboard.MaxY);
			}
			if (GlobalPosition.Y < Backboard.MinY) {
				GlobalPosition = new Vector2(GlobalPosition.X, Backboard.MinY);
			}
			if (GlobalPosition.X > Backboard.MaxX) {
				GlobalPosition = new Vector2(Backboard.MaxX, GlobalPosition.Y);
			}
			if (GlobalPosition.X < Backboard.MinX) {
				GlobalPosition = new Vector2(Backboard.MinX, GlobalPosition.Y);
			}
			InfoPanel.Visible = false;
		}

		if (Dragging && Input.IsActionJustReleased("click"))
		{
			Dragging = false;
			backboard.Dragging = false;
		}

		if (MouseOver)
		{
			InfoPanel.Position = GetLocalMousePosition();
		}
	}

	public void _OnMouseOver()
	{
		MouseOver = true;
		backboard.MouseOver(this);
	}

	public void _OnMouseExit()
	{
		MouseOver = false;
		backboard.MouseOut(this);
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

	public void ShowInfo() {
		if (backboard.Dragging) {
			return;
		}

		InfoPanel.Visible = true;
	}

	public void HideInfo() {
		InfoPanel.Visible = false;
	}
}
