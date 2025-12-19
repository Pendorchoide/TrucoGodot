using Godot;
using System;

public partial class Card : Node2D
{
	[Export] public bool isBeingDragged = false;

	public int originalZIndex;
	public Vector2 originalPosition;
	public Vector2 originalGlobalPosition;
	public float originalRotation;

	public float effectOriginalRotation;

	public Vector2 originalScale;

	public Vector2 effectOriginalScale;

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

	//Shrink effect variables
	[Export] public bool shrinking = false;
	[Export] public bool resettingScale = false;
	
	//Pop effect variables
	[Export] public bool popping = false;
					bool popGrowing = true;

	//Rotation variables
	[Export] public bool rotatingToZero = false;
	[Export] public bool returningToOriginalRotation = false;

	//Go to position effect variables
	[Export] public bool goingToPosition = false;
	public Vector2 targetPosition;

	/*public Card(int value, string suit)
	{
		this.value = value;
		this.suit = suit;
	}
*/

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("card");

		CallDeferred(nameof(SetCardSprite)); // Llama a la funcion despues de que el nodo este listo

		originalPosition = Position;
		originalGlobalPosition = GlobalPosition;
		originalRotation = Rotation;
		originalZIndex = ZIndex;
		originalScale = Scale;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (shaking)
			ShakeEffect(delta);

