using Godot;
using System;

public class Tower : Area2D
{
    int rotat;
    int experience;

[Export]
public Godot.Collections.Array<Vector2> RangePositions;

[Export]
public Godot.Collections.Array<Vector2> RangeSpritePositions;

[Export]
public Godot.Collections.Array<String> SoundsRef;

public Godot.Collections.Array<AudioStream> Sounds;


[Export]
public float[] RangeRotations;

[Export]
public Godot.Collections.Array<Shape2D> RangeShapes;

[Export]
public string keycode;

[Export]
public int cost;

AnimatedSprite sprite;

AnimatedSprite ammoBar;

//da pra botar um array de floats tbm pro tempo de refresh e reload por nivel

[Export]
public int lvl = 0;

[Export]
public int [] maxXP = new int[5] {100,200,300,400,500};

[Export]
public float[] damage= new float[5] {1,2,3,4,5};

[Export]
public float[]minDamage = new float[5] {0.5f,1,1,2,2};

[Export]
public float[] refreshTime =  new float[5] {0.4f,0.4f,0.4f,0.4f,0.2f};
[Export]
public float[] reloadTime =  new float[5] {0.4f,0.4f,0.4f,0.4f,0.2f};

[Export]
int[] maxAmmo = new int[5] {4,5,6,7,8};
int currentAmmo;

[Export]
public Color color;
[Export]
public int colorId;

public bool canShoot;
public bool refreshing, reloading;
private AudioStreamPlayer sound;

Range range;
int soundIdx;

private ProgressBar bar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        soundIdx = 0;
        sound = GetNode<AudioStreamPlayer>("Sound");
        GetNode<Range>("Range").setKeycode(keycode);
        currentAmmo=maxAmmo[lvl];
        canShoot= true;

        bar = GetNode<ProgressBar>("ProgressBar");
        bar.Hide();

        sprite = GetNode<AnimatedSprite>("TowerBase");
        ammoBar = GetNode<AnimatedSprite>("AmmoBar");


        range = GetNode<Range>("Range");
        Sounds = new Godot.Collections.Array<AudioStream>();
        loadSounds();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
  {
      manageBar();
      sprite.Play();

    
  }

    void Refresh()
    {
        canShoot = true;
        refreshing = false;
        bar.Hide();
        //sprite.Animation = "default";
        ammoBar.Animation = currentAmmo.ToString();
    }

    void Reload()
    {
        currentAmmo = maxAmmo[lvl];
        ammoBar.Animation = currentAmmo.ToString();
        canShoot =true;
        reloading = false;
        bar.Hide();
    }

    public void Fire(Godot.Collections.Array<Enemy> enemiesInRange)
    {
        //GD.Print("VOU ATIRAR");
         for(int i =0;i<enemiesInRange.Count;++i)
       { 
           if(IsInstanceValid(enemiesInRange[i]) )
            {   
                Enemy e =enemiesInRange[i];
                 e.TakeDamage(damage[lvl]*checkDamage(e));

            }
            
        

        }
      

       currentAmmo--;
        sound.Stream = Sounds[soundIdx%Sounds.Count];
        sound.Play();
        soundIdx =  (soundIdx +1)%Sounds.Count;

        canShoot = false;
        if(currentAmmo>0)
        setupRefresh();

        else
       setupReload();
    }

    public void Misfire()
    {
        currentAmmo--;
        canShoot = false;
        if(currentAmmo>0)
        setupRefresh();

        else
        setupReload();
    }

    public void IdleFire(Godot.Collections.Array<Enemy> enemiesInRange)
    {
        for(int i =0;i<enemiesInRange.Count;++i)
       { 
           if(IsInstanceValid(enemiesInRange[i]) )
            {   
                Enemy e =enemiesInRange[i];
                 e.TakeDamage(minDamage[lvl]*checkDamage(e));

            }
            
        

        }
      
        currentAmmo--;
        sound.Stream = Sounds[soundIdx%Sounds.Count];
        sound.Play();
        soundIdx =  (soundIdx+1)%Sounds.Count;
        canShoot = false;

        soundIdx++;

        if(currentAmmo>0)
        setupRefresh();

        else
       setupReload();
        
        
        
    }

