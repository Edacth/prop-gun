using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// represents an interactable physics object
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class InteractableObject : MonoBehaviour
{
    public Rigidbody myRigidbody { get; protected set; }
    public Collider myCollider { get; protected set; }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
    }

    /// <summary>
    /// called on selection enter
    /// </summary>
    public abstract void OnPointerEnter();

    /// <summary>
    /// called on selection exit
    /// </summary>
    public abstract void OnPointerExit();
}
