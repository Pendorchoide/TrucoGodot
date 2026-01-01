using Godot;
using System;

public partial class CroupierManager : Node2D
{
	[Export] public Deck deck;
	[Export] public PlayerHand playerHand;
	[Export] public OpponentHand oponentHand;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void ShuffleDeck()
	{
		 var rng = new Random();

		for (int i = deck.cards.Count - 1; i > 0; i--)
		{
			int j = rng.Next(i + 1); // 0 <= j <= i

			// swap
			Card temp = deck.cards[i];
			deck.cards[i] = deck.cards[j];
			deck.cards[j] = temp;
		}
	}

	public void DealCards()
	{
		if (deck.cards.Count < 6)
		{
			GD.Print("Not enough cards in the deck to deal.");
			return;
		}

		for (int i = 0; i < 3; i++) // Reparte 3 cartas a cada jugador
		{
			Card playerCard = deck.cards[0];
			deck.cards.RemoveAt(0);


			playerCard.Visible = true;
			playerHand.AddCard(playerCard, playerHand.playerCardsInHand);



			Card oponentCard = deck.cards[0];
			deck.cards.RemoveAt(0);


			oponentCard.Visible = true;
			oponentHand.AddCard(oponentCard, oponentHand.opponentCardsInHand);

		}
	}

	public void ClearHands()
	{
		playerHand.playerCardsInHand.Clear();
		oponentHand.opponentCardsInHand.Clear();
	}	



}
