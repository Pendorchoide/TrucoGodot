using Godot;
using System;

public partial class Mazo : Node2D
{

	Card[] cartas = new Card[40]; // Array para almacenar las 40 cartas del mazo




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeDeck();
	}

	private void InitializeDeck()
	{
		for (int i = 0; i < 40; i++)
		{
			//cartas[i] = new Card(i % 10 + 1, GetSuit(i)); // Asigna valor y palo a cada carta
		}
	}

	private string GetSuit(int index)
	{
		if (index < 10)
			return "Oro";
		else if (index < 20)
			return "Copa";
		else if (index < 30)
			return "Espada";
		else
			return "Basto";
	}

}
