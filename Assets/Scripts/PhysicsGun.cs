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
        mass, // increase/decrease mass
        material, // change physics material
        gravity, // increase/decrease gravity modifier
        layer, // change collision layer
        kinematic, // toggle isKinematic
        force, // apply pushing force
        magnet, // apply pulling force
        torque // apply torque
    };

    const int modeCount = 8; // how many modes there are

    public static Mode currentMode; 
    InteractableObjectCollectionManager collectionManager;
    Dictionary<Mode, PhysicsEffect> effects;

    // Start is called before the first frame update
    void Start()
    {
        currentMode = Mode.mass; // initialize to default

        // can be changed to drag and drop for performance later
        collectionManager = FindObjectOfType<InteractableObjectCollectionManager>();

        // initalize effects
        effects = new Dictionary<Mode, PhysicsEffect>();
        effects.Add(Mode.mass, new ChangeMass());
        effects.Add(Mode.material, new ChangeMaterial());
        effects.Add(Mode.gravity, new ChangeGravity());
        effects.Add(Mode.layer, new ChangeLayer());
        effects.Add(Mode.kinematic, new ToggleKinematic());
        effects.Add(Mode.force, new ApplyForce());
        effects.Add(Mode.magnet, new UseMagnet());
        effects.Add(Mode.torque, new ApplyTorque());
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

        if(Input.mouseScrollDelta.y < 0)
        {
            ScrollSelection();
        }
    }

    /// <summary>
    /// go to next mode
    /// </summary>
    void ScrollSelection()
    {
        int cur = (int)currentMode;
        cur++;
        if(cur >= modeCount) { cur = 0; }
        SwitchMode((Mode)cur);

        Debug.Log("switched to " + currentMode + " mode");
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
        PhysicsEffect eff;
        effects.TryGetValue(currentMode, out eff);
        eff?.ApplyEffect();
    }
}
