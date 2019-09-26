using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// gun that can affect physics properties of objects
/// </summary>
public class PhysicsGun : MonoBehaviour
{
    /// <summary>
    /// how the gun affects physics objects
    /// </summary>
    public enum Mode
    {
        mass,       // increase/decrease mass
        material,   // change physics material
        gravity,    // increase/decrease gravity modifier
        layer,      // change collision layer
        kinematic,  // toggle isKinematic
        force,      // apply pushing force
        magnet,     // apply pulling force
        torque      // apply torque
    };

    public InteractableChecker interactableChecker;
    public PhysicMaterial bounce;

    const int modeCount = 8; // how many modes there are

    public static Mode currentMode;
    public static InteractableObject currentObject;
    InteractableObjectCollectionManager collectionManager;
    static Dictionary<Mode, PhysicsEffect> effects;

    // Start is called before the first frame update
    void Start()
    {
        currentMode = Mode.mass; // initialize to default

        // can be changed to drag and drop for performance later
        collectionManager = FindObjectOfType<InteractableObjectCollectionManager>();

        // initalize effects
        if(null == effects)
        {
            effects = new Dictionary<Mode, PhysicsEffect>();
            effects.Add(Mode.mass, new ChangeMass());
            effects.Add(Mode.material, new ChangeMaterial(bounce));
            effects.Add(Mode.gravity, new ChangeGravity());
            effects.Add(Mode.layer, new ChangeLayer());
            effects.Add(Mode.kinematic, new ToggleKinematic());
            effects.Add(Mode.force, new ApplyForce());
            effects.Add(Mode.magnet, new UseMagnet());
            effects.Add(Mode.torque, new ApplyTorque());
        }
    }

    void Update()
    {
        TakeInput();
    }

    /// <summary>
    /// get mouse and keyboard input
    /// </summary>
    void TakeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if(Input.mouseScrollDelta.y < 0) { ScrollSelection(1); }
        else if(Input.mouseScrollDelta.y > 0) { ScrollSelection(-1); }
    }

    /// <summary>
    /// go to next mode
    /// </summary>
    void ScrollSelection(int add)
    {
        int cur = (int)currentMode;
        cur += add;

        if(cur >= modeCount) { cur = 0; }
        else if(cur < 0) { cur = modeCount - 1; }

        SwitchMode((Mode)cur);

        Debug.Log(currentMode);
    }

    public void SwitchMode(Mode newMode)
    {
        collectionManager?.SwitchMode(newMode);

        // any kind of animation here

        // switch gun ui

        currentMode = newMode;
    }

    /// <summary>
    /// fire the gun
    /// </summary>
    public void Fire()
    {
        if(null == currentObject) { return; }

        PhysicsEffect eff;
        effects.TryGetValue(currentMode, out eff);
        eff?.ApplyEffect(currentObject);
    }
}
