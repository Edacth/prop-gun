using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ToDo: add ui, colors, inspector vars, etc

/// <summary>
/// An effect on an objects physics
/// </summary>
public abstract class PhysicsEffect
{
    /// <summary>
    /// apply this physics effect
    /// </summary>
    /// 
    /// <param name="target">
    /// the target of this effect
    /// </param>
    public abstract void ApplyEffect(InteractableObject target);

    /// <summary>
    /// open effect editor gui
    /// </summary>
    public void EnterEditMode()
    {
        Debug.Log("enter " + GetType().ToString() + " editor");

        // EffectEditor.Open(PhysicsEffectType this);
    }

    /// <summary>
    /// update effect editor gui
    /// </summary>
    public abstract void RunEditMode();

    /// <summary>
    /// close effect editor gui
    /// </summary>
    public void ExitEditMode()
    {
        Debug.Log("exit " + GetType().ToString() + " editor");

        // EffectEditor.Close();
    }
}


public class ChangeMass : PhysicsEffect
{
    float min, max, current;
    public ChangeMass(float _min, float _max)
    {
        min = current = _min;
        max = _max;
    }
    private ChangeMass() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.myRigidbody.mass = current;
    }

    public override void RunEditMode()
    {
        if(Input.mouseScrollDelta.y > 0)
        { 
            current = max;

            Debug.Log("mass editor value = " + current);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            current = min;

            Debug.Log("mass editor value = " + current);
        }
    }
}

public class ChangeMaterial : PhysicsEffect
{
    List<PhysicMaterial> mats;
    int idx;

    private ChangeMaterial() { }
    public ChangeMaterial(List<PhysicMaterial> _mats)
    {
        mats = _mats;
        idx = 0;
    }

    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = new Color(1, 0, 1, 1);
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}

public class ChangeGravity : PhysicsEffect
{
    float min, max;
    public ChangeGravity(float _min, float _max)
    {
        min = _min;
        max = _max;
    }

    private ChangeGravity() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.grey;
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}

public class ChangeLayer : PhysicsEffect
{
    int def, lay1, lay2;
    public ChangeLayer(int _def, int _lay1, int _lay2)
    {
        def = _def;
        lay1 = _lay1;
        lay2 = _lay2;
    }

    private ChangeLayer() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.cyan;
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}

public class ToggleKinematic : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.green;
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}

public class ApplyForce : PhysicsEffect
{
    Vector3 force;
    public ApplyForce(Vector3 _force)
    {
        force = _force;
    }

    private ApplyForce() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = new Color(0, 1, 1, 1);
        Debug.Log("boom");
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}

public class UseMagnet : PhysicsEffect
{
    Vector3 force;
    public UseMagnet(Vector3 _force)
    {
        force = _force;
    }

    private UseMagnet() { }
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.red;
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}

public class ApplyTorque : PhysicsEffect
{
    Vector3 force;
    public ApplyTorque(Vector3 _force)
    {
        force = _force;
    }

    private ApplyTorque() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = new Color(1, 1, 0, 1);
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}


