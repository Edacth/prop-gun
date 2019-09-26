using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// maintains a list of interactable objects that correspond to a gun mode
/// </summary>
public class InteractableObjectCollection : MonoBehaviour
{
    [Tooltip("What kind of interaction mode this collection represents")]
    public InteractableObjectCollectionManager.GunMode interactionMode;
    [Tooltip("The color to highlight the objects")]
    public Color highlightColor;
    [Tooltip("All the objects that will be highlighted")]
    public List<InteractableObject> objects;

    /// <summary>
    /// highlights all objects 
    /// </summary>
    public void Highlight()
    {
        // ToDo: determine highlighting implementation
    }

    /// <summary>
    /// unhighlights all objects
    /// </summary>
    public void UnHighlight()
    {
        // ToDo: determine highlighting implementation
    }
}
