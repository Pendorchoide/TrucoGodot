using Godot;
using System;
using System.Numerics;

public partial class Card : Node2D
{
	[Export] public bool isBeingDragged = false;
	[Export] public bool isOnTable = false;

	public int originalZIndex;
	[Export] public Godot.Vector2 originalPosition;
	[Export] public Godot.Vector2 originalGlobalPosition;
	public float originalRotation;
	public Godot.Vector2 originalScale;


	[Export] public string suit; // Palo de la carta
	[Export] public int value;   // Valor numerico de la carta




	//Hover effect variables
	[Export] public bool hovered = false;
			 public bool cancelHoverEffect = false; //para frenar el hover si esta activo



	//Tween Variables
	private Tween shrinkTween;
	private Tween rotationTween;
	private Tween scaleTween;
	private Tween moveTween;








	/*public Card(int value, string suit)
	{
		this.value = value;
		this.suit = suit;
	}
*/

	 public void Init(string suit, int value)
    {
        this.suit = suit;
        this.value = value;

        SetCardSprite();
    }

	



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

		bool scaleEffectActive = (scaleTween != null && scaleTween.IsValid());


		
	}

	private void SetCardSprite()
	{

		Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");

		sprite.RegionEnabled = true;
		if (value < 8) {
			sprite.RegionRect = new Rect2(
			(value - 1) * 62,
			GetSuitOffset(suit) * 95,
			62,
			95);
		} else {
			sprite.RegionRect = new Rect2(
			(value - 3) * 62,
			GetSuitOffset(suit) * 95,
			62,
			95
			);
		}
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


	public void ResetOriginalCardProperties()
	{

		originalPosition = GlobalPosition;
		originalGlobalPosition = GlobalPosition;
		originalScale = Scale;
		originalRotation = Rotation;
		originalZIndex = ZIndex;
	}


	public void StartShaking(
	
		float strength = 0.08f,
		float duration = 0.15f,
		int steps = 6 // Número de sacudidas
	)
	{
		// Si ya hay un tween de sacudida, lo matamos
		rotationTween?.Kill();

		float originalRot = Rotation;
		rotationTween = CreateTween();

		float stepTime = duration / steps;  // Duración de cada sacudida

		for (int i = 0; i < steps; i++)
		{
			float dir = (i % 2 == 0) ? 1f : -1f;
			float targetRot = originalRot + dir * strength;

			rotationTween.TweenProperty(
				this,
				"rotation",
				targetRot,
				stepTime
			);
		}

    // Volver EXACTO al valor original
    rotationTween.TweenProperty(
        this,
        "rotation",
        originalRot,
        stepTime
		);
	}


	public void StartReturnToOrigin(
		float speed = 0.3f
	)
	{
		  StopAllPositionEffects();

		moveTween?.Kill();
		
		moveTween = CreateTween();
		moveTween.SetEase(Tween.EaseType.Out);
		moveTween.SetTrans(Tween.TransitionType.Back);
		moveTween.TweenProperty(
			this,
			"global_position",
			originalGlobalPosition,
			speed   // duración en segundos
		);
	}



	public void StartGoToPosition(Godot.Vector2 targetPosition, float speed = 0.25f)
	{
		StopAllPositionEffects();

		moveTween?.Kill();

		moveTween = CreateTween();
		moveTween.SetEase(Tween.EaseType.Out);
		moveTween.SetTrans(Tween.TransitionType.Back);

		moveTween.TweenProperty(
			this,
			"position",
			targetPosition,
			speed
		);
	}



	public void StartShrink()
	{
		const float DESIRED_SCALE = 1f;
		float tweenDuration = 0.15f;

		

		// Si ya hay un tween de escala, lo matamos
		shrinkTween?.Kill();

		shrinkTween = CreateTween();
		shrinkTween.SetEase(Tween.EaseType.Out);
		shrinkTween.SetTrans(Tween.TransitionType.Quad);

		shrinkTween.TweenProperty(
			this,
			"scale",
			new Godot.Vector2(DESIRED_SCALE, DESIRED_SCALE),
			tweenDuration   // duración en segundos
		);
	}



	public void StartResetScale()
	{
		float tweenDuration = 0.2f;

		shrinkTween?.Kill();

		shrinkTween = CreateTween();
		shrinkTween.SetEase(Tween.EaseType.Out);
		shrinkTween.SetTrans(Tween.TransitionType.Back);

		shrinkTween.TweenProperty(
			this,
			"scale",
			originalScale,
			tweenDuration   // duración en segundos
		);
	}


	public void StartPop(
		float strength = 1.2f,
		float speed = 0.12f
	)
	{
		StopAllScaleEffects();
		StopAllRotationEffects();
		// Si ya hay un pop en curso, lo matamos
		scaleTween?.Kill();

		Godot.Vector2 baseScale = originalScale;
		Godot.Vector2 popScale = baseScale * strength;

		scaleTween = CreateTween();
		scaleTween.SetEase(Tween.EaseType.Out);
		scaleTween.SetTrans(Tween.TransitionType.Back);

		scaleTween.TweenProperty(this, "scale", popScale, speed);
		//scaleTween.TweenProperty(this, "scale", baseScale, speed);
	}

	public void StartRotationToZero(float duration = 0.15f)
	{
		rotationTween?.Kill();

		rotationTween = CreateTween();
		rotationTween.SetTrans(Tween.TransitionType.Sine);
		rotationTween.SetEase(Tween.EaseType.Out);

		rotationTween.TweenProperty(this, "rotation", 0f, duration);
	}

	public void StartReturnToOriginalRotation(float duration = 0.2f)
	{
		rotationTween?.Kill();

		rotationTween = CreateTween();
		rotationTween.SetTrans(Tween.TransitionType.Sine);
		rotationTween.SetEase(Tween.EaseType.Out);

		rotationTween.TweenProperty(
			this,
			"rotation",
			originalRotation,
			duration
		);
	}

	public void StartHoverEffect(float speed = 0.12f)
	{
		scaleTween?.Kill();

		Godot.Vector2 targetScale = originalScale * 1.2f;

		scaleTween = CreateTween();
		scaleTween.SetTrans(Tween.TransitionType.Sine);
		scaleTween.SetEase(Tween.EaseType.Out);

		scaleTween.TweenProperty(
			this,
			"scale",
			targetScale,
			speed
		);
	}

	public void StopHover(float speed = 0.15f)
	{
		scaleTween?.Kill();

		scaleTween = CreateTween();
		scaleTween.SetTrans(Tween.TransitionType.Sine);
		scaleTween.SetEase(Tween.EaseType.Out);

		scaleTween.TweenProperty(
			this,
			"scale",
			originalScale,
			speed
		);
	}

	public void StopAllPositionEffects()
	{
		moveTween?.Kill();
   		moveTween = null;
	}
	

	private void StopAllRotationEffects()
	{
		rotationTween?.Kill();
    	rotationTween = null;
	}

	private void StopAllScaleEffects()
	{
		scaleTween?.Kill();
    	scaleTween = null;
	}

	public void StopAllEffects()
	{
		StopAllPositionEffects();
		StopAllRotationEffects();
		StopAllScaleEffects();
	}



}


