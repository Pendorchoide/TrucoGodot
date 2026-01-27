using Godot;
using System;

public partial class CardVisuals : Node
{
	


	public override void _Process(double delta)
	{
		////bool scaleEffectActive = (scaleTween != null && scaleTween.IsValid());
	}


	public void SetCardSprite(
		Card targetNode,
		int value, 
		string suit
		)
	{
		Sprite2D sprite =  GetParent<Card>().GetNode<Sprite2D>("Sprite2D");

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




	public void StartShaking(
		Card targetNode,
		float strength = 0.08f,
		float duration = 0.15f,
		int steps = 6 // Número de sacudidas
	)
	{
		// Si ya hay un tween de sacudida, lo matamos
		targetNode.rotationTween?.Kill();

		float originalRot = targetNode.Rotation;
		targetNode.rotationTween = CreateTween();

		float stepTime = duration / steps;  // Duración de cada sacudida

		for (int i = 0; i < steps; i++)
		{
			float dir = (i % 2 == 0) ? 1f : -1f;
			float targetRot = originalRot + dir * strength;

			targetNode.rotationTween.TweenProperty(
				targetNode,
				"rotation",
				targetRot,
				stepTime
			);
		}

    // Volver EXACTO al valor original
    targetNode.rotationTween.TweenProperty(
        targetNode,
        "rotation",
        originalRot,
        stepTime
		);
	}


	public void StartReturnToOrigin(
		Card targetNode,
		float speed = 0.3f
	)
	{
		  StopAllPositionEffects(targetNode);

		targetNode.moveTween?.Kill();
		
		targetNode.moveTween = CreateTween();
		targetNode.moveTween.SetEase(Tween.EaseType.Out);
		targetNode.moveTween.SetTrans(Tween.TransitionType.Back);
		targetNode.moveTween.TweenProperty(
			targetNode,
			"global_position",
			targetNode.originalGlobalPosition,
			speed   // duración en segundos
		);
	}



	public void StartGoToPosition(Card targetNode, Godot.Vector2 targetPosition, float speed = 0.25f)
	{
		StopAllPositionEffects(targetNode);

		targetNode.moveTween?.Kill();

		targetNode.moveTween = CreateTween();
		targetNode.moveTween.SetEase(Tween.EaseType.Out);
		targetNode.moveTween.SetTrans(Tween.TransitionType.Back);
		targetNode.moveTween.TweenProperty(
			targetNode,
			"position",
			targetPosition,
			speed
		);
	}



	public void StartShrink(Card targetNode)
	{
		const float DESIRED_SCALE = 1f;
		float tweenDuration = 0.15f;

		

		// Si ya hay un tween de escala, lo matamos
		targetNode.shrinkTween?.Kill();

		targetNode.shrinkTween = CreateTween();
		targetNode.shrinkTween.SetEase(Tween.EaseType.Out);
		targetNode.shrinkTween.SetTrans(Tween.TransitionType.Quad);

		targetNode.shrinkTween.TweenProperty(
			targetNode,
			"scale",
			new Godot.Vector2(DESIRED_SCALE, DESIRED_SCALE),
			tweenDuration   // duración en segundos
		);
	}



	public void StartResetScale(Card targetNode)
	{
		float tweenDuration = 0.2f;

		targetNode.shrinkTween?.Kill();

		targetNode.shrinkTween = CreateTween();
		targetNode.shrinkTween.SetEase(Tween.EaseType.Out);
		targetNode.shrinkTween.SetTrans(Tween.TransitionType.Back);
		targetNode.shrinkTween.TweenProperty(
			targetNode,
			"scale",
			targetNode.originalScale,
			tweenDuration   // duración en segundos
		);
	}


	public void StartPop(
		Card targetNode,
		float strength = 1.2f,
		float speed = 0.12f
	)
	{
		StopAllScaleEffects(targetNode);
		StopAllRotationEffects(targetNode);
		// Si ya hay un pop en curso, lo matamos
		targetNode.scaleTween?.Kill();

		Godot.Vector2 baseScale = targetNode.originalScale;
		Godot.Vector2 popScale = baseScale * strength;

		targetNode.scaleTween = CreateTween();
		targetNode.scaleTween.SetEase(Tween.EaseType.Out);
		targetNode.scaleTween.SetTrans(Tween.TransitionType.Back);

		targetNode.scaleTween.TweenProperty(targetNode, "scale", popScale, speed);
		//scaleTween.TweenProperty(targetNode, "scale", baseScale, speed);
	}

	public void StartRotationToZero(
		Card targetNode,	
		float duration = 0.15f
		)
	{
		targetNode.rotationTween?.Kill();

		targetNode.rotationTween = CreateTween();
		targetNode.rotationTween.SetTrans(Tween.TransitionType.Sine);
		targetNode.rotationTween.SetEase(Tween.EaseType.Out);
		targetNode.rotationTween.TweenProperty(targetNode, "rotation", 0f, duration);
	}

	public void StartReturnToOriginalRotation(
		Card targetNode,
		float duration = 0.2f
	)
	{
		targetNode.rotationTween?.Kill();

		targetNode.rotationTween = CreateTween();
		targetNode.rotationTween.SetTrans(Tween.TransitionType.Sine);
		targetNode.rotationTween.SetEase(Tween.EaseType.Out);

		targetNode.rotationTween.TweenProperty(
			targetNode,
			"rotation",
			targetNode.originalRotation,
			duration
		);
	}

	public void StartHoverEffect(
		Card targetNode,
		float speed = 0.12f)
	{
		targetNode.scaleTween?.Kill();

		Godot.Vector2 targetScale = targetNode.originalScale * 1.2f;
		targetNode.scaleTween = CreateTween();
		targetNode.scaleTween.SetTrans(Tween.TransitionType.Sine);
		targetNode.scaleTween.SetEase(Tween.EaseType.Out);

		targetNode.scaleTween.TweenProperty(
			targetNode,
			"scale",
			targetScale,
			speed
		);
	}

	public void StopHover(
		Card targetNode,
		float speed = 0.15f)
	{
		targetNode.scaleTween?.Kill();

		targetNode.scaleTween = CreateTween();
		targetNode.scaleTween.SetTrans(Tween.TransitionType.Sine);
		targetNode.scaleTween.SetEase(Tween.EaseType.Out);	
		targetNode.scaleTween.TweenProperty(
			targetNode,
			"scale",
			targetNode.originalScale,
			speed
		);
	}

	public void StopAllPositionEffects(
		Card targetNode
	)
	{
		targetNode.moveTween?.Kill();
   		targetNode.moveTween = null;
	}
	

	private void StopAllRotationEffects(
		Card targetNode
	)
	{
		targetNode.rotationTween?.Kill();
    	targetNode.rotationTween = null;
	}

	private void StopAllScaleEffects(
		Card targetNode
	)
	{
		targetNode.scaleTween?.Kill();
    	targetNode.scaleTween = null;
	}

	public void StopAllEffects(
		Card targetNode)
	{
		StopAllPositionEffects(targetNode);
		StopAllRotationEffects(targetNode);
		StopAllScaleEffects(targetNode);
	}






}
