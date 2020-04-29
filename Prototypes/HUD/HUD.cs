using Godot;
using System;

public class HUD : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Signal]
    public delegate void NextScene();
    [Signal]
    public delegate void RestartScene();
    [Signal]
    public delegate void goToMainMenu();

    ProgressBar lifeBar;
    Label waveCounter,moneyCounter,waveMaxCounter;

    Panel popups,gameOverWin,gameOverLose;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lifeBar = GetNode<ProgressBar>("LifeBar");
        waveCounter = GetNode<Label>("Panel/Wave/WaveCounter");
        waveMaxCounter= GetNode<Label>("Panel/Wave/WaveMax");
        moneyCounter= GetNode<Label>("Panel/Money/MoneyCounter");
        popups = GetNode<Panel>("PopUps");
        gameOverLose = GetNode<Panel>("PopUps/GameOverWin");
        gameOverWin = GetNode<Panel>("PopUps/GameOverWin");
   
    }

    public void UpdateLife(int maxLife,int life)
    {
       lifeBar.MaxValue = maxLife;
       lifeBar.Value = life;
    }

   public void UpdateWave(int w)
    {
        waveCounter.Text = w.ToString();
    }

    public void UpdateMaxWave(int w)
    {
        waveMaxCounter.Text = "/"+w;
    }

    public void UpdateMoney(int m)
    {
        moneyCounter.Text = m.ToString();
    }

    public void OpenGameOverWin()
    {
        
        popups.Show();
        gameOverWin.Show();
    }

    public void OpenGameOverLose()
    {
        popups.Show();
        gameOverLose.Show();

    }

    public void closePopup()
    {
        gameOverLose.Hide();
        gameOverWin.Hide();
        popups.Hide();
    }

    public void BackToMenu()
    {
        closePopup();
        EmitSignal("goToMainMenu");

    }

    public void goNextScene()
    {
        closePopup();
        EmitSignal("NextScene");
    }

    public void goRestartScene()
    {
        closePopup();
        EmitSignal("RestartScene");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
