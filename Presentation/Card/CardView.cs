using System.Runtime.CompilerServices;
using Godot;

public partial class  CardView : Node2D {

    //Z Index Constants
    public const int Z_INDEX_DEFAULT = 0;
    public const int Z_INDEX_HOVER = 10;
    public const int Z_INDEX_DRAGGING = 100;


  	//Original Godot Properties
    public int _originalZIndex;
    public Godot.Vector2 _originalPosition;
    public Godot.Vector2 _originalGlobalPosition;
    public float _originalRotation;
    public Godot.Vector2 _originalScale;

    [Export] private Sprite2D _cardSprite;
    [Export] private Area2D _area2D;

    private MouseEvents mouseEvents;



    bool MouseOver = false;
    bool Dragging = false;
    bool Hovereable = true;




    public override void _Ready() {

      Position = new Vector2(GetViewportRect().Size.X / 2, GetViewportRect().Size.Y / 2 + 200);

      

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
      

      DragCheck();
      HoverCheck();


    }



    public override void _Input(InputEvent @event)
    {
        mouseEvents.HandleInput(@event);
    }
    

    private void OnMouseEntered() {
      MouseOver = true;
      Hovereable = true;

    }

    private void OnMouseExited()
    {
      if (Dragging) return;
      MouseOver = false;
    }
    
    private void HoverCheck(){
      if (MouseOver && Hovereable) {
        Animate.StartHover(this, _originalScale);
        ZIndex = Z_INDEX_HOVER;


      }
      else if (!MouseOver && Hovereable) {
        Animate.StopHover(this, _originalScale);
        ZIndex = _originalZIndex;
      }
    }

    private void DragCheck(){
      var  screenSize = GetViewportRect().Size;

      var mousePos = GetGlobalMousePosition();

      if (MouseOver && mouseEvents.LeftClickPressed) {
        Dragging = true;
        FollowMouseShader();
        Animate.StartHover(this, _originalScale);
        ZIndex = Z_INDEX_DRAGGING;  //Bring to front while dragging
      }

      if (Dragging){
        Vector2 MouseCordsBounded = new(				
        Mathf.Clamp(mousePos.X, 0, screenSize.X),		//limita la posicion X dentro de los limites de la pantalla
        Mathf.Clamp(mousePos.Y, 0, screenSize.Y));      //limita la posicion Y dentro de los limites de la pantalla
      
        Animate.MoveTo(this, MouseCordsBounded, .04f);
        
        if (!mouseEvents.LeftClickPressed){ //Dragging released
          Dragging = false;
          Hovereable = false;
          ZIndex = _originalZIndex;  //Return to original Z index when not dragging
          Animate.StopHover(this, _originalScale);
          _cardSprite.SetInstanceShaderParameter("mouse_position",new Vector2(0,0)); //set "skew" to 0
          Animate.MoveTo(this, _originalPosition, .2f, Tween.TransitionType.Spring, Tween.EaseType.Out);
        }
      }
    }

    private void ResetPosition(){
      Animate.MoveTo(this, _originalPosition, .1f);
    }



  // SHADERS CONTROL

  private void FollowMouseShader(){
    Vector2 mousePos = GetGlobalMousePosition();
    Vector2 localMousePos = ToLocal(mousePos);
    _cardSprite.SetInstanceShaderParameter("mouse_position", localMousePos);


  }
    
  }





