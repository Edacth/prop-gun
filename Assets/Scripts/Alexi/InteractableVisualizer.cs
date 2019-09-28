using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// shows icon for physics effect
/// </summary>
public class InteractableVisualizer : MonoBehaviour
{
    public Image display; // holder for image to display

    public static InteractableVisualizer instance;

    Transform target; // target to show graphic

    void Start()
    {
        instance = this;
        HideDisplay();
    }

    void Update()
    {
        if(null != target)
        {
            transform.LookAt(target);
        }
    }

    /// <summary>
    /// show the visualizer
    /// </summary>
    public void ShowDisplay(Transform _target, Vector3 origin, Sprite sprite)
    {
        display.enabled = true;
        target = _target;
        // display.sprite = sprite;

        transform.position = origin;
    }

    /// <summary>
    /// hide the visualizer
    /// </summary>
    public void HideDisplay()
    {
        display.enabled = false;
        target = null;
    }
}
