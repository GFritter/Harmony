using Godot;
using System;

public class Bullet : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public float speed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LinearVelocity = speed*Vector2.Up;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
