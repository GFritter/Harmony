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
    public Godot.Collections.Array<NodePath> spanwRefs;
    [Export]
    public Godot.Collections.Array<NodePath> buildRefs;

    [Export]
    public Godot.Collections.Array<PackedScene> towerRefs;

    [Export]
    public Godot.Collections.Array<Color> colors;

    public Spawner[] spawners;

    [Signal]
    public delegate void OnLifeUpdate(int maxLife,int life);

    [Signal]
    public delegate void OnUpdateWave(int w);

    [Signal]
    public delegate void UpdateMaxWaves(int mw);

    [Signal]
    public delegate void OnMoneyChange(int m);

    [Signal]
    public delegate void OnBaseDeath();

    [Signal]
    public delegate void OnWin();

    [Signal]
    public delegate void StartWaves();

    [Signal]
    public delegate void EnableBuilding();

    [Signal]
    public delegate void SendBuildRequest(int cost);

    [Signal]
    public delegate void DeliverBuildPermit(bool b);

   

    [Export]
    bool waveClear;
    int clearCounter;

    [Export]
    public float[] waveTimes;

    int currentWave;

    Timer waveTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        spawners = new Spawner[spanwRefs.Count];

        for(int i=0;i<spanwRefs.Count;i++)
        {   
           

            spawners[i] =GetNode<Spawner>((spanwRefs[i]));
        }

        connectSpawners();
        connectBuilders();

        b = GetNode<Base>(baseRef);
        waveClear = true;
        waveTimer = GetNode<Timer>("WaveTimer");

        waveTimer.WaitTime = waveTimes[currentWave];
    
        
        
    }

//*******Functions to work with the MAIN/HUD*******
    public void linkHUD()
    {
        EmitSignal("OnLifeUpdate",b.maxLife,b.life);
        EmitSignal("UpdateMaxWaves",waveTimes.Length);
    }
    public void OnBaseLifeUpdate(int maxLife, int life)
    {
        EmitSignal("OnLifeUpdate",maxLife,life);

    }
    public void OnBaseDied()
    {
        EmitSignal("OnBaseDeath");
    }

    public void EnemyDied(int m)
    {
        EmitSignal("OnMoneyChange",m);
    }

    public void Clear()
    {
        
      Godot.Collections.Array children = GetChildren();

      for(int i =0;i<children.Count;i++)
      {
         Node temp = new Node();
         temp =(Node) children[i];
         temp.QueueFree();
      }  
        
        QueueFree();
    }

    //****Functions to work with the waves**** */

    public void startWaveTimer()
    {
        if(waveClear && currentWave < waveTimes.Length)
            waveTimer.Start();

    }

    public void startWaves()
    {
        waveClear = false;
        clearCounter = 0;
       
        EmitSignal("StartWaves");
        EmitSignal("OnUpdateWave",currentWave+1);

    }

    void getWaveClear()
    {
        clearCounter++;
        
        if(clearCounter == spanwRefs.Count)
        {
            clearCounter=0;
            waveClear = true;
            EmitSignal("EnableBuilding");
            

           
            currentWave++;
            if(currentWave<waveTimes.Length)
            {
                waveTimer.WaitTime = waveTimes[currentWave];
           
                
            }

            else
            {
                EmitSignal("OnWin");
               
            }
            
        }
    }

    //********Functions to work with the Builders******* */

    public void SendBuildPermit(int id)
    {
        //this is for testing purposes only, in reality this sends the request to the main to calculate the price and if it can build
        //ReceiveBuildPermit(true);
        Tower temp = (Tower)towerRefs[id%towerRefs.Count].Instance();
        int cost = temp.cost;
        EmitSignal("SendBuildRequest",cost);

    }

    //gets the build permit from main and passes down to the builder
    public void ReceiveBuildPermit(bool b)
    {
        EmitSignal("DeliverBuildPermit",b);

    }

    public void Build(int id, Builder owner, int rot)
    {
        Tower tInstance = new Tower();
        tInstance = (Tower)towerRefs[id].Instance();

        AddChild(tInstance);

        tInstance.Position = owner.Position;
        tInstance.Scale =(owner.Scale/2);

         tInstance.SetRotat(rot);

        owner.BecomeTowerMode(tInstance);

        EmitSignal("OnMoneyChange",-(tInstance.cost));

    }


    public void Sell(int m)
    {
        EmitSignal("OnMoneyChange",m);
    }
//*****Connecting signals and mostly initializing Stuff****** */
    void connectSpawners()
    {

        for(int i= 0;i<spanwRefs.Count;i++)
        { 
            
            Spawner temp =GetNode<Spawner>((NodePath)spanwRefs[i]);
           
            Connect(nameof(StartWaves),temp,nameof(temp.StartWave));
            temp.Connect(nameof(Spawner.WaveClear),this,nameof(getWaveClear));

            temp.GetColors(colors);
        }
    }

    void connectBuilders()
    {
        for(int i =0;i<buildRefs.Count;i++)
        {
            Builder temp = GetNode<Builder>((NodePath)buildRefs[i]);

            Connect(nameof(StartWaves),temp, nameof(temp.DisableBuild));
            Connect(nameof(EnableBuilding),temp,nameof(temp.EnableBuild));

            Connect(nameof(DeliverBuildPermit),temp,nameof(temp.setBuildPermit));

            temp.Connect(nameof(Builder.BuildTower),this,nameof(Build));

            temp.Connect(nameof(Builder.RequestBuildPermit),this,nameof(SendBuildPermit));

            temp.Connect(nameof(Builder.DeliverMoney),this,nameof(Sell));
            temp.GetTowers(towerRefs);
        }
    }
  

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
