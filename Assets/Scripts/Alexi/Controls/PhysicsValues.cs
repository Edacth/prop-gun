using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// holds values for physics manipulations
/// </summary>
public class PhysicsValues : MonoBehaviour
{
    public static PhysicsValues instance;

    [Header("Mass")]
    [Tooltip("Minimum allowable mass")]
    public float minMass;
    [Tooltip("Maximum allowable mass")]
    public float maxMass;
    [Tooltip("Mass step value")]
    public float step;
    [Tooltip("Default mass")]
    public float defMass;
    [Tooltip("Mass panel UI")]
    public GameObject massPanel;
    [Tooltip("Mass slider")]
    public Slider massSlider;
    [Tooltip("Mass value display")]
    public TextMeshProUGUI massText;

    [Header("Material")]
    [Tooltip("Available physics materials")]
    public List<PhysicMaterial> physMaterials; // would individual be better?
    [SerializeField]
    public MaterialUI[] m;

    [Header("Gravity")]
    [Tooltip("Gravity panel UI")]
    public GameObject gravityImage;
    [Tooltip("Gravity panel UI")]
    public GameObject gravityValue;
    public Sprite onSprite;
    public Sprite offSprite;
    public Sprite noneSprite;

    [Header("Layer")]
    [Tooltip("Default object layer")]
    public int defaultLayer;
    [Tooltip("Other collison layer")]
    public int layer1;
    [Tooltip("Other collison layer")]
    public int layer2;

    // kinematic (toggle - doesn't need anything)

    [Header("Force")]
    [SerializeField]
    [Tooltip("Force applied")]
    public Vector3 force = new Vector3(0, 0, -600);
    [Tooltip("Amount to increment in run mode")]
    public float forceStepAmount = 10;
    [Tooltip("Camera to base rotation off of. Should be player's camera")]
    public GameObject camera;
    [Tooltip("Mass panel UI")]
    public GameObject forceImage;

    [Header("Magnet")]
    [SerializeField]
    [Tooltip("Force applied towards player")]
    public Vector3 magForce;

    [Header("Torque")]
    [SerializeField]
    [Tooltip("Torque applied")]
    public Vector3 torque;

    [Header("Visualizer images")]
    public Sprite bigMass;
    public Sprite lilMass;
    public Sprite layer;
    public Sprite bounce;
    public Sprite noBounce;
    public Sprite friction;
    public Sprite noFriction;

    [Header("UI Panels")]
    public GameObject[] UIPanels;

    [Header("Other")]
    [Tooltip("Target for visualizers")]
    public Transform visualTarget;

    void Awake()
    {
        instance = this;
        if (null == visualTarget)
        {
            Debug.LogError("No visual target set, cannot display visualizer icons");
        }
    }

}
