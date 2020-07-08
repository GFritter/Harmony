using Godot;
using System;

public class Builder : Area2D
{
    [Signal]
    public delegate void BuildTower(int id,Builder owner, int rot);

    [Signal]
    public delegate void RequestBuildPermit(int id);

    [Signal]
    public delegate void DeliverMoney(int id);

    [Export]
    public Color disabled;

   Tower tower;
    
    Label [] tCosts = new Label[3];

    Button [] tButtons = new Button[3];

    [Export]
    public int colorId;
    //vermelho =1 verde =2 azul = 3
    
    [Export]
    public Color cOn;

    [Export]
    public Color cOff;

    [Export]
    public Color canBuildCol;

    [Export]
    public Color cantBuildCol;

    [Export]
    public string key;

    AnimatedSprite placeHolderSprite;

    Control buildHud;
    bool mouseOn,hudON,canBuild = false;
    bool buildEnabled = true;

    Label keyShow;
    bool towerMode;

    int rot,towerId;

    bool waiting = false;

    //tower mode things, attributes I mean
    Control towerHud;
    Label ammoCount, Damage, DamageIdle, Speed, Reload;

   
    public override void _Ready()
    {
        buildHud = GetNode<Control>("BuildHud");
        buildHud.Hide();

        keyShow = GetNode<Label>("KeyShow");

        towerHud = GetNode<Control>("TowerHud");
        towerHud.Hide();
        placeHolderSprite = GetNode<AnimatedSprite>("PlaceHolderSprite");
        GetNode<AnimatedSprite>("Sprite").Modulate = cOff;

        tButtons[0] = GetNode<Button>("BuildHud/Button");
        tButtons[0].Hide();
        tButtons[1] = GetNode<Button>("BuildHud/Button2");
        tButtons[1].Hide();
        tButtons[2] = GetNode<Button>("BuildHud/Button3");
        tButtons[2].Hide();

        tCosts[0] = GetNode<Label>("BuildHud/Button/Custo");
        tCosts[1] = GetNode<Label>("BuildHud/Button2/Custo");
        tCosts[2] = GetNode<Label>("BuildHud/Button3/Custo");
        
        ammoCount = GetNode<Label>("TowerHud/Stats/Ammo/Stat");
        Damage = GetNode<Label>("TowerHud/Stats/Dano/Stat");
        DamageIdle = GetNode<Label>("TowerHud/Stats/DanoAuto/Stat");
        Speed = GetNode<Label>("TowerHud/Stats/Velocidade/Stat");
        Reload = GetNode<Label>("TowerHud/Stats/Recarrega/Stat");

        Godot.Collections.Array acts = InputMap.GetActionList(key);
        keyShow.Text = "";
        foreach(object o in acts)
        {
            InputEvent ie = (InputEvent)o;
            if(ie is InputEventKey)
            {
                InputEventKey iek = (InputEventKey)ie;
                string scString =OS.GetScancodeString(iek.Scancode);
               keyShow.Text += scString + "/";

            }
        }
       keyShow.Text = keyShow.Text.Remove(keyShow.Text.Length-1,1);
        

    }

    public void mouseEntered()
    {
       
        if(buildEnabled)
       { mouseOn = true;
        GetNode<AnimatedSprite>("Sprite").Modulate = cOn;
       }

    }

    public void mouseExited()
    {
        if(buildEnabled)
       {mouseOn = false;
       GetNode<AnimatedSprite>("Sprite").Modulate = cOff;
       }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton eventMouseButtonPressed && @event.IsActionPressed("mouse_click") &&  mouseOn)
        {
            if(!towerMode && buildEnabled)
            {
            if(!hudON)
            {
                buildHud.Show();
                placeHolderSprite.Show();

                keyShow.Hide();
            
            }
            
           else 
           {
               placeHolderSprite.Animation = "default";
               placeHolderSprite.Hide();
               buildHud.Hide();

               keyShow.Show();
           }
            }

           else if(towerMode && buildEnabled)
            {
                if(!hudON)
                {
                    towerHud.Show();
                    keyShow.Show();
                }

                if(hudON)
                {
                    towerHud.Hide();
                    keyShow.Hide();
                }

            }


           hudON = !hudON;
            
        }

