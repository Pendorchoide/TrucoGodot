using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Hand : Node2D
{
	

	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	

	public void AddCard(Card card, List<Card> cardsInHand)
	{
		card.Reparent(this);
		cardsInHand.Add(card);
		card.Visible = true;
	}



		//debug 

	public void PrintHand(List<Card> cardsInHand)	
	{
		GD.Print("Player's Hand:");
		foreach (var card in cardsInHand)
		{
			GD.Print($"- {card.value} of {card.suit}");
		}
	}	
}
	







