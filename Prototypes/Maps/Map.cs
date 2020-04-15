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
    public delegate void OnUpdateWave(int w);

    [Signal]
    public delegate void OnBaseDeath();

    [Signal]
    public delegate void StartWaves();

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

        spawners = new Spawner[spanwRefs.Length];

        for(int i=0;i<spanwRefs.Length;i++)
        {
            spawners[i] =GetNode<Spawner>(((NodePath)spanwRefs.GetValue(i)));
        }

        b = GetNode<Base>(baseRef);
        waveTimer = GetNode<Timer>("WaveTimer");

        waveTimer.WaitTime = waveTimes[currentWave];
        waveTimer.Start();

        
        
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

    public void startWaves()
    {
        waveClear = false;
        clearCounter = 0;
        GD.Print("Ta na hora de startar os spawns");
        EmitSignal("StartWaves");
        EmitSignal("OnUpdateWave",currentWave+1);

    }

    void getWaveClear()
    {
        clearCounter++;
        
        if(clearCounter == spanwRefs.Length+1)
        {
            clearCounter=0;
            waveClear = true;
            

            GD.Print("terminamos a wave "+currentWave +"de "+waveTimes.Length);
            currentWave++;
            if(currentWave<waveTimes.Length)
            {
                waveTimer.WaitTime = waveTimes[currentWave];
                waveTimer.Start();
            }

            else
            {
                GD.Print("PARABENS VOCE GANHOU SEU IDIOTA");
            }
            
        }
    }
  

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
