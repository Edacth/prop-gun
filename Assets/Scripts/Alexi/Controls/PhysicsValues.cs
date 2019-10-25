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
    public bool massEnabled = false;
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
    public bool materialEnabled = false;
    [Tooltip("Available physics materials")]
    public MaterialUI[] physMaterials;
    [Tooltip("Material image")]
    public Image matImage;
    [Tooltip("Material name text")]
    public TextMeshProUGUI matName;

    [Header("Gravity")]
    public bool gravityEnabled = false;
    [Tooltip("Gravity panel UI")]
    public GameObject gravityImage;
    [Tooltip("Gravity panel UI")]
    public GameObject gravityValue;
    public Sprite onSprite;
    public Sprite offSprite;
    public Sprite noneSprite;

    [Header("Layer")]
    public int objectLayer = 10;
    public int objectOutlineLayer = 11;
    public int objectGoThruLayer = 12;
    [Tooltip("Layer image")]
    public Image layerImage;
    [Tooltip("Layer name text")]
    public TextMeshProUGUI layerName;
    public string defaultLayerName;
    public Sprite defaultLayerSprite;
    public string thruLayerName;
    public Sprite thruLayerSprite;

    [Header("Kinematic")]
    [Tooltip("Mass panel UI")]
    public GameObject kinematicValue;

    [Header("Force")]
    public bool forceEnabled = false;
    [SerializeField]
    [Tooltip("Force applied")]
    public Vector3 force = new Vector3(0, 0, -600);
    [Tooltip("Amount to increment in run mode")]
    public float forceStepAmount = 10;
    [Tooltip("Camera to base rotation off of. Should be player's camera")]
    public GameObject camera;
    [Tooltip("Mass panel UI")]
    public GameObject forceImage;

    [Header("UI Panels")]
    public GameObject[] UIPanels;

    [Header("Other")]
    [Tooltip("Particle system at the shot point")]
    public GameObject shotPointParticles;

    void Awake()
    {
        instance = this;
    }

    public static bool IsGameObjectOnObjectLayer(GameObject toCheck)
    {
        if(toCheck.layer == instance.objectLayer 
            || toCheck.layer == instance.objectOutlineLayer 
            || toCheck.layer == instance.objectGoThruLayer) { return true; }
        return false;
    }
}
