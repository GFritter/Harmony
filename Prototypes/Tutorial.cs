using Godot;
using System;

public class Tutorial : Control
{
    [Export]
    public PackedScene mainScene;
    public PackedScene prevScene;
    public PackedScene nextScene;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }


    void LoadMainScene()
    {
        GetTree().ChangeSceneTo(mainScene);
    }

    void LoadOptionsScene()
    {
        GetTree().ChangeScene("res://Options/options.tscn"); 
    }

    void LoadComoJogarScene()
    {
        GetTree().ChangeScene("res://Tutorial/Tutorial.tscn");
    }

    void LoadPrevScene()
    {
        GetTree().ChangeSceneTo(prevScene);
    }
    void LoadNextScene()
    {
        GetTree().ChangeSceneTo(nextScene);
    }
    void Quit()
    {
        GetTree().Quit();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
