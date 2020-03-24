using Godot;
using System;

public class RythymBar : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public PackedScene rythmBox;

    [Export]
    public string keycode;
    
    [Signal]
    public delegate void Fire();

    [Export]
    public int speed;

    [Export]
    public float waitTime;
    
        private Position2D startPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        startPosition = GetNode<Position2D>("StartPosition");
        GetNode<Timer>("Timer").Start();
        GetNode<Timer>("Timer").WaitTime=waitTime;

        Collisor col = GetNode<Collisor>("Collisor");
        col.setKeycode(keycode);

    }


public void OnTimerTimeout()
{
    RythymBox boxInstance = new RythymBox();
     boxInstance =  (RythymBox)rythmBox.Instance();
    AddChild(boxInstance);

    boxInstance.Position = startPosition.Position; 
    boxInstance.setSpeed(speed);
   
}

public void onCollisorWrong()
{
     GetNode<Timer>("Timer").Start();
}


public void onHit()
{
    EmitSignal("Fire");
}


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 
}
