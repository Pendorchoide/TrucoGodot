using Godot;
using System;
using System.Numerics;

public partial class Card : Node2D
{
	//States
	public bool isBeingDragged = false;
	public bool isOnTable = false;
	public bool hovered = false;

	//Original Godot Properties
	public int originalZIndex;
	public Godot.Vector2 originalPosition;
	public Godot.Vector2 originalGlobalPosition;
	public float originalRotation;
	public Godot.Vector2 originalScale;

	//Game Properties
	[Export] public string suit; // Palo de la carta
	[Export] public int value;   // Valor numerico de la carta

	//Visuals Handler
	public CardVisuals cardVisuals;


	//Tween Variables
	public Tween shrinkTween;
	public Tween rotationTween;
	public Tween scaleTween;
	public Tween moveTween;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("card");
		
		cardVisuals = GetNode<CardVisuals>("CardVisuals");
		cardVisuals.SetCardSprite(this, value, suit);

		originalPosition = Position;
		originalGlobalPosition = GlobalPosition;
		originalRotation = Rotation;
		originalZIndex = ZIndex;
		originalScale = Scale;
	}
	 public void Init(string suit, int value)
    {
        this.suit = suit;
        this.value = value;
    }

	public void ResetOriginalCardProperties()
	{

		originalPosition = GlobalPosition;
		originalGlobalPosition = GlobalPosition;
		originalScale = Scale;
		originalRotation = Rotation;
		originalZIndex = ZIndex;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	
	}

}


