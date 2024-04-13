using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
	// Fields
	public const int Fields = 9;
	public List<Marker2D> FieldMarkers = new List<Marker2D>();
	
	// Summons
	public List<summon> Summons = new List<summon>();

	// Game
	public int Tick = 0;
	
	public void AddSummon(summon Summon)
	{
		Summons.Add(Summon);
	}
}
