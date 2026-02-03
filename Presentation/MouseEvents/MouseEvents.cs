using System;
using Godot;

public partial class MouseEvents : Node{

    private Node2D parent;

    public bool MouseMotion;
    public bool LeftClickPressed;
    public bool RightClickPressed;

    public bool LeftClickReleased;
    public bool RightClickReleased;

    public MouseEvents(Node2D parent){
      this.parent = parent;
    }


    public void HandleInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
               LeftClickPressed = true;
               
            }
            else if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
               LeftClickReleased = true;
            }
            else{
                LeftClickPressed = false;
                LeftClickReleased = false;
            }
        }

        if (@event is InputEventMouseMotion motion)
        {
            MouseMotion = true;
        }
        else{
            MouseMotion = false;
        }

        
    }


}