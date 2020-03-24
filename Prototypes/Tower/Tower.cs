using Godot;
using System;

public class Tower : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
   public PackedScene bullet;
   private Position2D spawnPoint;

[Export]

public string keycode;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        spawnPoint = GetNode<Position2D>("SpawnPosition");

        Fire();

        GetNode<Range>("Range").setKeycode(keycode);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void Fire()
    {
        Bullet bulletInstance = new Bullet();
        bulletInstance = (Bullet)bullet.Instance();

        AddChild(bulletInstance);

        bulletInstance.Position = spawnPoint.Position;

    }
}
