using Godot;
using System;

public class Builder : Area2D
{
    [Signal]
    public delegate void BuildTower(int id,Builder owner, int rot);

    [Signal]
    public delegate void RequestBuildPermit(int id);

    [Export]
    public Color disabled;

   Tower tower;
    
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

    
    bool towerMode;

    int rot,towerId;
   
    public override void _Ready()
    {
        buildHud = GetNode<Control>("BuildHud");
        buildHud.Hide();
        placeHolderSprite = GetNode<AnimatedSprite>("PlaceHolderSprite");
        GetNode<AnimatedSprite>("Sprite").Modulate = cOff;
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
            
            }
            
           else 
           {
               placeHolderSprite.Animation = "default";
               placeHolderSprite.Hide();
               buildHud.Hide();
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

        EmitSignal("RequestBuildPermit",towerId); 


    }

    public void chooseTower2()
    {
        towerId = 1;
        placeHolderSprite.Animation = "Tower2";
        placeHolderSprite.Frame = rot;

             EmitSignal("RequestBuildPermit",towerId); 
    }

    public void chooseTower3()
    {
        towerId =2;
        placeHolderSprite.Animation = "Tower3";
        placeHolderSprite.Frame = rot;

             EmitSignal("RequestBuildPermit",towerId); 
    }


    public void setBuildPermit(bool permit)
    {
        canBuild = permit;

        if(canBuild)
        placeHolderSprite.Modulate = canBuildCol;

        else
        placeHolderSprite.Modulate = cantBuildCol;

    }

    public void Build()
    {
        if(canBuild)
        {EmitSignal("BuildTower",towerId,this,rot);
        buildHud.Hide();
        placeHolderSprite.Hide();
        }

          EmitSignal("RequestBuildPermit",towerId); 
    }

    public void BecomeTowerMode(Tower t)
    {
        towerMode = true;
        t.setKeycode(key);
        tower  =t;
        t.colorId = colorId;
        t.setRangeCol(cOff);
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

    }

 //public override void _Process(float delta)
 //{
 //    
 //}
}