    void setupRefresh()
    {
        GetNode<Timer>("ShootTimer").WaitTime = refreshTime[lvl];
        GetNode<Timer>("ShootTimer").Start();
        refreshing = true;
        bar.MaxValue =  GetNode<Timer>("ShootTimer").WaitTime;
        //sprite.Animation = ("cooldown");
    }

    void setupReload()
    {
          GetNode<Timer>("RechargeTimer").WaitTime=(reloadTime[lvl]);
          GetNode<Timer>("RechargeTimer").Start();
          reloading =true;
          bar.MaxValue = GetNode<Timer>("RechargeTimer").WaitTime;

    }

    void manageBar()
    {
        if(reloading)
        {
            bar.Show();
            bar.Value = GetNode<Timer>("RechargeTimer").WaitTime -GetNode<Timer>("RechargeTimer").TimeLeft; 

        }

        else if(refreshing)
        {
             bar.Show();
            bar.Value = GetNode<Timer>("ShootTimer").WaitTime -GetNode<Timer>("ShootTimer").TimeLeft; 

        }

       
    }

    float checkDamage(Enemy e)
    {
        switch(colorId)
        {
            case 1: 
                if(e.colorId==1)
                {
                    return 1.0f;
                }
                else if(e.colorId==2)
                {
                    return 1.25f; 
                }
                else if(e.colorId==3)
                {
                 return 0.75f;
                }
                break;

             case 2: 
                if(e.colorId==1)
                {
                    return 0.75f;
                }
                else if(e.colorId==2)
                {
                    return 1.0f; 
                }
                else if(e.colorId==3)
                {
                    return 1.25f;
                }
                 break;

             case 3: 
                 if(e.colorId==1)
                {
                    return 1.25f;
                }
                 else if(e.colorId==2)
                {
                  return 0.75f; 
                }
                 else if(e.colorId==3)
                {
                   return 1.0f;
                }
                 break;
        }

        return 1.0f;

    }

    public void setRangeCol(Color c)
    {
        range.setColor(c);
    }

    public void SetRotat(int rot)
    {

        sprite.Animation = "Pos"+rot;
        range.Position = (RangePositions[rot]);
        range.RotationDegrees = RangeRotations[rot];
        range.GetNode<CollisionShape2D>("CollisionShape2D").Shape = RangeShapes[rot];
        range.GetNode<AnimatedSprite>("RangeSprite").Position = (RangeSpritePositions[rot]);

        if(rot%2==1)
            {
                range.GetNode<AnimatedSprite>("RangeSprite").FlipH = true;
            }

    }

    public void setKeycode(string k)
    {
        keycode = k;
        range.setKeycode(k);
    }

    public void getXp(int xp)
    {
        experience+= xp;

        //in case we dont want to store xp
        //if(experience> maxXP[lvl])
            //experience = maxXP[lvl];
    }

    public void levelUp()
    {
        experience -= maxXP[lvl];

        //do something cool

        lvl++;

    }

    public bool canLevelUp()
    {
        return (experience>=maxXP[lvl] && lvl<5);
    }

    //get stats used to link the tower hud

    public int GetAmmo()
    {
        return maxAmmo[lvl];

    }

    public float GetDamage()
    {
        return damage[lvl];
    }

    public float GetMinDamage()
    {
        return minDamage[lvl];
    }

    public float GetSpeed()
    {
        return refreshTime[lvl];
    }

    public float GetReloadSpeed()
    {
        return reloadTime[lvl];
    }

    public AnimatedSprite getSprite()
    {
        return GetNode<AnimatedSprite>("TowerBase");
    }

    void loadSounds()
    {
        for(int i=0;i<SoundsRef.Count;i++)
        {
           AudioStream streamTemp =(AudioStream) GD.Load(SoundsRef[i]) ;
       

        Sounds.Add(streamTemp);
        }

    }

}

