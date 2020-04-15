using Godot;
using System;

public class Tower : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
  

[Export]

public string keycode;

AnimatedSprite sprite;

AnimatedSprite ammoBar;

//da pra botar um array de floats tbm pro tempo de refresh e reload por nivel

[Export]
public int lvl = 0;

[Export]
public float[] damage= new float[5] {1,2,3,4,5};

[Export]
public float[]minDamage = new float[5] {0.5f,1,1,2,2};

[Export]
int[] maxAmmo = new int[5] {4,5,6,7,8};
int currentAmmo;

public bool canShoot;
public bool refreshing, reloading;
private AudioStreamPlayer sound;

private ProgressBar bar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sound = GetNode<AudioStreamPlayer>("Sound");
        GetNode<Range>("Range").setKeycode(keycode);
        currentAmmo=maxAmmo[lvl];
        canShoot= true;

        bar = GetNode<ProgressBar>("ProgressBar");
        bar.Hide();

        sprite = GetNode<AnimatedSprite>("TowerBase");
        ammoBar = GetNode<AnimatedSprite>("AmmoBar");
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
        sprite.Animation = "default";
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

    public void Fire(Enemy e)
    {
        e.TakeDamage(damage[lvl]);
        currentAmmo--;
        sound.Play();
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

    public void IdleFire(Enemy e)
    {
        
         e.TakeDamage(minDamage[lvl]);
         currentAmmo--;
        sound.Play();
        canShoot = false;
        if(currentAmmo>0)
        setupRefresh();

        else
       setupReload();
        
    }

    void setupRefresh()
    {
        GetNode<Timer>("ShootTimer").Start();
        refreshing = true;
        bar.MaxValue =  GetNode<Timer>("ShootTimer").WaitTime;
        sprite.Animation = ("cooldown");
    }

    void setupReload()
    {
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
}
