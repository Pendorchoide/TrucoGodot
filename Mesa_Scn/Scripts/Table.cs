using Godot;
using System;

public partial class Table : Node2D
{
	public Card[] playableSlotInTable = new Card[3]; // Array para almacenar las cartas jugadas en la mesa
	Area2D dropZone; // Zona donde se pueden soltar las cartas

	public Vector2 Slot1Position;
	public Vector2 Slot2Position;
	public Vector2 Slot3Position;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("table");
		dropZone = GetNode<Area2D>("Area2D");

		Slot1Position = GetNode<Node2D>("PlayableSlot").GlobalPosition; //slot1
		Slot2Position = GetNode<Node2D>("PlayableSlot2").GlobalPosition; //slot2
		Slot3Position = GetNode<Node2D>("PlayableSlot3").GlobalPosition; //slot3

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
