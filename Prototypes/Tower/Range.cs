using Godot;
using System;

public class Range : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
     [Signal]
    public delegate void Hit(Enemy e);

    [Signal]
    public delegate void idleHit(Enemy e);

    [Signal]
    public delegate void Wrong();



    private AnimatedSprite sprite;
    private string keycode;

    Tower tower;
    Area2D targetBox;
    Enemy targetEnemy,lastEnemy;
    public bool onSpot;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sprite  = GetNode<AnimatedSprite>("RangeSprite");
        tower = (Tower)Owner;
        onSpot = false;
    }

  public void setKeycode(string k)
    {
        keycode = k;
    }

    public void OnCollisorEnter(Area2D area)
{
    onSpot = true;
    targetBox = area;
   
    targetEnemy = (Enemy)targetBox.Owner;

}

public void OnCollisorExit(Area2D area)
{
    onSpot = false;

    if(tower.canShoot)
    {
        EmitSignal("idleHit",targetEnemy);

    }
    
}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
public override void _Process(float delta)
  { if(tower.canShoot)
   {if(onSpot)
      { 
          sprite.Animation = "ready";

          if(Input.IsActionJustPressed(keycode))
          {
              sprite.Animation = "right";
            

              EmitSignal("Hit",targetEnemy);
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
      

     
  }

  else
  {
      sprite.Animation = "wrong";
  }

   sprite.Play();
  }

}
