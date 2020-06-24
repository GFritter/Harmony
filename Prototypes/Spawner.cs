using Godot;
using System;

public class Spawner : Path2D
{

    [Signal]
    public delegate void WaveClear();

    [Export]
    public string[] enemyRefs;

      PackedScene[] enemies;

    int enemyCounter =0;

   
    public int numWaves;

    int currentWave =0;

    [Export]
    string pathFileWave;  

    [Export]
    string pathFileColor;
    [Export]
    string pathFileSpeed;
    [Export]
    string pathFileHealth;

    File inFile;
    int[][] matrix;
    float [][] matrixSpeed;
    float [][] matrixHealth;
    int [][] matrixColor;

    public Godot.Collections.Array<Color> colors;

    bool waitingForClear;
    int numChildDefault;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       readFileWave();
       readFileColor();
       readFileHealth();
       readFileSpeed();
       readEnemies();
       numChildDefault = GetChildCount();
    }

    public void Spawn()
    {
         if(waitingForClear)
        {
            if(GetChildCount()==numChildDefault)
            {
             
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

        enemyInstance.speed += matrixSpeed[currentWave][enemyCounter];
        enemyInstance.setMaxHP(enemyInstance.maxLife+matrixHealth[currentWave][enemyCounter]);
        enemyInstance.colorId = matrixColor[currentWave][enemyCounter];
        enemyInstance.SetAuraColor(colors[enemyInstance.colorId]);
       
        AddChild(enemyInstance);
        connectEnemyToMap(enemyInstance);
        enemyInstance.Position = this.Position;

        }

         enemyCounter++;

        if(enemyCounter>=matrix[currentWave].Length)
      {
       
       
       
        waitingForClear = true;
      }
   
       }
    
    }

    public void connectEnemyToMap(Enemy e)
    {
        if(GetParentOrNull<Map>()!= null)
        {
            Map dad = GetParent<Map>();

            e.Connect(nameof(Enemy.OnDeath),dad,nameof(Map.EnemyDied));

        }
    }

    public void setTimer(float t)
    {
        GetNode<Timer>("Timer").WaitTime = t;
    }

    public void readFileWave()
    {
        inFile = new File();
        if(inFile.FileExists(pathFileWave))
        {
            
        inFile.Open(pathFileWave,File.ModeFlags.Read);
        int x,y;
        string buffer;
        string[] buff;

        buffer = inFile.GetLine();
        x = buffer.ToInt();
        buffer = inFile.GetLine();
        y= buffer.ToInt();

        numWaves = x;

       
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

    
    public void readFileSpeed()
    {
        inFile = new File();
        if(inFile.FileExists(pathFileSpeed))
        {
            
        inFile.Open(pathFileSpeed,File.ModeFlags.Read);
        int x,y;
        string buffer;
        string[] buff;

        buffer = inFile.GetLine();
        x = buffer.ToInt();
        buffer = inFile.GetLine();
        y= buffer.ToInt();


        matrixSpeed = new float[x][];
        for(int i=0;i<x;i++)
        {
            matrixSpeed[i] = new float[y];
        }

        for(int i=0;i<x;i++)
        {
             buff = inFile.GetCsvLine();
            for(int j=0;j<y;j++)
            {
                matrixSpeed[i][j] = buff[j].ToFloat();
            }

        }

        inFile.Close();

        }

    }
      public void readFileHealth()
    {
        inFile = new File();
        if(inFile.FileExists(pathFileHealth))
        {
            
        inFile.Open(pathFileHealth,File.ModeFlags.Read);
        int x,y;
        string buffer;
        string[] buff;

        buffer = inFile.GetLine();
        x = buffer.ToInt();
        buffer = inFile.GetLine();
        y= buffer.ToInt();


        matrixHealth = new float[x][];
        for(int i=0;i<x;i++)
        {
            matrixHealth[i] = new float[y];
        }

        for(int i=0;i<x;i++)
        {
             buff = inFile.GetCsvLine();
            for(int j=0;j<y;j++)
            {
                matrixHealth[i][j] = buff[j].ToFloat();
            }

        }

        inFile.Close();

        }

    }


    
    public void readFileColor()
    {
        inFile = new File();
        if(inFile.FileExists(pathFileColor))
        {
            
        inFile.Open(pathFileColor,File.ModeFlags.Read);
        int x,y;
        string buffer;
        string[] buff;

        buffer = inFile.GetLine();
        x = buffer.ToInt();
        buffer = inFile.GetLine();
        y= buffer.ToInt();

       

       

        matrixColor = new int[x][];
        for(int i=0;i<x;i++)
        {
            matrixColor[i] = new int[y];
        }

        for(int i=0;i<x;i++)
        {
             buff = inFile.GetCsvLine();
            for(int j=0;j<y;j++)
            {
                matrixColor[i][j] = buff[j].ToInt();
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
        if(currentWave<numWaves)
         GetNode<Timer>("Timer").Start();

         else
        {
            EmitSignal("WaveClear");
        }

    }

 
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//public override void _Process(float delta)  {
 //   
//}
}
