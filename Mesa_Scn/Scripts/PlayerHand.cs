using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHand : Hand
{
	public List<Card> playerCardsInHand = new(); // Lista para almacenar las cartas en la mano del jugador
	[Export] public Node2D Card1PlaceHolder; //player card 1 placeholder
	[Export] public Node2D Card2PlaceHolder; //player card 2 placeholder
	[Export] public Node2D Card3PlaceHolder; //player card 3 placeholder

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

		public void PositionCards()
	{
		switch (playerCardsInHand.Count)
		{
			case 1:
				playerCardsInHand[0].GlobalTransform = Card1PlaceHolder.GlobalTransform;
				playerCardsInHand[0].ResetOriginalCardProperties();
				playerCardsInHand[0].Visible = true;
				break;
			case 2:
				playerCardsInHand[0].GlobalTransform = Card1PlaceHolder.GlobalTransform;
				playerCardsInHand[0].ResetOriginalCardProperties();
				playerCardsInHand[0].Visible = true;
				playerCardsInHand[1].GlobalTransform = Card2PlaceHolder.GlobalTransform;
				playerCardsInHand[1].ResetOriginalCardProperties();
				playerCardsInHand[1].Visible = true;	
				break;
			case 3:
				playerCardsInHand[0].GlobalTransform = Card1PlaceHolder.GlobalTransform;
				playerCardsInHand[0].ResetOriginalCardProperties();
				playerCardsInHand[0].Visible = true;
				playerCardsInHand[1].GlobalTransform = Card2PlaceHolder.GlobalTransform;
				playerCardsInHand[1].ResetOriginalCardProperties();
				playerCardsInHand[1].Visible = true;
				playerCardsInHand[2].GlobalTransform = Card3PlaceHolder.GlobalTransform;
				playerCardsInHand[2].ResetOriginalCardProperties();
				playerCardsInHand[2].Visible = true;
				break;
			default:
				GD.Print("No cards in hand to position.");
				break;		
		}

	}


}
