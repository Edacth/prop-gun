﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ToDo: add ui, colors, inspector vars, etc

/// <summary>
/// An effect on an object's physics
/// </summary>
public abstract class PhysicsEffect
{
    public static PhysicsEffect current;

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

    // public abstract void OnPointerEnter();

    /// <summary>
    /// update interactable object visual indicator effects
    /// </summary>
    /// <param name="target"></param>
    public abstract void OnPointerStay(InteractableObject target);

    // public abstract void OnPointerExit();

}

/// <summary>
/// change the mass of the object
/// </summary>
public class ChangeMass : PhysicsEffect
{
    // ToDo: add mass delta step (not just min/max options)

    float min, max, currentMass;
    public ChangeMass(float _min, float _max)
    {
        min = currentMass = _min;
        max = _max;
    }
    private ChangeMass() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.myRigidbody.mass = currentMass;

        Debug.Log(target.name + " mass changed to " + currentMass);
    }

    public override void OnPointerStay(InteractableObject target)
    {
        for(float i = 0; i < 2 * Mathf.PI; i += Mathf.PI / 12)
        {
            Debug.DrawLine(target.transform.position, target.transform.position + new Vector3(Mathf.Cos(i) * currentMass, 0, Mathf.Sin(i) * currentMass), Color.red);
        }
    }

    public override void RunEditMode()
    {
        if(Input.mouseScrollDelta.y > 0)
        { 
            currentMass = max;

            Debug.Log("mass editor value = " + currentMass);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            currentMass = min;

            Debug.Log("mass editor value = " + currentMass);
        }
    }
}

/// <summary>
/// change the physics material
/// </summary>
public class ChangeMaterial : PhysicsEffect
{
    List<PhysicMaterial> mats;
    PhysicMaterial current;
    int idx;

    private ChangeMaterial() { }
    public ChangeMaterial(List<PhysicMaterial> _mats)
    {
        mats = _mats;
        idx = 0;
        current = mats[idx];
    }

    public override void ApplyEffect(InteractableObject target)
    {
        target.myCollider.material = current;

        Debug.Log(target.name + " physics material set to " + current.name);
    }

    public override void RunEditMode()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            idx++;
            if(idx >= mats.Count) { idx = 0; }
            current = mats[idx];

            Debug.Log("material editor value set to " + current.name);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            idx--;
            if (idx < 0) { idx = mats.Count - 1; }
            current = mats[idx];

            Debug.Log("material editor value set to " + current.name);
        }
    }

    public override void OnPointerStay(InteractableObject target)
    {
        // material things
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

    public override void OnPointerStay(InteractableObject target)
    {
        // gravity things
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}

public class ChangeLayer : PhysicsEffect
{
    struct Layer
    {
        public string name;
        public int layer;

        public Layer(string n, int l)
        {
            name = n;
            layer = l;
        }
    }
    List<Layer> layers;

    int def, lay1, lay2, idx;
    Layer current;
    public ChangeLayer(int _def, int _lay1, int _lay2)
    {
        def = _def;
        lay1 = _lay1;
        lay2 = _lay2;
        idx = 0;

        layers = new List<Layer>();
        layers.Add(new Layer("default", def));
        layers.Add(new Layer("layer1", lay1));
        layers.Add(new Layer("layer2", lay2));

        current = layers[idx];
    }

    private ChangeLayer() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.gameObject.layer = current.layer;

        Debug.Log(target.name + " layer set to " + current.layer + ": " + current.name);
    }

    public override void RunEditMode()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            idx++;
            if (idx >= layers.Count) { idx = 0; }
            current = layers[idx];

            Debug.Log("layer editor value set to " + current.layer);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            idx--;
            if (idx < 0) { idx = layers.Count - 1; }
            current = layers[idx];

            Debug.Log("layer editor value set to " + current.layer);
        }
    }

    public override void OnPointerStay(InteractableObject target)
    {
        // layer things
    }
}

public class ToggleKinematic : PhysicsEffect
{
    public override void ApplyEffect(InteractableObject target)
    {
        target.GetComponent<Renderer>().material.color = Color.green;
    }

    public override void OnPointerStay(InteractableObject target)
    {
        // kinematic things
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}

public class ApplyForce : PhysicsEffect
{
    Vector3 defaultForce;
    Vector3 force;
    float rotation;
    public ApplyForce(Vector3 _defaultForce)
    {
        defaultForce = _defaultForce;
        force = defaultForce;
        rotation = 0.0f;
    }

    private ApplyForce() { }

    public override void ApplyEffect(InteractableObject target)
    {
        Debug.Log("Force Effect Applied");
        target.GetComponent<Renderer>().material.color = new Color(0, 1, 1, 1);
        target.myRigidbody.AddForce(force);
        
    }

    public override void OnPointerStay(InteractableObject target)
    {
        Debug.DrawRay(target.transform.position, force.normalized * 2 , Color.red);
    }

    public override void RunEditMode()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            

            Debug.Log("Mouse scroll value" + Input.mouseScrollDelta.y);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            //current = min;

            //Debug.Log("mass editor value = " + current);
        }
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

    public override void OnPointerStay(InteractableObject target)
    {
        // magnet things
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

    public override void OnPointerStay(InteractableObject target)
    {
        // torque things
    }

    public override void RunEditMode()
    {
        throw new System.NotImplementedException();
    }
}


