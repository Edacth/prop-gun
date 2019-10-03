using System.Collections;
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
    public virtual void EnterEditMode()
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
    public virtual void ExitEditMode()
    {
        Debug.Log("exit " + GetType().ToString() + " editor");

        // EffectEditor.Close();
    }

    /// <summary>
    /// initialize effect
    /// </summary>
    public virtual void OnPointerEnter() { }

    /// <summary>
    /// update interactable object visual indicator effects
    /// </summary>
    /// <param name="target"></param>
    public abstract void OnPointerStay(InteractableObject target);

    /// <summary>
    /// terminate effect
    /// </summary>
    public virtual void OnPointerExit() { }

}

/// <summary>
/// change the mass of the object
/// </summary>
public class ChangeMass : PhysicsEffect
{
    // ToDo: add mass delta step (not just min/max options)

    float currentMass;

    public ChangeMass()
    {
        currentMass = PhysicsValues.instance.defMass;
        PhysicsValues.instance.massSlider.value =
                Mathf.InverseLerp(PhysicsValues.instance.minMass, PhysicsValues.instance.maxMass, currentMass);
        PhysicsValues.instance.massText.text = currentMass.ToString("0.0");
    }

    public override void ApplyEffect(InteractableObject target)
    {
        target.myRigidbody.mass = currentMass;
        target.myRigidbody.WakeUp();


        Debug.Log(target.name + " mass changed to " + currentMass);
    }

    public override void OnPointerEnter()
    {
        // base.OnPointerEnter();
        // InteractableVisualizer.instance.ShowDisplay(InteractableObject.currentSelection.transform, Vector3.zero, PhysicsValues.instance.bigMass); // object position
    }

    public override void OnPointerStay(InteractableObject target)
    {
        for(float i = 0; i < 2 * Mathf.PI; i += Mathf.PI / 12)
        {
            Debug.DrawLine(target.transform.position, target.transform.position + new Vector3(Mathf.Cos(i) * currentMass, 0, Mathf.Sin(i) * currentMass), Color.red);
        }
    }

    public override void OnPointerExit()
    {
        // base.OnPointerExit();
        // InteractableVisualizer.instance.HideDisplay();
    }

    public override void EnterEditMode()
    {
        base.EnterEditMode();
        PhysicsValues.instance.massPanel.SetActive(true);
    }

    public override void RunEditMode()
    {
        if(Input.mouseScrollDelta.y > 0)
        { 
            currentMass += PhysicsValues.instance.step;
            if(currentMass > PhysicsValues.instance.maxMass)
            {
                currentMass = PhysicsValues.instance.maxMass;
            }
            PhysicsValues.instance.massSlider.value =
                Mathf.InverseLerp(PhysicsValues.instance.minMass, PhysicsValues.instance.maxMass, currentMass);
            PhysicsValues.instance.massText.text = currentMass.ToString("0.0");

            Debug.Log("mass editor value = " + currentMass);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            currentMass -= PhysicsValues.instance.step;
            if (currentMass < PhysicsValues.instance.minMass)
            {
                currentMass = PhysicsValues.instance.minMass;
            }
            PhysicsValues.instance.massSlider.value = 
                Mathf.InverseLerp(PhysicsValues.instance.minMass, PhysicsValues.instance.maxMass, currentMass);
            PhysicsValues.instance.massText.text = currentMass.ToString("0.0");

            Debug.Log("mass editor value = " + currentMass);
        }
    }

    public override void ExitEditMode()
    {
        base.ExitEditMode();
        //PhysicsValues.instance.massPanel.SetActive(false);
    }
}

/// <summary>
/// change the physics material
/// </summary>
public class ChangeMaterial : PhysicsEffect
{
    List<PhysicMaterial> mats;
    PhysicMaterial currentMat;
    int idx;

    private ChangeMaterial() { }
    public ChangeMaterial(List<PhysicMaterial> _mats)
    {
        mats = _mats;
        idx = 0;
        if (mats.Count <= 0) { return; }

        currentMat = mats[idx];
    }

    public override void ApplyEffect(InteractableObject target)
    {
        if(null == current) { return; }
        target.myCollider.material = currentMat;

        Debug.Log(target.name + " physics material set to " + currentMat.name);
    }

    public override void RunEditMode()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            idx++;
            if(idx >= mats.Count) { idx = 0; }
            currentMat = mats[idx];

            Debug.Log("material editor value set to " + currentMat.name);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            idx--;
            if (idx < 0) { idx = mats.Count - 1; }
            currentMat = mats[idx];

            Debug.Log("material editor value set to " + currentMat.name);
        }
    }

    public override void OnPointerStay(InteractableObject target)
    {
        // material things
    }
}

public class ChangeGravity : PhysicsEffect
{
    bool ModeOfSettage;

    public ChangeGravity() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.myRigidbody.useGravity = !target.myRigidbody.useGravity;
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
    Layer currentLayer;
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

        currentLayer = layers[idx];
    }

    private ChangeLayer() { }

    public override void ApplyEffect(InteractableObject target)
    {
        target.gameObject.layer = currentLayer.layer;

        Debug.Log(target.name + " layer set to " + currentLayer.layer + ": " + currentLayer.name);
    }

    public override void RunEditMode()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            idx++;
            if (idx >= layers.Count) { idx = 0; }
            currentLayer = layers[idx];

            Debug.Log("layer editor value set to " + currentLayer.layer);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            idx--;
            if (idx < 0) { idx = layers.Count - 1; }
            currentLayer = layers[idx];

            Debug.Log("layer editor value set to " + currentLayer.layer);
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
    float stepAmount;
    GameObject camera;
    public ApplyForce(Vector3 _defaultForce, float _stepAmount, GameObject _camera)
    {
        defaultForce = _defaultForce;
        force = defaultForce;
        stepAmount = _stepAmount;
        rotation = 0.0f;
        camera = _camera;
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
        float rotationInRadians = ((rotation) * (Mathf.PI / 180)) - ((camera.transform.localEulerAngles.y) * (Mathf.PI / 180)); // Convert to radians
        float rotatedX = Mathf.Cos(rotationInRadians) * (defaultForce.x) - Mathf.Sin(rotationInRadians) * (defaultForce.z);
        float rotatedZ = Mathf.Sin(rotationInRadians) * (defaultForce.x) + Mathf.Cos(rotationInRadians) * (defaultForce.z);

        force = new Vector3(rotatedX, 0, rotatedZ);
        //Debug.Log(camera.transform.localEulerAngles);

        Debug.DrawRay(target.transform.position, force.normalized * 2 , Color.red);
    }

    public override void RunEditMode()
    {
        if (Input.mouseScrollDelta.y > 0)
        {

            rotation += stepAmount;
            if (rotation > 360) { rotation -= 360; }

        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            rotation -= stepAmount;
            if (rotation < 360) { rotation += 360; }

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


