using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ToDo: add ui, colors, inspector vars, etc

/// <summary>
/// An effect on an objects physics
/// </summary>
public abstract class PhysicsEffect
{

    public abstract void ApplyEffect(InteractableObject target);
}


public class ChangeMass : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.black;
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

    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = new Color(1, 0, 1, 1);
    }
}

public class ChangeGravity : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.grey;
    }
}

public class ChangeLayer : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.cyan;
    }
}

public class ToggleKinematic : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.green;
    }
}

public class ApplyForce : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = new Color(0, 1, 1, 1);
        Debug.Log("boom");
    }
}

public class UseMagnet : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.red;
    }
}

public class ApplyTorque : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = new Color(1, 1, 0, 1);
    }
}


