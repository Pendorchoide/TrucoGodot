using Godot;
using System;

public partial class Card : Node2D
{
	public int originalZIndex;
	public Vector2 originalPosition;
	public float originalRotation;

	[Export] public string suit; // Palo de la carta
	[Export] public int value;   // Valor numerico de la carta

	//Return Effect variables
	[Export] public bool returningToOrigin = false;

	//Shaking effect variables
	[Export] public bool shaking = false;
	[Export] public float shakeStrength = .1f;
    [Export] public float shakeDuration = .3f;
    [Export] public float shakeFadeSpeed = 5f;

	private float shakeTime = 0f;


	/*public Card(int value, string suit)
	{
		this.value = value;
		this.suit = suit;
	}
*/

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		originalPosition = Position;
		originalRotation = Rotation;
		originalZIndex = ZIndex;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (shaking)
			ShakeEffect(delta);

		if(returningToOrigin)
		   ReturnToOriginalPosition();
	}

	private void ShakeEffect(double delta)
	{
			  // Reinicia el tiempo de sacudida cada vez que se llama
			
            
			 
       		if (shakeTime > 0)	 // Mientras el tiempo no acabe
        	{
				shakeTime -= (float) delta;

				float intensity = shakeStrength * (shakeTime / shakeDuration); // Va decayendo suavemente
				float randomRot = (float)Mathf.Sign(GD.RandRange(-1, 1)) * intensity;
			
            	Rotation = originalRotation + randomRot;
			}
			else{
				Rotation = Mathf.Lerp(Rotation, originalRotation, (float)delta * shakeFadeSpeed); //hace un fade out cuando se acaba el tiempo
				if (Mathf.Abs(Rotation - originalRotation) < 0.01f)  // Si la rotacion esta cerca de la original apaga el shake	
				{
				
					Rotation = originalRotation;
					shaking = false;
				}
			
			}
			
			
	}

	public void StartShaking()
	{
		shakeTime = shakeDuration;
		shaking = true;
	}

	private void ReturnToOriginalPosition()
	{
		float returnVel = 0.05f; // Ajusta este valor para cambiar la velocidad de retorno

		Position = new Vector2 (Mathf.Lerp(Position.X, originalPosition.X, returnVel),
							    Mathf.Lerp(Position.Y, originalPosition.Y, returnVel));
		
		if (Position.DistanceTo(originalPosition) < 1f) // Si esta cerca de la posicion original
		{
			Position = originalPosition;
			returningToOrigin = false;
			//StartShaking();
		}
	}

	public void ReturnToOrigin()
	{
		returningToOrigin = true;
	}
}