        if(hudON)
        {
            if(@event.IsActionPressed("rotate"))
            {
                rot++;
                rot = rot%4;
                placeHolderSprite.Frame = rot;
            }
        }

        if(@event.IsActionPressed(key))
            {
               if(!towerMode && buildEnabled)
               {
                   if(hudON)
                    {
                    placeHolderSprite.Animation = "default";
                    placeHolderSprite.Hide();
                    buildHud.Hide();
                    }
                   
                    else
                    { 
                        buildHud.Show();
                        placeHolderSprite.Show();
                    }

                    hudON= !hudON; 
               }
            }

    }

    public void chooseTower1()
    {
        towerId = 0;
        placeHolderSprite.Animation = "Tower1";
        placeHolderSprite.Frame = rot;

        waiting = true;
        EmitSignal("RequestBuildPermit",towerId); 


    }

    public void chooseTower2()
    {
        towerId = 1;
        placeHolderSprite.Animation = "Tower2";
        placeHolderSprite.Frame = rot;
        waiting = true;
             EmitSignal("RequestBuildPermit",towerId); 
    }

    public void chooseTower3()
    {
        towerId =2;
        placeHolderSprite.Animation = "Tower3";
        placeHolderSprite.Frame = rot;
        waiting = true;
             EmitSignal("RequestBuildPermit",towerId); 
    }


    public void getPermit()
    {
        EmitSignal("RequestBuildPermit",towerId);
    }

    public void setBuildPermit(bool permit)
    {
        canBuild = permit;

        if(canBuild)
        placeHolderSprite.Modulate = canBuildCol;

        else
        placeHolderSprite.Modulate = cantBuildCol;

        waiting = false;

    }

    public void Build()
    {
        EmitSignal("RequestBuildPermit",towerId);
       
        if(canBuild)
        {EmitSignal("BuildTower",towerId,this,rot);
        buildHud.Hide();
        placeHolderSprite.Hide();
        }

          EmitSignal("RequestBuildPermit",towerId); 
    }

    public void Sell()
    {
        EmitSignal("DeliverMoney",tower.cost);
        towerMode = false;
        tower.QueueFree();

        towerHud.Hide();


    }

    public void LevelUp()
    {
        if(tower.canLevelUp())
            tower.levelUp();
    }

    public void BecomeTowerMode(Tower t)
    {
        keyShow.Hide();

        towerHud.GetNode<AnimatedSprite>("Button/AnimatedSprite").Frames = t.getSprite().Frames;

        towerMode = true;
        t.setKeycode(key);
        tower  =t;
        t.colorId = colorId;
        t.setRangeCol(cOff);
        t.rotat = rot;
        

        ammoCount.Text = t.GetAmmo().ToString();
        Damage.Text = t.GetDamage().ToString();
        DamageIdle.Text = t.GetMinDamage().ToString();
        Speed.Text = t.GetSpeed().ToString();
        Reload.Text = t.GetReloadSpeed().ToString();
      
    }

    public void GetTowers(Godot.Collections.Array<PackedScene> towers)
    {

        for(int i=0;i<towers.Count && i<3;i++)
        {
            Tower temp = (Tower)towers[i].Instance();

            tButtons[i].Show();
            tButtons[i].GetNode<AnimatedSprite>("AnimatedSprite").Frames= (temp.getSprite().Frames);
            tCosts[i].Text = temp.cost.ToString();
        }

    }

    public void EnableBuild()
    {
        buildEnabled = true;
        
        GetNode<AnimatedSprite>("Sprite").Modulate = cOff;

    }

    public void DisableBuild()
    {
         buildEnabled = false;
        
        GetNode<AnimatedSprite>("Sprite").Modulate = disabled;

        buildHud.Hide();
        placeHolderSprite.Hide();
        towerHud.Hide();
        keyShow.Hide();

    }



 //public override void _Process(float delta)
 //{
 //    
 //}
}
