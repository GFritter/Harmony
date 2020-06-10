using Godot;
using System;

public class options : Control
{
    bool fullscreen = false;
    private Vector2 WindowSize = new Vector2(1024, 640);
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    void ScreenToggle()
    {
        fullscreen = !fullscreen;
        OS.SetWindowFullscreen(fullscreen);
    }

    void BackToMenu()
    {
        GetTree().ChangeScene("res://MenuPrincipal/Menu.tscn");
    }
    void Res1()
    {
        WindowSize = new Vector2(1024, 768);
        OS.SetWindowSize(WindowSize);
    }
    void Res2()
    {
        WindowSize = new Vector2(1024, 640);
        OS.SetWindowSize(WindowSize);
    }
    void Res3()
    {
        WindowSize = new Vector2(2560, 1080);
        OS.SetWindowSize(WindowSize);
    }
    void Res4()
    {
        GetTree().ChangeScene("res://Tutorial.tscn");
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
