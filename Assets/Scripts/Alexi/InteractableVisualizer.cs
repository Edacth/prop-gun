using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// shows icon for physics effect
/// </summary>
public class InteractableVisualizer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("holder for image to display")]
    Image display;
    [SerializeField]
    [Tooltip("Mode this represents")]
    PhysicsGun.Mode mode;

    void Start()
    {
        HideDisplay();
    }

    void Update()
    {
        transform.LookAt(PhysicsValues.instance.visualTarget);
    }

    /// <summary>
    /// show the visualizer
    /// </summary>
    public void ShowDisplay(Vector3 origin, Sprite sprite)
    {
        display.enabled = true;
        // display.sprite = sprite;

        transform.position = origin;
    }

    /// <summary>
    /// hide the visualizer
    /// </summary>
    public void HideDisplay()
    {
        display.enabled = false;
    }
}
