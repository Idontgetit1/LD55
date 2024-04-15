using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class main : Node2D
{
	[Export]
	private Marker2D LeftSummonMarker;
	[Export]
	private Marker2D RightSummonMarker;

	[Export] public Label PlayerHealthLabel;
	[Export] public Label EnemyHealthLabel;

	[Export]
	public RuneSummoningCircle RuneSummoningCircleLeft;
	[Export]
	public RuneSummoningCircle RuneSummoningCircleRight;

	[Export]
	private Marker2D[] FieldMarkers = new Marker2D[10];

	[Export]
	public AudioStreamPlayer ClickSound;
	[Export] public AudioStreamPlayer HitSound;
	[Export] public AudioStreamPlayer DeathSound;

	[Export] public Timer TickTimer;
	[Export] public Timer AtkTimer;

	[Export] public Node2D PauseMenu;
	[Export] public GameEndMenu GameEndMenu;

	// [Export]
	// public MagicBook Book;

	private PackedScene SummonScene = GD.Load<PackedScene>("res://Scenes/summon.tscn");
	private PackedScene RuneActivator = GD.Load<PackedScene>("res://Scenes/RuneActivator.tscn");

	private Game Game;


	// Tutorial

	[ExportCategory("Tutorial")]
	[Export] private ChatBox[] TutorialChats = new ChatBox[12];
	private bool SkipByClick = true;
	public bool TutorialAllowFight = false;
	private List<int> SkippableByClick = new List<int> {0, 1, 2, 3, 4, 7, 9, 10};

	public int TutorialChatIndex = -1;
	public void NextTutorial() {
		if (TutorialChatIndex >= 0) {
			TutorialChats[TutorialChatIndex].Hide();
		}
		TutorialChatIndex++;

		if (TutorialChatIndex == 1) {
			Game.Backboard.AddPage(SummonType.Slime);
		} else if (TutorialChatIndex == 6) {
			Game.Enemy.TutorialSummonEntity(SummonType.Slime);
		} else if (TutorialChatIndex == 10) {
			Game.Backboard.AddAllPages();
		}

		if (SkippableByClick.Contains(TutorialChatIndex)) {
			SkipByClick = true;
		} else {
			SkipByClick = false;
		}

		GD.Print("Tutorial No. " + TutorialChatIndex);
		GD.Print("Tutorial Text: " + TutorialChats[TutorialChatIndex].Text);
		if (TutorialChatIndex < TutorialChats.Length) {
			TutorialChats[TutorialChatIndex].Show();
		} else {
			Game.TutorialActive = false;
			Game.StartGame();
		}
	}

    public override void _Input(InputEvent @event)
    {
		// Left click
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
		{
			ClickSound.Play();
			if (TutorialChatIndex >= 0 && TutorialChatIndex < TutorialChats.Length) {
				if (SkipByClick) {
					NextTutorial();
				}
			}
		}
    }

    public override void _Ready()
	{
		Game = GetNode<Game>("/root/Game");
		Game.Main = this;

		// Start tutorial ?
		if (Game.TutorialActive) {
			NextTutorial();
		}


		foreach(var field in FieldMarkers) {
			Game.FieldMarkers.Add(field);
		}
	}

	private void SetSummonActivatorRunes() {
		// Summon Wolf
		// Book.CreateRuneActivator(new List<RuneType> {
		// 	RuneType.Up,
		// 	RuneType.Down,
		// 	RuneType.Left,
		// 	RuneType.Right
		// }, "Wolf", SummonType.FlameWolf, Callable.From(() => {SummonMonster(SummonType.FlameWolf, true); Game.ActivatedRunes(true);}));

		// // Summon Slime
		// Book.CreateRuneActivator(new List<RuneType> {
		// 	RuneType.Down,
		// 	RuneType.Right,
		// 	RuneType.Right,
		// 	RuneType.Up,
		// 	RuneType.Down
		// }, "Slime", SummonType.Slime, Callable.From(() => {SummonMonster(SummonType.Slime, true); Game.ActivatedRunes(true);}));
	}

	public void SummonMonster(SummonType type, bool IsPlayer)
	{
		var canSpawn = IsPlayer ? Game.GetNextFreeFieldLeft() != -1 : Game.GetNextFreeFieldRight() != -1;
		if (!canSpawn) {
			GD.Print("No free field to summon");
			return;
		}

		var summon = (summon)SummonScene.Instantiate();
		summon.Type = type;
		summon.IsPlayer = IsPlayer;
		summon.FieldIndex = IsPlayer ? -1 : -2;
		summon.FieldMarker = IsPlayer ? LeftSummonMarker : RightSummonMarker;
		summon.GlobalPosition = summon.FieldMarker.GlobalPosition;

		AddChild(summon);
		Game.AddSummon(summon);

		// Move to Free Field
		var nextFieldIndex = IsPlayer ? Game.GetNextFreeFieldLeft() : Game.GetNextFreeFieldRight();
		if (nextFieldIndex != -1) {
			summon.FieldIndex = nextFieldIndex;
			summon.FieldMarker = FieldMarkers[nextFieldIndex];
			summon.Move(nextFieldIndex);
		}
	}

	public void _OnTick() {

		if (Game.TutorialActive && TutorialAllowFight && TutorialChatIndex == 6) {
			// Check if monsters on field
			var monsterLeft = Game.GetSummonAtField(Game.MIDDLE_MARKER_INDEX_LEFT);
			var monsterRight = Game.GetSummonAtField(Game.MIDDLE_MARKER_INDEX_RIGHT);

			if (monsterLeft == null && monsterRight == null) {
				NextTutorial();
			}
		}

		GD.Print("Tick");

		// Remove Summons
		Game.RemoveTick();

		foreach (var summon in Game.Summons) {
			summon._OnTick();
		}
		
		// Move Summons
		MoveSummonsToMiddle();

	}

	public void _OnAtk() {

		if (Game.TutorialActive && !TutorialAllowFight) {
			return;
		}

		// If two monsters in middle
		var monsterLeft = Game.GetSummonAtField(Game.MIDDLE_MARKER_INDEX_LEFT);
		var monsterRight = Game.GetSummonAtField(Game.MIDDLE_MARKER_INDEX_RIGHT);

		if (monsterLeft != null && monsterRight != null) {
			monsterLeft.Attack(monsterRight);
			monsterRight.Attack(monsterLeft);
		}

		// If one monster in middle Attack Players life points directly
		if (monsterLeft != null && monsterRight == null) {
			monsterLeft.AsYouHaveNoMonstersOnYourFieldIWillAttackYourLifePointsDirectly();
		}

		if (monsterLeft == null && monsterRight != null) {
			monsterRight.AsYouHaveNoMonstersOnYourFieldIWillAttackYourLifePointsDirectly();
		}
	}

	public void MoveSummonsToMiddle() {
		// Move Summons to next Free Field in Middle
		// First Left (Count from middle outwards)
		for (int i = Game.MIDDLE_MARKER_INDEX_LEFT; i >= 0; i--) {
			var summon = Game.GetSummonAtField(i);
			if (summon != null) {
				var index = Game.GetNextFreeFieldLeft();
				// Check if new index is larger than current index
				if (index > i) {
					summon.Move(index);
				}
			}
		}

		// Then Right
		for (int i = Game.MIDDLE_MARKER_INDEX_RIGHT; i < Game.Fields; i++) {
			var summon = Game.GetSummonAtField(i);
			if (summon != null) {
				var index = Game.GetNextFreeFieldRight();
				// Check if new index is smaller than current index
				if (index < i) {
					summon.Move(index);
				}
			}
		}
	}

	public void onMusicButtonToggled(bool pressed)
	{
		if (!pressed) {
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), false);
		} else {
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), true);
		}
	}

	public void onSfxButtonToggled(bool pressed)
	{
		if (!pressed) {
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Sfx"), false);
		} else {
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Sfx"), true);
		}
	}
}
