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
    [Tooltip("Holder for image to display")]
    GameObject display;
    [SerializeField]
    [Tooltip("Image to display")]
    Sprite sprite;
    [SerializeField]
    [Tooltip("Mode this represents")]
    PhysicsGun.Mode mode;

    MeshRenderer mr;

    void Start()
    {
        mr = GetComponentInChildren<MeshRenderer>();
        HideDisplay();        
    }

    void Update()
    {
        transform.LookAt(PhysicsValues.instance.visualTarget);
    }

    /// <summary>
    /// show the visualizer
    /// </summary>
    public void ShowDisplay(Vector3 origin)
    {
        mr.enabled = true;

        transform.position = origin;
    }

    /// <summary>
    /// hide the visualizer
    /// </summary>
    public void HideDisplay()
    {
        mr.enabled = false;
    }
}
