using System.Runtime.CompilerServices;
using Godot;

public partial class  CardView : Node2D {

  	//Original Godot Properties
    public int _originalZIndex;
    public Godot.Vector2 _originalPosition;
    public Godot.Vector2 _originalGlobalPosition;
    public float _originalRotation;
    public Godot.Vector2 _originalScale;

    [Export] private Sprite2D _cardSprite;
    [Export] private Area2D _area2D;

    private MouseEvents mouseEvents;




    bool Dragging = false;




    public override void _Ready() {

      Position = new Vector2(300, 300);

      //set original properties
      _originalScale = Scale;
      _originalRotation = Rotation;
      _originalPosition = Position;
      _originalZIndex = ZIndex;
       
      //connect signals
      _area2D.MouseEntered += OnMouseEntered;
      _area2D.MouseExited  += OnMouseExited;

      mouseEvents = new MouseEvents(this);

    }

    public override void _Process(double delta) {
      var  screenSize = GetViewportRect().Size;

      var mousePos = GetGlobalMousePosition();

      if (Dragging)
      GlobalPosition = new Vector2(				//Actualiza la posicion de la carta arrastrada a la posicion del mouse
    			Mathf.Clamp(mousePos.X, 0, screenSize.X),		//limita la posicion X dentro de los limites de la pantalla
   				Mathf.Clamp(mousePos.Y, 0, screenSize.Y));      //limita la posicion Y dentro de los limites de la pantalla


    }

    public override void _Input(InputEvent @event)
    {
        mouseEvents.HandleInput(@event);
    }
    

    private void OnMouseEntered() {
      if (!mouseEvents.LeftClickPressed)
        Animate.StartHover(this);
      else {
        Animate.StopHover(this);
        Dragging = true;
      }
    }

    private void OnMouseExited()
    {

      Animate.StopHover(this);
    }
    

    
  }