		if(returningToOrigin)
		   ReturnToOriginalPosition(delta);
		if(shrinking)
		   ShrinkEffect(delta);
		if(resettingScale)
		   ResetScale(delta);
		if(rotatingToZero)
		   RotationToZero(delta);
		if(returningToOriginalRotation)
		   ReturnToOriginalRotation(delta);
		if(popping)
		   PopEffect(delta);
		if(goingToPosition)
		   GoToPosition(delta);
	}

	private void SetCardSprite()
{
	GD.Print($"Carta: {value} de {suit}");
    Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");

    sprite.RegionEnabled = true;
    sprite.RegionRect = new Rect2(
        (value - 1) * 62,
        GetSuitOffset(suit) * 95,
        62,
        95
    );
}
	private int GetSuitOffset(string suit)
	{
		suit = suit.ToLower().Trim();

		return suit switch
		{
			"espada" => 0,
			"basto" => 1,
			"oro" => 2,
			"copa" => 3,
			_ => 4,
		};
	}

	private void ShakeEffect(double delta)
	{
			  // Reinicia el tiempo de sacudida cada vez que se llama
			
			
			
	   		if (shakeTime > 0)	 // Mientras el tiempo no acabe
			{
				shakeTime -= (float) delta;

				float intensity = shakeStrength * (shakeTime / shakeDuration); // Va decayendo suavemente
				float randomRot = (float)Mathf.Sign(GD.RandRange(-1, 1)) * intensity;
			
				Rotation = effectOriginalRotation + randomRot;
			}
			else{
				Rotation = Mathf.Lerp(Rotation, effectOriginalRotation, (float)delta * shakeFadeSpeed); //hace un fade out cuando se acaba el tiempo
				if (Mathf.Abs(Rotation - effectOriginalRotation) < 0.01f)  // Si la rotacion esta cerca de la original apaga el shake	
				{
				
					Rotation = effectOriginalRotation;
					shaking = false;
				}
			
			}
			
			
	}

	public void StartShaking()
	{
		effectOriginalRotation = Rotation;
		shakeTime = shakeDuration;
		shaking = true;
	}

	private void ReturnToOriginalPosition(double delta)
	{
		float returnVel = 5f; // Ajusta este valor para cambiar la velocidad de retorno

		GlobalPosition = new Vector2 (Mathf.Lerp(GlobalPosition.X, originalGlobalPosition.X, returnVel * (float)delta),
								Mathf.Lerp(GlobalPosition.Y, originalGlobalPosition.Y, returnVel * (float)delta));
		
		if (GlobalPosition.DistanceTo(originalGlobalPosition) < 0.1f) // Si esta cerca de la posicion original
		{
			GlobalPosition = originalGlobalPosition;
			returningToOrigin = false;
			//StartShaking();
		}
	}

	public void StartReturnToOrigin()
	{
		returningToOrigin = true;
	}

	private void GoToPosition(double delta)
	{
		float lerpVel = 10f;
		Position = new Vector2 (Mathf.Lerp(Position.X, targetPosition.X, (float)delta * lerpVel),
								Mathf.Lerp(Position.Y, targetPosition.Y, (float)delta * lerpVel));
		
		if (Position.DistanceTo(targetPosition) < 0.1f) // Si esta cerca de la posicion objetivo
		{
			Position = targetPosition;
			goingToPosition = false;
		}
	}

		public void StartGoToPosition(Vector2 targetPosition)
	{
		this.targetPosition = targetPosition;
		goingToPosition = true;
	}

	private void ShrinkEffect(double delta)
	{	//float shrinkValue = 2f/3f;  //pasa la carta con 1.5 de escala a 1 de escala
		float lerpVel = 10f;
		const float DESIRED_SCALE = 1; // Escala deseada al encoger
		Vector2 newScale = new Vector2(DESIRED_SCALE,DESIRED_SCALE);
		
		Scale = Scale.Lerp(newScale, (float)delta * lerpVel);

		if (Scale.DistanceTo(newScale) < 0.1f) // Si esta cerca de la escala objetivo
		{
			Scale = newScale;
			shrinking = false;
		}
	}

	public void StartShrink()
	{
		StopAllScaleEffects();
		
		shrinking = true;
	}



	private void ResetScale(double delta)
	{
		float returnVel = 20f; // Ajusta este valor para cambiar la velocidad de retorno

		Scale = Scale.Lerp(originalScale, (float)delta * returnVel);
		
		if (Scale.DistanceTo(originalScale) < 0.1f) // Si esta cerca de la escala original
		{
			Scale = originalScale;
			resettingScale = false;
		}
	}

	public void StartResetScale()
	{
		StopAllScaleEffects();
		resettingScale = true;
	}

	private void PopEffect(double delta)
	{
		
		float speed = 30f;
		Vector2 targetScale;

		if (popGrowing)
			targetScale = effectOriginalScale * 1.2f;
		else
			targetScale = effectOriginalScale;
		

		Scale = Scale.Lerp(targetScale, (float)delta * speed);

		if (Scale.DistanceTo(targetScale) < 0.01f)
		{
			Scale = targetScale;

			if (popGrowing)
				popGrowing = false;
			else
				popping = false;
		}
	}

	public void StartPop()
	{
		StopAllScaleEffects();
		popping = true;
		popGrowing = true;
		effectOriginalScale = Scale;
	}	

	private void RotationToZero(double delta){
		/*GD.Print(Rotation);
		float lerpVel = 50f;
		Rotation = Mathf.Lerp(Rotation, 0f, (float)delta * lerpVel); //hace un fade out cuando se acaba el tiempo
				if (Mathf.Abs(Rotation) < 0.01f)  // Si la rotacion esta cerca de la original apaga el shake	
				{
					Rotation = 0;
					rotatingToZero = false;
				}
	*/
		Rotation = 0;
		rotatingToZero = false;
	}

	public void StartRotationToZero()
	{
		rotatingToZero = true;
	}

	private void ReturnToOriginalRotation(double delta)
	{
		/*float lerpVel = 5f;
		Rotation = Mathf.Lerp(Rotation, originalRotation, (float)delta * lerpVel); //hace un fade out cuando se acaba el tiempo
				if (Mathf.Abs(Rotation - originalRotation) < 0.01f)  // Si la rotacion esta cerca de la original apaga el shake	
				{
					Rotation = originalRotation;
					returningToOriginalRotation = false;
				}
		*/
		Rotation = originalRotation;
		returningToOriginalRotation = false;
	}

	public void StartReturnToOriginalRotation()
	{
		returningToOriginalRotation = true;
	}

	private void StopAllRotationEffects()
{
    shaking = false;
    rotatingToZero = false;
    returningToOriginalRotation = false;
}

private void StopAllScaleEffects()
{
    shrinking = false;
    resettingScale = false;
    popping = false;
	Scale = originalScale;
}





}


