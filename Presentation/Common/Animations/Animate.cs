using Godot;
using System.Collections.Generic;


public static class Animate {

    private class Tweens {
        private Node2D target;

        public Tween rotation;
        public Tween scale;
        public Tween translation;

        public Tweens(Node2D target) {
            this.target = target;
        }

        public void ScaleTweenProperty(
            float speed,
            float scaleFactor,
            Tween.TransitionType transitionType = Tween.TransitionType.Sine,
            Tween.EaseType easeType = Tween.EaseType.Out
        ) {
            if (scale != null && scale.IsValid())
                scale.Kill();

            scale = target.CreateTween();
            scale.SetTrans(transitionType);
            scale.SetEase(easeType);

            scale.TweenProperty(
                target,
                "scale",
                target.Scale * scaleFactor,
                speed
            );
        }

        public void RotationTweenProperty(
            float speed,
            float angle,
            Tween.TransitionType transitionType = Tween.TransitionType.Sine,
            Tween.EaseType easeType = Tween.EaseType.Out,
            bool chainable = false
        ) {
            if (rotation != null && rotation.IsValid() && !chainable) {
                rotation.Kill();
            }

            float originalTargetRotation = target.Rotation;

            if (!chainable || rotation == null || !rotation.IsValid()) {
                rotation = target.CreateTween();
                rotation.SetTrans(transitionType);
                rotation.SetEase(easeType);
            }
            
            rotation.TweenProperty(
                target,
                "rotation",
                originalTargetRotation + angle,
                speed
            );

        }


    }



    private static readonly Dictionary<Node2D, Tweens> tweens = new();

    // ---------- HOVER ----------

    public static void StartHover(
        Node2D target,
        float speed = 0.12f,
        float scaleFactor = 1.2f
    ) {
        if (!tweens.ContainsKey(target))
            tweens[target] = new Tweens(target);

        tweens[target].ScaleTweenProperty(
            speed,
            scaleFactor
        );
    }

    

    public static void StopHover(
        Node2D target,
        float speed = 0.12f,
        float scaleFactor = 1/1.2f)
    {
       if (!tweens.ContainsKey(target))
            tweens[target] = new Tweens(target);

        tweens[target].ScaleTweenProperty(
            speed,
            scaleFactor
        );
    }

    // ---------- SHAKE ----------

    public static void StartShaking(
        Node2D target,
        float strength = .1f,
        float duration = .3f,
        int steps = 6)
    {
        if (!tweens.ContainsKey(target))
            tweens[target] = new Tweens(target);


        float stepTime = duration / (steps + 1);


        for (int i = 0; i < steps; i++)
        {
            float dir = (i % 2 == 0) ? 1f : -1f;
            
            tweens[target].RotationTweenProperty(
                stepTime,
                dir * strength,
                default,
                default, 
                true
            );
        }


        tweens[target].RotationTweenProperty(
            strength,
            0f, // Return to original rotation
            default,
            default,
            true
        );
    }

}
          
