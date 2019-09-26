using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// represents an interactable physics object
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[Serializable]
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField]
    [Tooltip("All modes that can interact with this object")]
    public List<PhysicsGun.Mode> compatibleModes;

    public Rigidbody myRigidbody { get; private set; }
    public Collider myCollider { get; private set; }

    public static PhysicsGun gun;

    // Cade
    [SerializeField]
    InteractableChecker interactableChecker;
    MeshRenderer _meshRenderer;
    Color unselectedColor = Color.white;
    Color selectedColor = Color.green;
    bool selected = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();

        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void CheckIfSelected()
    {
        if (interactableChecker.getRaycastHit().transform == transform && !selected)
        {
            /*
            Debug.Log(gameObject.name + " is selected");
            _meshRenderer.material.color = selectedColor;
            */

            selected = true;
            OnPointerEnter();
        }
        else if (interactableChecker.getRaycastHit().transform != transform && selected)
        {

            // _meshRenderer.material.color = unselectedColor;

            selected = false;
            OnPointerExit();
        }
    }

    /// <summary>
    /// called on selection enter
    /// </summary>
    public abstract void OnPointerEnter();

    /// <summary>
    /// called on selection exit
    /// </summary>
    public abstract void OnPointerExit();

    /// <summary>
    /// shows this object can be interacted with
    /// </summary>
    public void MarkActive()
    {
        // ToDo: implement interactability effect

        GetComponent<Renderer>().material.color = Color.blue;
    }

    /// <summary>
    /// takes away interactable indication
    /// </summary>
    public void UnmarkActive()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    // Subscribing Delegate
    private void OnEnable()
    {
        InteractableChecker.interactableCheckDelegate += CheckIfSelected;
    }

    // Unsubsribing Delegate
    private void OnDisable()
    {
        InteractableChecker.interactableCheckDelegate -= CheckIfSelected;
    }
}
