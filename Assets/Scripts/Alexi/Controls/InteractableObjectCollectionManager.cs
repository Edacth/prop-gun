using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manages collections of interactable objects
/// </summary>
public class InteractableObjectCollectionManager : MonoBehaviour
{
    [Tooltip("All the interactable objects")]
    public List<InteractableObject> objects;

    PhysicsGun.Mode currentMode; // current gun mode

    void Start()
    {
        currentMode = PhysicsGun.currentMode; // initialize to default

        // initialize another way
        objects = new List<InteractableObject>(FindObjectsOfType<InteractableObject>());
    }

    /// <summary>
    /// updates interactable visuals
    /// </summary>
    /// 
    /// <param name="newMode">
    /// the new gun mode
    /// </param>
    public void SwitchMode(PhysicsGun.Mode newMode)
    {
        foreach (InteractableObject io in objects.FindAll(io => io.compatibleModes.Contains(currentMode))) // ToDo: make less expensive
        {
            io.MarkActive();
        }

        currentMode = newMode;

        foreach (InteractableObject io in objects.FindAll(io => io.compatibleModes.Contains(currentMode)))
        {
            io.UnmarkActive();
        }
    }
}
