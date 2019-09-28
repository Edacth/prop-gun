using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// holds values for physics manipulations
/// </summary>
public class PhysicsValues : MonoBehaviour
{
    [Header("Mass")]
    [Tooltip("Minimum allowable mass")]
    public float minMass;
    [Tooltip("Maximum allowable mass")]
    public float maxMass;
    [Tooltip("Mass step value")]
    public float massStep;

    [Header("Material")]
    [Tooltip("Available physics materials")]
    public List<PhysicMaterial> physMaterials; // would individual be better?

    [Header("Gravity")]
    [Tooltip("Minimum allowable gravity scale")]
    public float minGrav;
    [Tooltip("Maximum allowable gravity scale")]
    public float maxGrav;

    [Header("Layer")]
    [Tooltip("Default object layer")]
    public int defaultLayer;
    [Tooltip("Other collison layer")]
    public int layer1;
    [Tooltip("Other collison layer")]
    public int layer2;

    // kinematic (toggle - doesn't need anything

    [Header("Force")]
    [SerializeField]
    [Tooltip("Force applied")]
    public Vector3 force;
    public GameObject camera;

    [Header("Magnet")]
    [SerializeField]
    [Tooltip("Force applied towards player")]
    public Vector3 magForce;

    [Header("Torque")]
    [SerializeField]
    [Tooltip("Torque applied")]
    public Vector3 torque;
}
