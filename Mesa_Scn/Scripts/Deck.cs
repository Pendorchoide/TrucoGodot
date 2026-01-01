using Godot;
using System;
using System.Collections.Generic;

public partial class Deck : Node2D
{

	[Export] public PackedScene cardScene;
	public List<Card> cards = new(); // Array para almacenar las 40 cartas del mazo
	



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}


	public void InitializeDeck()
	{
		int[] values = { 1,2,3,4,5,6,7,10,11,12 }; // Valores de las cartas en el truco
		string[] suits = { "Oro", "Copa", "Espada", "Basto" };


		foreach (var suit in suits)
		{
			foreach (var value in values)
			{
				Card card = cardScene.Instantiate<Card>();
				card.Init(suit, value);
				card.Visible = true;

				cards.Add(card);
				AddChild(card);
			}
		}
	}



}
