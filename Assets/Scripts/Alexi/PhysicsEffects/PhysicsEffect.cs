using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        //Debug.Log("enter " + GetType().ToString() + " editor");

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
        //Debug.Log("exit " + GetType().ToString() + " editor");

        // EffectEditor.Close();
    }

    /// <summary>
    /// initialize effect
    /// </summary>
    public virtual void OnPointerEnter(InteractableObject target) { }

    /// <summary>
    /// update interactable object visual indicator effects
    /// </summary>
    /// <param name="target"></param>
    public abstract void OnPointerStay(InteractableObject target);

    /// <summary>
    /// terminate effect
    /// </summary>
    public virtual void OnPointerExit() { }

    public virtual void OnSwitchedTo() { }

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
    }

    public override void OnPointerEnter(InteractableObject target)
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
    MaterialUI currentMat;
    int idx;
    public ChangeMaterial()
    {
        idx = 0;
        if (PhysicsValues.instance.physMaterials.Length <= 0) { return; }

        currentMat = PhysicsValues.instance.physMaterials[idx];
    }

    public override void ApplyEffect(InteractableObject target)
    {
        if(null == current) { return; }
        target.myCollider.material = currentMat.material;

        Debug.Log(target.name + " physics material set to " + currentMat.name);
    }

    public override void RunEditMode()
    {
        if (!PhysicsValues.instance.materialEnabled) { return; }

        if(Input.mouseScrollDelta.y == 0) { return; }
        else if(Input.mouseScrollDelta.y > 0)
        {
            idx++;
            if(idx >= PhysicsValues.instance.physMaterials.Length) { idx = 0; }          
        }
        else
        {
            idx--;
            if (idx < 0) { idx = PhysicsValues.instance.physMaterials.Length - 1; }
        }

        currentMat = PhysicsValues.instance.physMaterials[idx];
        PhysicsValues.instance.matName.text = currentMat.name;
        PhysicsValues.instance.matImage.sprite = currentMat.image;
    }

    public override void OnPointerStay(InteractableObject target)
    {
        for (float i = 0; i < 2 * Mathf.PI; i += Mathf.PI / 12)
        {
            Debug.DrawLine(target.transform.position, target.transform.position 
                + new Vector3(Mathf.Cos(i) * 2, Mathf.Sin(i) * 2, 0), Color.red);
        }
    }
}

public class ChangeGravity : PhysicsEffect
{
    bool modeOfSettage;
    Image gravityImage;
    TMP_Text gravityValue;
    bool newTarget;

    public ChangeGravity()
    {
        if(null == gravityImage) { return; }

        gravityImage = PhysicsValues.instance.gravityImage.GetComponent<Image>();
        gravityValue = PhysicsValues.instance.gravityValue.GetComponent<TMP_Text>();
    }

    public override void ApplyEffect(InteractableObject target)
    {
        if(null == gravityImage) { return; } // no action if values not assigned

        target.myRigidbody.useGravity = !target.myRigidbody.useGravity;
        gravityImage.sprite = target.myRigidbody.useGravity ? PhysicsValues.instance.onSprite : PhysicsValues.instance.offSprite;
        gravityValue.text = target.myRigidbody.useGravity ? "ON" : "OFF";
    }

    public override void OnPointerEnter(InteractableObject target)
    {
        newTarget = true;
    }

    public override void OnPointerStay(InteractableObject target)
    {
        if (newTarget)
        {
            if (null != target)
            {
                gravityImage.sprite = target.myRigidbody.useGravity ? PhysicsValues.instance.onSprite : PhysicsValues.instance.offSprite;
                gravityValue.text = target.myRigidbody.useGravity ? "ON" : "OFF";
            }
            newTarget = false;
        }
    }

    public override void OnPointerExit()
    {
        if (!PhysicsValues.instance.gravityEnabled) { return; }

        gravityImage.sprite = PhysicsValues.instance.noneSprite;
        gravityValue.text = "NULL";
    }

    public override void RunEditMode() { }

    public override void OnSwitchedTo()
    {
        if (!PhysicsValues.instance.gravityEnabled) { return; }

        gravityImage.sprite = PhysicsValues.instance.noneSprite;
        gravityValue.text = "NULL";
        newTarget = true;
    }
}

public class ChangeLayer : PhysicsEffect
{
    int idx;
    LayerUI currentLayer;
    public ChangeLayer()
    {
        if (!PhysicsValues.instance.layerEnabled) { return; }

        idx = 0;
        currentLayer = PhysicsValues.instance.layers[idx];
    }

    public override void ApplyEffect(InteractableObject target)
    {
        target.gameObject.layer = currentLayer.layer;
    }

    public override void RunEditMode()
    {
        if (!PhysicsValues.instance.layerEnabled) { return; }

        if(Input.mouseScrollDelta.y == 0) { return; }
        else if (Input.mouseScrollDelta.y > 0)
        {
            idx++;
            if (idx >= PhysicsValues.instance.layers.Length) { idx = 0; }
        }
        else
        {
            idx--;
            if (idx < 0) { idx = PhysicsValues.instance.layers.Length - 1; }
        }

        currentLayer = PhysicsValues.instance.layers[idx];
        PhysicsValues.instance.layerName.text = currentLayer.name;
        PhysicsValues.instance.layerImage.sprite = currentLayer.image;
    }

    public override void OnPointerStay(InteractableObject target)
    {
        for (float i = 0; i < 2 * Mathf.PI; i += Mathf.PI / 12)
        {
            Debug.DrawLine(target.transform.position, target.transform.position
                + new Vector3(0, Mathf.Sin(i) * 2, Mathf.Cos(i) * 2), Color.green);
        }
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

        force = new Vector3(rotatedX, force.y, rotatedZ);
        //Debug.Log(camera.transform.localEulerAngles);

        Debug.DrawRay(target.transform.position, force.normalized * 2 , Color.red);
    }

    public override void RunEditMode()
    {
        if (Input.mouseScrollDelta.y > 0)
        {

            rotation += stepAmount;
            if (rotation > 360) { rotation -= 360; }
            PhysicsValues.instance.forceImage.transform.eulerAngles = new Vector3(0, 0, rotation);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            rotation -= stepAmount;
            if (rotation < 360) { rotation += 360; }
            PhysicsValues.instance.forceImage.transform.eulerAngles = new Vector3(0, 0, rotation);
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


