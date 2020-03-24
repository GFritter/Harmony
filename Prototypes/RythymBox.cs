using Godot;
using System;

public class RythymBox : RigidBody2D
{

    [Export]
    public int speed = 100;
    //public Position2D startPos;


    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
     LinearVelocity = Vector2.Down * speed;
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      // Position+=(Vector2.Down * speed) * delta ;
      LinearVelocity = Vector2.Down * speed;
  }

  public void setSpeed(int s)
  {
      speed = s;
  }
}
