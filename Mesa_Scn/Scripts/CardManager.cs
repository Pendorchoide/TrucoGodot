using Godot;
using System;

public partial class CardManager : Node2D
{	
	

	const int MASK_CARD  = 1;
	const int MASK_TABLE = 2;
	const int MASK_ALL   = MASK_CARD | MASK_TABLE;
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
			if (draggedCard != null && draggedCard.isBeingDragged)
			{
				
				var mousePos = GetViewport().GetMousePosition();
				draggedCard.GlobalPosition = new Vector2(				//Actualiza la posicion de la carta arrastrada a la posicion del mouse
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
					Node2D clickedArea = Raycast_check(); //guarda el area2D clickeada por el mouse
					
					if (clickedArea.IsInGroup("card")) //si el area2D clickeada es una carta de la Mano
					{
						Node2D cardChild = Raycast_check();	 		//Comprueba si el mouse esta sobre un Area2D y obtiene la carta (Node2D) correspondiente
						if (cardChild != null){
							//draggedCard.GetNode<Area2D>("Area2D").CollisionLayer = 0;
							draggedCard = cardChild as Card;   			//si hay una carta bajo el mouse, la asigna a draggedCard para arrastrarla
							
							draggedCard.isBeingDragged = true;

							draggedCard.GetParent().MoveChild(draggedCard, draggedCard.GetParent().GetChildCount() - 1); //Mueve la carta al final de la lista de hijos para que se renderice encima de las demas
							//draggedCard.StartRotationToZero(); //Inicia el efecto de rotacion a cero al comenzar a arrastrar
							draggedCard.Rotation = 0; //Establece la rotacion a cero al comenzar a arrastrar
							draggedCard.StartShaking();      //Inicia el efecto de sacudida al comenzar a arrastrar
							draggedCard.StartShrink();        //Inicia el efecto de encogimiento al comenzar a arrastrar
						}
						//GD.Print("Click Izquierdo");
					}

				
				}
			}
			else
			{	
				if (draggedCard != null)
				{
					draggedCard.isBeingDragged = false;
					

					Node2D releasedArea = Raycast_check(); //guarda el area2D clickeada por el mouse
					bool droppedOnTable = releasedArea != null && releasedArea.IsInGroup("table");
					
					if (droppedOnTable && !draggedCard.isBeingDragged) //Si se suelta la carta sobre la mesa
					{
						
						 PlaceCardOnTable(draggedCard, releasedArea as Table);
					}
					else
					{
						if (!draggedCard.isBeingDragged)
						{
							draggedCard.StartReturnToOrigin();  //Cuando se suelta el boton del mouse, la carta vuelve a su posicion original
							draggedCard.StartResetScale();      //Inicia el efecto de volver a la escala original al soltar
							//draggedCard.StartReturnToOriginalRotation(); //Inicia el efecto de volver a la rotacion original al soltar
							draggedCard.Rotation = draggedCard.originalRotation; //Establece la rotacion a la original al soltar
							draggedCard.StartShaking();      //Inicia el efecto de sacudida al soltar
						}
					}
					draggedCard = null; //Libera la carta arrastrada
				}
			}
		}  
	}

private void PlaceCardOnTable(Card card, Table table)
{
    for (int i = 0; i < table.playableSlotInTable.Length; i++) //recorre los slots de la mesa
    {
        if (table.playableSlotInTable[i] == null)
        {
			Vector2 tableScale = new Vector2(1f, 1f);

            table.playableSlotInTable[i] = card;
            card.Reparent(table);
			card.originalScale = tableScale;

            Vector2 slotPos = i switch
            {
                0 => table.Slot1Position,
                1 => table.Slot2Position,
                2 => table.Slot3Position,
                _ => table.Slot1Position
            };

            card.StartGoToPosition(table.ToLocal(slotPos));
            card.StartPop();
            card.SetZIndex(0);
            return;
        }
    }
}


	public Node2D Raycast_check(){															//Comprueba si el mouse esta sobre un Area2D
		var spaceState = GetWorld2D().DirectSpaceState;     								//Obtiene el estado del espacio 2D
		PhysicsPointQueryParameters2D parameters = new PhysicsPointQueryParameters2D(){ 	//Crea los parametros para la consulta
			Position = GetViewport().GetMousePosition(),  									//Establece la posicion del mouse en los parametros
			CollideWithAreas = true, 														//Habilita la colision con Areas2D
			CollisionMask = MASK_ALL        //Establece la mascara de colision (para que detecte tanto cartas como mesa)
		};
		var result = spaceState.IntersectPoint(parameters);  //Realiza la consulta y obtiene los resultados (si el mouse esta sobre un Area2D)
		
		if (result.Count == 0)
        	return null;




    // Priorizar mesa
    foreach (var hit in result)
    {
        var collider = (Node)hit["collider"];
        if (collider.GetParent<Node2D>().IsInGroup("table"))
            return collider.GetParent<Node2D>();
    }

    // Si no hay mesa, devolver carta
    foreach (var hit in result)
    {
        var collider = (Node)hit["collider"];
        if (collider.GetParent<Node2D>().IsInGroup("card"))
            return collider.GetParent<Node2D>();
    }

    return null;
	}
	

}
