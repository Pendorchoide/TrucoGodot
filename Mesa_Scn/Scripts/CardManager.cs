using Godot;
using System;

public partial class CardManager : Node2D
{	
	

	const int MASK_CARD  = 1;
	const int MASK_TABLE = 2;
	const int MASK_ALL  = MASK_CARD | MASK_TABLE;

	const int Z_HAND_BASE = 10;
	const int Z_HAND_HOVER = 20;
	const int Z_HAND_DRAG = 30;
	private int hoverZCounter = Z_HAND_BASE;
	private int globalZCounter = 1; // Contador global para asignar ZIndex unico a las cartas arrastradas

	private Card draggedCard = null; 	// Variable para almacenar la carta que se esta arrastrando
	private Card currentHoveredCard = null; 	// Variable para almacenar la carta que se esta haciendo hover

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
				
				var mousePos =GetGlobalMousePosition();
				draggedCard.GlobalPosition = new Vector2(				//Actualiza la posicion de la carta arrastrada a la posicion del mouse
    			Mathf.Clamp(mousePos.X, 0, screenSize.X),		//limita la posicion X dentro de los limites de la pantalla
   				Mathf.Clamp(mousePos.Y, 0, screenSize.Y));      //limita la posicion Y dentro de los limites de la pantalla
				
			}
			UpdateHover();
	}


    public override void _Input(InputEvent @event)  //detecta inputs todo el tiempo
    {

		if (@event is InputEventMouseButton mouseButtonEvent)  //si el evento es un click de mouse
		{
			if (mouseButtonEvent.Pressed)	//si el boton del mouse esta presionado
			{
				if (mouseButtonEvent.ButtonIndex == MouseButton.Left) //si el boton presionado es el izquierdo
				{
					Node clickedArea = Raycast_check(); //guarda el area2D clickeada por el mouse
					Card cardChild = FindNodeInGroup(clickedArea, "card") as Card; //Comprueba si se clickeo sobre una carta


					
					if (cardChild != null && !cardChild.isOnTable) //si se clickeo sobre una carta que no este en la mesa
					{
						//draggedCard.GetNode<Area2D>("Area2D").CollisionLayer = 0;
						draggedCard = cardChild; 			//si hay una carta bajo el mouse, la asigna a draggedCard para arrastrarla
						
						draggedCard.isBeingDragged = true;

						//draggedCard.GetParent().MoveChild(draggedCard, draggedCard.GetParent().GetChildCount() - 1); //Mueve la carta al final de la lista de hijos para que se renderice encima de las demas
						//draggedCard.StartRotationToZero(); //Inicia el efecto de rotacion a cero al comenzar a arrastrar
						draggedCard.Rotation = 0; //Establece la rotacion a cero al comenzar a arrastrar
						draggedCard.StartShaking();      //Inicia el efecto de sacudida al comenzar a arrastrar
						draggedCard.StartShrink();        //Inicia el efecto de encogimiento al comenzar a arrastrar
					}
					//GD.Print("Click Izquierdo");
						
					
				}
			}
			else
			{	
				if (draggedCard != null)
				{
					draggedCard.isBeingDragged = false;
					

					Node2D releasedArea = Raycast_check() as Node2D; //guarda el area2D clickeada por el mouse
					bool droppedOnTable = releasedArea != null && releasedArea.IsInGroup("table");
					
					if (droppedOnTable && !draggedCard.isBeingDragged) //Si se suelta la carta sobre la mesa
					{
						
						 PlaceCardOnTable(draggedCard, releasedArea as Table);
					}
					else
					{
						if (!draggedCard.isBeingDragged)
						{
							draggedCard.StartReturnToOriginalRotation(); //Inicia el efecto de volver a la rotacion original al soltar
							draggedCard.hovered = false; //Desactiva el hover al soltar la carta
							draggedCard.StartReturnToOrigin();  //Cuando se suelta el boton del mouse, la carta vuelve a su posicion original
							draggedCard.StartResetScale();      //Inicia el efecto de volver a la escala original al soltar
							//draggedCard.StartReturnToOriginalRotation(); //Inicia el efecto de volver a la rotacion original al soltar
							draggedCard.Rotation = draggedCard.originalRotation; //Establece la rotacion a la original al soltar
							//draggedCard.StartShaking();      //Inicia el efecto de sacudida al soltar
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
			card.isOnTable = true;
            table.playableSlotInTable[i] = card;
            card.Reparent(table);
			
			
			card.StopAllEffects();
            card.hovered = false;
            card.isBeingDragged = false;

			card.Scale = Vector2.One;
			card.originalScale = Vector2.One;

			card.Rotation = 0;
			card.originalRotation = 0;

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




	public Node Raycast_check(){															//Comprueba si el mouse esta sobre un Area2D
		var spaceState = GetWorld2D().DirectSpaceState;     								//Obtiene el estado del espacio 2D
		
		var parameters = new PhysicsPointQueryParameters2D		//Crea los parametros para la consulta
		{ 	
			Position = GetGlobalMousePosition(),  									//Establece la posicion del mouse en los parametros
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
        var table = FindNodeInGroup(collider, "table");
		if (table != null)
            return collider.GetParent<Node2D>();
    }

    // Si no hay mesa, devolver carta
    foreach (var hit in result)
    {
        var collider = (Node)hit["collider"];
        var card = FindNodeInGroup(collider, "card");
		if (card != null)
            return collider.GetParent<Node2D>();
    }

    return null;
	}




	
private const float SWITCH_DISTANCE_THRESHOLD = 15f; // pixels

private void UpdateHover()
{
    if (draggedCard != null)
        return;

    var hoveredCards = GetHoveredCards();

    if (hoveredCards.Count == 0)
    {
        ClearHover();
        return;
    }

    Card bestCandidate = GetBestCandidate(hoveredCards);

	if (bestCandidate == null)
	{
		ClearHover();
		return; 
	}

	if (bestCandidate.isOnTable)
	{
		ClearHover();
		return;
	}

    if (currentHoveredCard == null) // si no hay ninguna carta en hover
    {
        SetHover(bestCandidate);  //se aplica la mejor candidata
        return;
    }

    // Si es la misma, no hacemos nada
    if (bestCandidate == currentHoveredCard)
        return;

    // Comparación con hysteresis
    //if (IsBetterCandidate(bestCandidate, currentHoveredCard))
    //{
		if (bestCandidate != null)
		{
        ClearHover();
        SetHover(bestCandidate);
		}
	//}
}
	/*private bool IsBetterCandidate(Card candidate, Card current)
	{
		// ZIndex manda
		if (candidate.ZIndex > current.ZIndex)
			return true;

		if (candidate.ZIndex < current.ZIndex)
			return false;

		// Distancia SOLO si es claramente mejor
		float dCandidate = candidate.GlobalPosition.DistanceTo(GetGlobalMousePosition());
		float dCurrent   = current.GlobalPosition.DistanceTo(GetGlobalMousePosition());

		return dCandidate + SWITCH_DISTANCE_THRESHOLD < dCurrent;
	}*/

	private void SetHover(Card card)
	{
		currentHoveredCard = card;
		card.hovered = true;
		card.StartHoverEffect();

		hoverZCounter++;
		card.SetZIndex(hoverZCounter);
	}

	private void ClearHover()
	{
		if (currentHoveredCard == null)
			return;
		currentHoveredCard.StopHover();
		currentHoveredCard.hovered = false;
		currentHoveredCard.StartResetScale();
		currentHoveredCard = null;
		
	}

	
	public void BringCardToFront(Card card)
	{
		//card.ZIndex = globalZCounter;
		//card.originalZIndex = card.ZIndex;
		globalZCounter++;
	}

	public Godot.Collections.Array<Card> GetHoveredCards() //Obtiene todas las cartas bajo el mouse que no esten siendo arrastradas
	{
		var spaceState = GetWorld2D().DirectSpaceState;

		var parameters = new PhysicsPointQueryParameters2D
		{
			Position = GetGlobalMousePosition(),
			CollideWithAreas = true,
			CollisionMask = MASK_CARD
		};

		

		var result = spaceState.IntersectPoint(parameters);
		var cards = new Godot.Collections.Array<Card>();

		foreach (var hit in result)
		{
			var collider = (Node2D)hit["collider"];
			var card = FindNodeInGroup(collider, "card") as Card;

			if (card != null && !card.isBeingDragged)
				cards.Add(card);
		}

		return cards;
		}
		private Card GetBestCandidate(Godot.Collections.Array<Card> cards)
		{
			Card best = null;

			foreach (var card in cards)
			{
				if (card == null || card.isBeingDragged || card.isOnTable)
					continue;

				if (best == null)
				{
					best = card;
					continue;
				}

				// Prioridad por ZIndex
				if (card.ZIndex > best.ZIndex)
				{
					best = card;
					continue;
				}

				// prioridad por cercania al mouse
				if (card.ZIndex == best.ZIndex)
				{
					float d1 = card.GlobalPosition.DistanceTo(GetGlobalMousePosition());
					float d2 = best.GlobalPosition.DistanceTo(GetGlobalMousePosition());

					if (d1 < d2)
					{
						best = card;
					}
				}
			}

			return best;
	}



	private Node FindNodeInGroup(Node node, string group)
	{
		while (node != null)
		{
			if (node.IsInGroup(group))
				return node;
			node = node.GetParent();
		}
		return null;
	}

	/*private Card GetTopPriorityCard(Godot.Collections.Array<Card> cards)		//Obtiene la carta con mayor prioridad (ZIndex mas alto, y si empatan, la mas cercana al mouse)
	{
    Card best = null;

    foreach (var card in cards)
    {
        if (best == null)
        {
            best = card;
            continue;
        }

        // Prioridad por ZIndex
        if (card.ZIndex > best.ZIndex)
        {
            best = card;
        }
        // Si empatan, la más cercana al mouse
        else if (card.ZIndex == best.ZIndex)
        {
            float d1 = card.GlobalPosition.DistanceTo(GetGlobalMousePosition());
            float d2 = best.GlobalPosition.DistanceTo(GetGlobalMousePosition());

            if (d1 < d2)
                best = card;
        }
    }

    return best;
	}
*/


}


