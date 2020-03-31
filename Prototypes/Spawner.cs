using Godot;
using System;

public class Spawner : Node2D
{
    [Export]
    public PackedScene box; //pode ser trpcado por inimigo dps

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public int[] numEnemies;

    [Export]
    public int numWaves;

    int currentWave;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Timer>("Timer").Start();
    }

    public void Spawn()
    {
        Enemy enemyInstance = new Enemy();
        enemyInstance = (Enemy)box.Instance();

        AddChild(enemyInstance);
        enemyInstance.Position = this.Position;
    }

    public void setTimer(float t)
    {
        GetNode<Timer>("Timer").WaitTime = t;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
