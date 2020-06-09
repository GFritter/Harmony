using Godot;
using System;

public class Enemy : PathFollow2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Signal]
    public delegate void OnDeath(int m);

    [Export]
    public float maxLife;
    [Export]
    public float speed;

    [Export]
    public int moneyValue;

    private ProgressBar lifeBar;
    private float actualLife;

    [Export]
    public int damage;

    Vector2[] points;
    int idx;
    Vector2 currentPoint,nextPoint;

    int rot;
    AnimatedSprite sprite;

    [Export]
    public int colorId;
    [Export]
    public Color auraColor;
    [Export]
    public int ExpValue;


    public bool dead;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        actualLife= maxLife;
        lifeBar = GetNode<ProgressBar>("LifeBar");
        lifeBar.MaxValue = maxLife;
        
        lifeBar.Value = actualLife;

        sprite = GetNode<Area2D>("Area2D").GetNode<AnimatedSprite>("AnimatedSprite");
        
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

    public void TakeDamage(float d)
    {
        actualLife -=d;
        lifeBar.Value = actualLife;

        
      if(actualLife<=0 )
      dead = true;

    }

    public void move(float delta)
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

    }

    public void setMaxHP(float mHP)
    {
        maxLife = mHP;
          actualLife= maxLife;
        lifeBar = GetNode<ProgressBar>("LifeBar");
        lifeBar.MaxValue = maxLife;
        
        lifeBar.Value = actualLife;
    }

    public void onDeath()
    {
        EmitSignal("OnDeath",moneyValue);
         GetNode<AudioStreamPlayer>("OnDeath").Play();
    
    }

    public void destroy()
    {
       
        QueueFree();
    }

    public void checkRotation()
    {   
        Vector2 deltaPos = nextPoint - currentPoint;
        float angle = deltaPos.Angle();
        angle = Mathf.Rad2Deg(angle);

        float angleMod = Mathf.PosMod(angle,360);

     

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

        sprite.Animation="Pos"+rot;


    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
     {
         if(!dead)
        move(delta);
       

     }


}
