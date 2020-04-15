using Godot;
using System;

public class ShaderHolder : ViewportContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    ShaderMaterial mat;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mat = (ShaderMaterial)Material;
        
    }

    public void changeR(float r)
    {
        mat.SetShaderParam("r_value",r);
    }

    public void changeG(float g)
    {
        mat.SetShaderParam("g_value",g);
    }

    public void changeB(float b)
    {
        mat.SetShaderParam("b_value",b);
    }

    public void changeSaturation(float s)
    {
        mat.SetShaderParam("saturation",s);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
