using Godot;
using System;

public class Enemy : PathFollow2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public int maxLife;
    [Export]
    public float speed;

    [Export]
    public int moneyValue;

    private ProgressBar lifeBar;
    private int actualLife;

    [Export]
    public int damage;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        actualLife= maxLife;
        lifeBar = GetNode<ProgressBar>("LifeBar");
        lifeBar.MaxValue = maxLife;
        
        lifeBar.Value = actualLife;

    }

    public void TakeDamage(int d)
    {
        actualLife -=d;
        lifeBar.Value = actualLife;
    }

    public void move()
    {

    }

    public void onDeath()
    {

        QueueFree();

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
     {

      if(actualLife<=0)
      onDeath();

      Offset += speed*delta;

     }
}
