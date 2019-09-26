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

    // Start is called before the first frame update
    void Start()
    {
        currentMode = Mode.mass; // initialize to default

        // can be changed to drag and drop for performance later
        collectionManager = FindObjectOfType<InteractableObjectCollectionManager>();
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
        Debug.Log("fire");
    }
}
