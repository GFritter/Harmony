using Godot;
using System;

public class Map : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public NodePath baseRef;

    Base b;

    [Export]
    public Array spanwRefs;

    public Spawner[] spawners;

    [Signal]
    public delegate void OnLifeUpdate(int maxLife,int life);

    [Signal]
    public delegate void OnBaseDeath();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        spawners = new Spawner[spanwRefs.Length];

        for(int i=0;i<spanwRefs.Length;i++)
        {
            spawners[i] =GetNode<Spawner>(((NodePath)spanwRefs.GetValue(i)));
        }

        b = GetNode<Base>(baseRef);

        
        
    }

    void linkHUD()
    {
        EmitSignal("OnLifeUpdate",b.maxLife,b.life);
    }


    public void OnBaseLifeUpdate(int maxLife, int life)
    {
        EmitSignal("OnLifeUpdate",maxLife,life);

    }

    public void OnBaseDied()
    {
        EmitSignal("OnBaseDeath");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
