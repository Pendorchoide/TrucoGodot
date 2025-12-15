using Godot;
using System;

public partial class CardManager : Node2D
{	
	

	const int COLLISION_MASK_CARD = 1; // Mascara de colision para las cartas (capa 1)
	Card draggedCard; 	// Variable para almacenar la carta que se esta arrastrando
	Vector2 screenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		 screenSize = GetViewportRect().Size;
	}

	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			if (draggedCard != null)
			{
				
				var mousePos = GetViewport().GetMousePosition();
				draggedCard.Position = new Vector2(				//Actualiza la posicion de la carta arrastrada a la posicion del mouse
    			Mathf.Clamp(mousePos.X, 0, screenSize.X),		//limita la posicion X dentro de los limites de la pantalla
   				Mathf.Clamp(mousePos.Y, 0, screenSize.Y));      //limita la posicion Y dentro de los limites de la pantalla
				
			}
	}


    public override void _Input(InputEvent @event)  //detecta inputs todo el tiempo
    {

		if (@event is InputEventMouseButton mouseButtonEvent)  //si el evento es un click de mouse
		{
			if (mouseButtonEvent.Pressed)		//si el boton del mouse esta presionado
			{
				if (mouseButtonEvent.ButtonIndex == MouseButton.Left) //si el boton presionado es el izquierdo
				{
					Node2D cardChild = Raycast_check();	 		//Comprueba si el mouse esta sobre un Area2D y obtiene la carta (Node2D) correspondiente
					if (cardChild != null){
						draggedCard = cardChild as Card;   			//si hay una carta bajo el mouse, la asigna a draggedCard para arrastrarla
						draggedCard.GetParent().MoveChild(draggedCard, draggedCard.GetParent().GetChildCount() - 1); //Mueve la carta al final de la lista de hijos para que se renderice encima de las demas
						draggedCard.StartShaking();      //Inicia el efecto de sacudida al comenzar a arrastrar

					}
					//GD.Print("Click Izquierdo");
				}
			}
			else
			{
					if (draggedCard != null)
						draggedCard.ReturnToOrigin();  //Cuando se suelta el boton del mouse, la carta vuelve a su posicion original
					
					draggedCard = null;
					
			}
		}  

    }


	public Node2D Raycast_check(){						//Comprueba si el mouse esta sobre un Area2D
		var spaceState = GetWorld2D().DirectSpaceState;     //Obtiene el estado del espacio 2D
		PhysicsPointQueryParameters2D parameters = new PhysicsPointQueryParameters2D();  //Crea los parametros para la consulta
		parameters.Position = GetViewport().GetMousePosition();  //Establece la posicion del mouse en los parametros
		parameters.CollideWithAreas = true;  //Habilita la colision con Areas2D
		parameters.CollisionMask = COLLISION_MASK_CARD;        //Establece la mascara de colision (1 = capa 1)  sabra dios para que
		var result = spaceState.IntersectPoint(parameters);  //Realiza la consulta y obtiene los resultados (si el mouse esta sobre un Area2D)
		
		if (result.Count > 0){  				  //Si hay resultados (el mouse esta efectivamente sobre un Area2D)
			var dict = result[0]; 		// primer resultado (el primer diccionario en el array de resultados)
			var collider = (Node)dict["collider"];  // obtiene el collider del resultado (el campo del diccionario)
			return (collider.GetParent() as Node2D); //Imprime el padre del collider (la carta)
		}
		else
		{
			return null;
		}
	}
	

}
