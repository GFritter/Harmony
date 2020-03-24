using Godot;
using System;

public class Collisor : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Signal]
    public delegate void Hit();

    [Signal]
    public delegate void Wrong();

    private AnimatedSprite sprite;
    private string keycode;

    PhysicsBody2D targetBox;

    public bool onSpot;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        sprite  = GetNode<AnimatedSprite>("AnimatedSprite");

        onSpot = false;
        
    }

   public void setKeycode(string k)
    {
        keycode = k;
    }

    public void OnCollisorEnter(PhysicsBody2D body)
{
    onSpot = true;
    targetBox = body;
}

public void OnCollisorExit(PhysicsBody2D body)
{
    onSpot = false;
}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
public override void _Process(float delta)
  {
      if(onSpot)
      { 
          sprite.Animation = "ready";

          if(Input.IsActionJustPressed(keycode))
          {
              sprite.Animation = "right";
              targetBox.QueueFree();

              EmitSignal("Hit");
          }
        
      }

      else
      {
          sprite.Animation = "idle";

          if(Input.IsActionJustPressed(keycode))
          {
              sprite.Animation = "wrong";
              EmitSignal("Wrong");
              
          }
      }
      

      sprite.Play();
  }
}
