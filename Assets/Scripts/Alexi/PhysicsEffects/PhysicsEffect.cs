using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ToDo: add ui, colors, inspector vars, etc

/// <summary>
/// An effect on an objects physics
/// </summary>
public abstract class PhysicsEffect
{

    public abstract void ApplyEffect();
}


public class ChangeMass : PhysicsEffect
{
    public override void ApplyEffect()
    {
        Debug.Log(this.GetType().ToString());
    }
}

public class ChangeMaterial : PhysicsEffect
{
    PhysicMaterial origMat, newMat;

    private ChangeMaterial() { }
    public ChangeMaterial(PhysicMaterial _newMat)
    {
        newMat = _newMat;
    }

    public override void ApplyEffect()
    {
        Debug.Log(this.GetType().ToString());
    }
}

public class ChangeGravity : PhysicsEffect
{
    public override void ApplyEffect()
    {
        Debug.Log(this.GetType().ToString());
    }
}

public class ChangeLayer : PhysicsEffect
{
    public override void ApplyEffect()
    {
        Debug.Log(this.GetType().ToString());
    }
}

public class ToggleKinematic : PhysicsEffect
{
    public override void ApplyEffect()
    {
        Debug.Log(this.GetType().ToString());
    }
}

public class ApplyForce : PhysicsEffect
{
    public override void ApplyEffect()
    {
        Debug.Log(this.GetType().ToString());
    }
}

public class UseMagnet : PhysicsEffect
{
    public override void ApplyEffect()
    {
        Debug.Log(this.GetType().ToString());
    }
}

public class ApplyTorque : PhysicsEffect
{
    public override void ApplyEffect()
    {
        Debug.Log(this.GetType().ToString());
    }
}


