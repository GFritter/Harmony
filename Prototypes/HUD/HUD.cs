using Godot;
using System;

public class HUD : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    ProgressBar lifeBar;
    Label waveCounter,moneyCounter;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lifeBar = GetNode<ProgressBar>("LifeBar");
        waveCounter = GetNode<Label>("Panel/Wave/WaveCounter");
        moneyCounter= GetNode<Label>("Panel/Money/MoneyCounter");
    }

    void UpdateLife(int maxLife,int life)
    {
       lifeBar.MaxValue = maxLife;
       lifeBar.Value = life;
    }

    void UpdateWave(int w)
    {
        waveCounter.Text = w.ToString();
    }

    void UpdateMoney(int m)
    {
        moneyCounter.Text = m.ToString();
    }

    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
