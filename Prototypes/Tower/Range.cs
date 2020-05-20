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
   
    if(targetBox.Owner.GetGroups().Contains("Enemy"))
        targetEnemy = (Enemy)targetBox.Owner;

}

public void OnCollisorExit(Area2D area)
{
    onSpot = false;
    if(tower !=null && area!=null && area.Owner!=null)
    {if(tower.canShoot && targetEnemy!=null && area.Owner.GetGroups().Contains("Enemy"))
    {
        EmitSignal("idleHit",targetEnemy);

    }
    }
    
}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
public override void _Process(float delta)
  { if(tower.canShoot)
   {if(onSpot)
      { 
          sprite.Animation = "ready";
          sprite.Modulate = new Color(0.5f,0.5f,0f);

          if(Input.IsActionJustPressed(keycode))
          {
              sprite.Animation = "right";
                sprite.Modulate = new Color(0f,0.75f,0f);

              EmitSignal("Hit",targetEnemy);
          }
        
      }

      else
      {
          sprite.Animation = "idle";
           sprite.Modulate = new Color(1f,1f,1f);

          if(Input.IsActionJustPressed(keycode))
          {
              sprite.Animation = "wrong";
               sprite.Modulate = new Color(1f,0f,0f);
              EmitSignal("Wrong");
              
          }
      }
      

     
  }

  else
  {
      sprite.Animation = "wrong";
        sprite.Modulate = new Color(1f,0f,0f);
  }

   sprite.Play();
  }

}
