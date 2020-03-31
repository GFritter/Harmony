using Godot;
using System;

public class Base : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public int maxLife = 20;

    public int life;

    [Signal]
    public delegate void Died();

    [Signal]
    public delegate void UpdateLife(int maxLife, int life);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        life = maxLife;
    }

    void TakeDamage(Area2D other)
    {
        Enemy e = (Enemy)other.Owner;

        life -= e.damage;

        e.QueueFree();

        EmitSignal("UpdateLife",maxLife,life);
        
    }

    void onDeath()
    {
        EmitSignal("Died");
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
  {
     if(life<=0)
     {
         onDeath();
     }
  }
}
