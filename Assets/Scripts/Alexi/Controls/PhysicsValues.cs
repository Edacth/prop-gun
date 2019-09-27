using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// holds values for physics manipulations
/// </summary>
public class PhysicsValues : MonoBehaviour
{
    [Header("Mass")]
    public float minMass;
    public float maxMass;
    [Header("Material")]
    public List<PhysicMaterial> physMaterials;
    [Header("Gravity")]
    public float minGrav;
    public float maxGrav;
    [Header("Layer")]
    public int defaultLayer;
    public int layer1;
    public int layer2;
    // kinematic
    [Header("Force")]
    [SerializeField]
    public Vector3 force;
    [Header("Magnet")]
    [SerializeField]
    public Vector3 magForce;
    [Header("Torque")]
    [SerializeField]
    public Vector3 torque;
}
