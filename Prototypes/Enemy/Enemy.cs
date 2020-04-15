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




    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        actualLife= maxLife;
        lifeBar = GetNode<ProgressBar>("LifeBar");
        lifeBar.MaxValue = maxLife;
        
        lifeBar.Value = actualLife;

    }

    public void TakeDamage(float d)
    {
        actualLife -=d;
        lifeBar.Value = actualLife;
    }

    public void move(float delta)
    {

      Offset += speed*delta;

    }

    void setMaxHP()
    {

    }

    public void onDeath()
    {
        EmitSignal("OnDeath",moneyValue);
        QueueFree();

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
     {

      if(actualLife<=0)
      onDeath();

        move(delta);
     }
}
