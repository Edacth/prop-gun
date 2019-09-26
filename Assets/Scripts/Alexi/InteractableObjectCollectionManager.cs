using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manages collections of interactable objects
/// </summary>
public class InteractableObjectCollectionManager : MonoBehaviour
{
    // ToDo: move to gun script
    public enum GunMode
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

    [Tooltip("All the interactable object collections")]
    public List<InteractableObjectCollection> collections;

    GunMode currentMode; // current gun mode

    void Start()
    {
        currentMode = GunMode.mass; // initialize to default
    }

    /// <summary>
    /// updates interactable visuals
    /// </summary>
    /// 
    /// <param name="newMode">
    /// the new gun mode
    /// </param>
    public void SwitchMode(GunMode newMode)
    {
        foreach (InteractableObjectCollection ioc in collections.FindAll(ioc => ioc.interactionMode == currentMode))
        {
            ioc.UnHighlight();
        }

        currentMode = newMode;

        foreach (InteractableObjectCollection ioc in collections.FindAll(ioc => ioc.interactionMode == currentMode))
        {
            ioc.Highlight();
        }
    }
}
