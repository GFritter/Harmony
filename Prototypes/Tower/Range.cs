using Godot;
using System;

public class Range : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
     [Signal]
    public delegate void Hit(Godot.Collections.Array<Enemy> enemiesInRange);

    [Signal]
    public delegate void idleHit(Godot.Collections.Array<Enemy> enemiesInRange);

    [Signal]
    public delegate void Wrong();



    private AnimatedSprite sprite;
    private string keycode;

    Tower tower;
    Area2D targetBox;
    Godot.Collections.Array<Enemy> enemiesInRange;
    Enemy targetEnemy,lastEnemy;
    public bool onSpot;

    public Color spot,wrong,right,idle; 

public void setColor(Color c)
{
    idle = c*0.25f;
    spot = new Color(c.r,c.g,c.b,0.5f);
    right = new Color(c.r+0.2f,c.g+0.2f,c.b+0.2f,0.6f);
    wrong = new Color(c.r-0.3f,c.g-0.3f,c.b-0.3f,0.6f);
}
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sprite  = GetNode<AnimatedSprite>("RangeSprite");
        enemiesInRange = new Godot.Collections.Array<Enemy>();
        
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
   
    if(targetBox.GetGroups().Contains("Enemy"))
       enemiesInRange.Add((Enemy)targetBox.Owner);

  checkArray();
}

public void OnCollisorExit(Area2D area)
{

    if(IsInstanceValid(area.Owner) && area.GetGroups().Contains("Enemy"))
    {
        
        if(tower.canShoot )
        {
        
        EmitSignal("idleHit",enemiesInRange);

         }
         checkArray();

        if(enemiesInRange.Contains((Enemy)area.Owner))
         {
          enemiesInRange.Remove((Enemy)area.Owner);
         }
    }

    if(enemiesInRange.Count<=0)
    {
          onSpot = false;
    }
    checkArray();
}

void checkArray()
{
    for(int i=0;i<enemiesInRange.Count;i++)
    {
       
        if(Godot.Object.IsInstanceValid(enemiesInRange[i]) &&enemiesInRange[i].dead)
        {
            tower.getXp(enemiesInRange[i].ExpValue);
            enemiesInRange[i].onDeath();
            enemiesInRange.RemoveAt(i);
        }

        else if(!Godot.Object.IsInstanceValid(enemiesInRange[i]))
        {
            GD.Print("Achei um null");
        }
    }
}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
public override void _Process(float delta)
  { if(tower.canShoot)
   {if(onSpot)
      { 
          sprite.Animation = "ready";
          sprite.Modulate = spot;

          if(Input.IsActionJustPressed(keycode))
          {
              sprite.Animation = "right";
                sprite.Modulate = right;

              EmitSignal("Hit",enemiesInRange);
              checkArray();
          }
        
      }

      else
      {
          sprite.Animation = "idle";
           sprite.Modulate = new Color(1f,1f,1f);

          if(Input.IsActionJustPressed(keycode))
          {
              sprite.Animation = "wrong";
               sprite.Modulate = wrong;
              EmitSignal("Wrong");
              
          }
      }
      

     
  }

  else
  {
      sprite.Animation = "wrong";
        sprite.Modulate = wrong;
  }

   sprite.Play();
  }

}
