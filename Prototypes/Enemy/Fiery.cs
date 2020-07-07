using Godot;
using System;

public class Fiery : Enemy
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    Godot.Collections.Array<Vector2> RangePos;
    bool freakOut = false;

    CollisionShape2D frontCol;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        actualLife= maxLife;
        lifeBar = GetNode<ProgressBar>("LifeBar");
        lifeBar.MaxValue = maxLife;
        
        lifeBar.Value = actualLife;

        sprite = GetNode<Area2D>("Area2D").GetNode<AnimatedSprite>("AnimatedSprite");
        auraSprite = GetNode<Area2D>("Area2D").GetNode<AnimatedSprite>("Aura");
        frontCol = GetNode<CollisionShape2D>("FrontCollision/Front");

        auraSprite.Modulate = auraColor;

        if(GetParentOrNull<Spawner>()!=null)
        {Spawner dad =(Spawner) GetParent();
        
        points = dad.Curve.GetBakedPoints();
        idx =0;
        currentPoint = points[idx];
        nextPoint=points[idx+1 %points.Length];
        checkRotation();
        }

        GetNode<AudioStreamPlayer>("OnSpawn").Play();
    }

  
    public override void  move(float delta)
    {

      Offset += speed*delta;
      
      float distance =Position.DistanceTo(nextPoint);
     
    
      if(distance < speed*delta)
      {
          if(idx+2 <points.Length)
        {
          idx++;
          currentPoint = points[idx%points.Length];
          nextPoint = points[idx+1 % points.Length];

          checkRotation();
        }
      }

        sprite.Play();
        auraSprite.Play();

    }

   
   

   

    public override void  checkRotation()
    {   
        Vector2 deltaPos = nextPoint - currentPoint;
        float angle = deltaPos.Angle();
        angle = Mathf.Rad2Deg(angle);

        float angleMod = Mathf.PosMod(angle,360);
        GetNode<CPUParticles2D>("Particles").Direction = -deltaPos;
        GetNode<CPUParticles2D>("Particles2").Direction = -deltaPos;

        if(angleMod>90 && angleMod<180)
        {
            rot = 3;
        }

        else if(angleMod >180 && angleMod<270)
        {
            rot=0;
        }

        else if(angleMod>270 && angleMod<360)
        {
            rot =1;
        }

        else
        {
            rot =2;
        }

        frontCol.Position = RangePos[rot];
        sprite.Animation="Pos"+rot;
        auraSprite.Animation = "Pos"+rot;


    }

    public void treatCollision(Area2D area)
    {
        if(area.IsInGroup("Tower"))
        {
            //set the control bool to true and start the timer nd set animation
            freakOut = true;
            GetNode<Timer>("Timer").Start();
        }
    }

   void endFreakOut()
   {
       //reset the nimation aand go back to normal
       freakOut = false;
   }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
     {
         if(!dead && !freakOut)
        move(delta);
       

     }


}
