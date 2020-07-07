using Godot;
using System;

public class ShaderHolder : AnimatedSprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    ShaderMaterial mat;
    float initR,initG,initB,initS;

    [Export]

    public float step;
    public Quat modsOnDamage;
    public Quat modsOnMoney;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mat = (ShaderMaterial)this.Material;

        initR = getR();
        initG = getG();
        initB = getB();
        initS= getS();
     
    }

    public void changeR(float r)
    {
        mat.SetShaderParam("r_value",r);
    }

    public void addR(float r)
    {
        float inR =(float) mat.GetShaderParam("r_value");
        mat.SetShaderParam("r_value",inR+r);
    }

    public void changeG(float g)
    {
        mat.SetShaderParam("g_value",g);
    }

    public void addG(float g)
    {
        float inG =(float) mat.GetShaderParam("g_value");
        mat.SetShaderParam("g_value",inG+g);
    }

    public void changeB(float b)
    {
        mat.SetShaderParam("b_value",b);
    }
    public void addB(float b)
    {
        float inB =(float) mat.GetShaderParam("b_value");
        mat.SetShaderParam("b_value",inB+b);
    }

    public void changeSaturation(float s)
    {
        mat.SetShaderParam("saturation",s);
    }

    public void addSaturation(float s)
    {
        float inS =(float) mat.GetShaderParam("saturation");
        mat.SetShaderParam("saturation",inS+s);
    }

    public void treatDamage(int maxLife, int life)
    {
        float portion  = (float)(life-maxLife)/(float)maxLife;
       
        
        addSaturation(portion*modsOnDamage.w);
        addR(-portion*modsOnDamage.x);
        addG(-portion*modsOnDamage.y);
        addB(-portion*modsOnDamage.z);
       

    }

    public void treatMoney(int money)
    {
        float portion = money/100.0f;

        addSaturation(portion* modsOnMoney.w);

        addR(portion*modsOnMoney.x);
        addG(portion*modsOnMoney.y);
        addB(portion*modsOnMoney.z);
 
    }


public void scaleBack()
{
    changeSaturation(Mathf.Lerp(getS(),initS,step));
    changeR(Mathf.Lerp(getR(),initR,step));
    changeG(Mathf.Lerp(getG(),initG,step));
    changeB(Mathf.Lerp(getB(),initB,step));
}

public void reset()
{
    changeSaturation(initS);
    changeR(initR);
    changeG(initG);
    changeB(initB);
}

public float getR()
{
    return (float)mat.GetShaderParam("r_value");
}
public float getG()
{
    return (float)mat.GetShaderParam("g_value");
}public float getB()
{
    return (float)mat.GetShaderParam("b_value");
}public float getS()
{
    return (float)mat.GetShaderParam("saturation");
}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      scaleBack();
  }
}
