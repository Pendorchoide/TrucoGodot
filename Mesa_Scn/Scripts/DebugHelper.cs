using Godot;
using System;

public partial class DebugHelper : Node
{

	[Export] private CroupierManager CroupierManager;
	[Export] private Deck Deck;
	[Export] private PlayerHand PlayerHand;
	[Export] private OpponentHand OpponentHand;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed)
		{

			switch (eventKey.Keycode)
			{
				case Key.I: 		//Initialize deck
					Deck.InitializeDeck();
					GD.Print("deck initialized");
					break;
				case Key.D: 			//deal cards
					CroupierManager.DealCards();
					GD.Print("cards dealt");
					break;
				case Key.S: 			//shuffle deck
					CroupierManager.ShuffleDeck();
					GD.Print("deck shuffled");
					break;

				case Key.H: 			//print player hand
					PlayerHand.PrintHand(PlayerHand.playerCardsInHand);
					break;
				case Key.P:				//position player hand
					PlayerHand.PositionCards();
					GD.Print("Player's cards positioned.");
					break;

				// Add more key cases as needed
				default:
					break;
			}
		}
	}
}
