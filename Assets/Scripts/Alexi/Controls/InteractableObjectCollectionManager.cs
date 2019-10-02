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

    static PhysicsGun.Mode currentMode; // current gun mode

    void Start()
    {
        currentMode = PhysicsGun.currentMode;

        // initialize another way
        objects = new List<InteractableObject>(FindObjectsOfType<InteractableObject>());

        foreach(InteractableObject io in objects)
        {
            io.UnmarkActive();
        }
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
            io.UnmarkActive();
        }

        currentMode = newMode;

        foreach (InteractableObject io in objects.FindAll(io => io.compatibleModes.Contains(currentMode)))
        {
            io.MarkActive();
        }
    }

    /// <summary>
    /// checks if object ie selesctable
    /// </summary>
    /// 
    /// <param name="io">
    /// object to check
    /// param>
    /// <returns>
    /// this objece is selectable
    /// </returns>
    public static bool QuerySelectable(InteractableObject io)
    {
        return io.compatibleModes.Contains(currentMode);
    }
}
