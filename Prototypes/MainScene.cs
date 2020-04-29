using Godot;
using System;

public class MainScene : Node2D
{

    [Signal]
    public delegate void UpdateMoney(int m);

    [Signal] 
    public delegate void SendBuildPermit(bool b);
    public int money;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    Godot.Collections.Array<PackedScene> maps;

    [Export]
    PackedScene sceneGameFinished;
    Map currentMap;

    [Export]
    int [] StartingMoney;

    public HUD hud;
    public ShaderHolder sHolder;

    int idx = 0;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        hud = GetNode<HUD>("HUD");
       // sHolder = GetNode<ShaderHolder>("Renderer");

        connectHud();

        loadMap();

        if(GetParentOrNull<Viewport>()!= null)
        {
            Viewport root = (Viewport)GetParent();

            root.PhysicsObjectPicking = true;
            root.GuiDisableInput = false;
        

        }

    }

   
    //*************money management**********//
    public void GetMoney(int m)
    {
        money += m;
        EmitSignal("UpdateMoney",money);

    }

    public void testCanBuild(int m)
    {   
        if(m<=money)
        EmitSignal("SendBuildPermit",true);

        else
        EmitSignal("SendBuildPermit",false);

    }

    //*********Scene Management **********//
    public void gameOverLose()
    {
        hud.OpenGameOverLose();

    }

    public void gameOverWin()
    {
        hud.OpenGameOverWin();

    }

    public void gameFinished()
    {
        GetTree().ChangeSceneTo(sceneGameFinished);
    }
     public void Reset()
    {
        GetTree().ReloadCurrentScene();
    }
    
    void loadMap()
    {
        if(idx<maps.Count)
        {
            Map temp = new Map();
            temp = (Map)maps[idx].Instance();

            connectMap(temp);
            AddChild(temp);
            MoveChild(temp,1);
            //sHolder.AddMap(temp);
            temp.linkHUD();

           GetMoney(StartingMoney[idx%StartingMoney.Length]);

           currentMap = temp;

        }
    }

    void nextMap()
    {
    if(idx +1 <maps.Count)
     {
         idx++;
         currentMap.QueueFree();
        loadMap();
     }

     else
        gameFinished();




    }

    public void goToMainMenu()
    {
        GetTree().ChangeScene("res://Tutorial.tscn");
    }

    //**********Connectors and stuff **********/

    void connectMap(Map m)
    {
        m.Connect(nameof(Map.OnLifeUpdate),hud,nameof(hud.UpdateLife));
        m.Connect(nameof(Map.OnUpdateWave),hud,nameof(hud.UpdateWave));
        m.Connect(nameof(Map.OnMoneyChange),this,nameof(GetMoney));
        m.Connect(nameof(Map.SendBuildRequest),this,nameof(testCanBuild));
        m.Connect(nameof(Map.OnBaseDeath),this,nameof(gameOverLose));
        m.Connect(nameof(Map.OnWin),this,nameof(gameOverWin));

        Connect(nameof(MainScene.SendBuildPermit),m,nameof(m.ReceiveBuildPermit));

    }

    void connectHud()
    {
        Connect(nameof(MainScene.UpdateMoney),hud,nameof(hud.UpdateMoney));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
