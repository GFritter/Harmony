using Godot;
using System;

public class Spawner : Node2D
{

    [Signal]
    public delegate void WaveClear();

    [Export]
    public string[] enemyRefs;

      PackedScene[] enemies;
    
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public int[] numEnemies;

    int enemyCounter =0;

    [Export]
    public int numWaves;

    int currentWave =0;

    [Export]
    string pathFile;  
    File inFile;
    int[][] matrix;

    bool waitingForClear;
    int numChildDefault;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       readFile();
       readEnemies();
       numChildDefault = GetChildCount();
    }

    public void Spawn()
    {
         if(waitingForClear)
        {
            if(GetChildCount()==numChildDefault)
            {
                GD.Print("terminei minha wave");
            GetNode<Timer>("Timer").Stop();
             EmitSignal("WaveClear");
            
             currentWave++;
             enemyCounter = 0;
             waitingForClear = false;

             }
         }

       else
       {
            if(matrix[currentWave][enemyCounter]!=-1)
        {
             Enemy enemyInstance = new Enemy();
        enemyInstance =(Enemy) enemies[matrix[currentWave][enemyCounter]].Instance();
       
        AddChild(enemyInstance);
        enemyInstance.Position = this.Position;

        }

         enemyCounter++;

        if(enemyCounter>=matrix[currentWave].Length)
      {
        GD.Print("nao tenho mais nada pra spawnar");
       
        GD.Print("tenho " + GetChildCount() + "filhos");
        waitingForClear = true;
      }
   
       }
    
    }

    public void setTimer(float t)
    {
        GetNode<Timer>("Timer").WaitTime = t;
    }

    public void readFile()
    {
        inFile = new File();
        if(inFile.FileExists(pathFile))
        {
            
        inFile.Open(pathFile,File.ModeFlags.Read);
        int x,y;
        string buffer;
        string[] buff;

        buffer = inFile.GetLine();
        x = buffer.ToInt();
        buffer = inFile.GetLine();
        y= buffer.ToInt();

        GD.Print(x);
        GD.Print(y);

        matrix = new int[x][];
        for(int i=0;i<x;i++)
        {
            matrix[i] = new int[y];
        }

        for(int i=0;i<x;i++)
        {
             buff = inFile.GetCsvLine();
            for(int j=0;j<y;j++)
            {
                matrix[i][j] = buff[j].ToInt();
            }

        }

        inFile.Close();

        }

    }

    public void readEnemies()
    {
        enemies = new PackedScene[enemyRefs.Length];
        for(int i=0;i<enemyRefs.Length;i++)
        {
            PackedScene tempE =(PackedScene) GD.Load(enemyRefs[i]);
            enemies[i] =tempE;

        }
    }
    public void StartWave()
    {
        GD.Print("Comecando os trabalhos");
        
         GetNode<Timer>("Timer").Start();

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//public override void _Process(float delta)  {
 //   
//}
}
