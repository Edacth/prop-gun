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

    public static InteractableObject currentSelection { get; private set; }

    // Cade
    [SerializeField]
    InteractableChecker interactableChecker;
    public MeshRenderer myMeshRenderer { get; private set; }
    Color unselectedColor = Color.white;
    Color selectedColor = Color.green;
    bool selected = false;
    bool selectable;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();

        myMeshRenderer = GetComponent<MeshRenderer>();

        selectable = true;
        myMeshRenderer = GetComponent<MeshRenderer>();
    }

    void CheckIfSelected()
    {
        if (interactableChecker.getRaycastHit().transform == transform && !selected && selectable)
        {
            // _meshRenderer.material.color = selectedColor;
            currentSelection = this;
            selected = true;
            OnPointerEnter();
            // PhysicsEffect.current.OnPointerEnter();
        }
        else if (interactableChecker.getRaycastHit().transform != transform && selected)
        {
            myMeshRenderer.material.color = unselectedColor;

            currentSelection = null;
            selected = false;
            OnPointerExit();
            // PhysicsEffect.current.OnPointerExit();
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

        selectable = true;

        Debug.Log(name + " active");
    }

    /// <summary>
    /// takes away interactable indication
    /// </summary>
    public void UnmarkActive()
    {
        selectable = false;

        Debug.Log(name + " inactive");
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
