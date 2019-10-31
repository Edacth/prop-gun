using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING: there may be errors due to script order here

/// <summary>
/// manages collections of interactable objects
/// </summary>
public class InteractableObjectCollectionManager : MonoBehaviour
{
    public static List<InteractableObject> objects { get; private set; } // all interactable objects
    static PhysicsGun.Mode currentMode; // current gun mode

    void Start()
    {
        currentMode = PhysicsGun.currentMode;
        // foreach(InteractableObject io in objects) { SortInteractable(io); }
    }

    /// <summary>
    /// updates interactable visuals
    /// </summary>
    /// 
    /// <param name="newMode">
    /// the new gun mode
    /// </param>
    public static void SwitchMode(PhysicsGun.Mode newMode)
    {
        foreach (InteractableObject io in objects.FindAll(io => io.compatibleModes.Contains(currentMode))) // ToDo: make less expensive
        {
            io.UnmarkActive();
        }

        currentMode = newMode;

        foreach (InteractableObject io in objects.FindAll(io => io.compatibleModes.Contains(newMode)))
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
    /// this object is selectable
    /// </returns>
    public static bool QuerySelectable(InteractableObject io)
    {
        return io.compatibleModes.Contains(currentMode);
    }

    /// <summary>
    /// add an object to the list
    /// </summary>
    /// 
    /// <param name="io">
    /// the object to add 
    /// </param>
    public static void PushInteractable(InteractableObject io)
    {
        if(null == objects) { objects = new List<InteractableObject>(); }
        objects.Add(io);

        SortInteractable(io);
    }

    /// <summary>
    /// remove an object from the list
    /// </summary>
    /// 
    /// <param name="io">
    /// the object to remove
    /// </param>
    public static void PopInteractable(InteractableObject io)
    {
        io.UnmarkActive();
        objects.Remove(io);
    }

    /// <summary>
    /// mark or unmark object given mode
    /// </summary>
    /// 
    /// <param name="io">
    /// new object to sort
    /// </param>
    static void SortInteractable(InteractableObject io)
    {
        if (null == io.compatibleModes) { Debug.Log(io.gameObject.name); }
        if (io.compatibleModes.Contains(currentMode)) { io.MarkActive(); }
        else { io.UnmarkActive(); }
    }
}
