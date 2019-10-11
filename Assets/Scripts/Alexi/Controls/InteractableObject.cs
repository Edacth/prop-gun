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
    [Tooltip("Effect visualiation")]
    public InteractableVisualizer visual;

    public Rigidbody myRigidbody { get; private set; }
    public Collider myCollider { get; private set; }

    public static InteractableObject currentSelection { get; private set; }

    // Cade
    [SerializeField]
    InteractableChecker interactableChecker = null;
    public MeshRenderer myMeshRenderer { get; private set; }
    Color unselectedColor = Color.white;
    Color selectedColor = Color.green;
    bool selected = false;
    bool selectable;

    void Awake()
    {
        interactableChecker = FindObjectOfType<InteractableChecker>();
        grabTarget = GameObject.Find("GrabPoint").transform;
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myMeshRenderer = GetComponent<MeshRenderer>();

        selectable = true;
        // selectable = InteractableObjectCollectionManager.QuerySelectable(this);

        interactableChecker = GameObject.FindObjectOfType<InteractableChecker>(); // change this
    }

    void Update()
    {
        if (selected)
        {
            if(null == PhysicsEffect.current)
            { /*Debug.LogWarning("Invalid physics effect");*/ }
            else { PhysicsEffect.current.OnPointerStay(this); }

        } 
    }

    void CheckIfSelected()
    {
        if (!selectable) { return; }

        if (interactableChecker.getRaycastHit().transform == transform && !selected)
        {
            // _meshRenderer.material.color = selectedColor;
            currentSelection = this;
            selected = true;
            OnPointerEnter();
            // PhysicsEffect.current.OnPointerEnter();
        }
        else if (interactableChecker.getRaycastHit().transform != transform && selected)
        {
            //myMeshRenderer.material.color = unselectedColor;

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

        // Debug.Log(name + " active");
    }

    /// <summary>
    /// takes away interactable indication
    /// </summary>
    public void UnmarkActive()
    {
        selectable = false;

        // Debug.Log(name + " inactive");
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

    public float P = 25, I = 3, D = -60;
    private Vector3 lastError;
    private Vector3 error = Vector3.zero;
    private Vector3 errorSum = Vector3.zero;
    public Transform grabTarget;
    public void grabUpdate()//with our current archatecture it makes most sense for Physics gun to call this, instead of this calling it's self
    {
        lastError = error;
        error = grabTarget.position - transform.position; //how far off target are we on each axis
        errorSum += error * Time.fixedDeltaTime;
        //Debug.DrawRay(transform.position, errorSum);
        Debug.DrawRay(transform.position, Vector3.Project(myRigidbody.velocity, myRigidbody.velocity) * D * Time.fixedDeltaTime, Color.red);
        Debug.DrawRay(transform.position, myRigidbody.velocity, Color.blue);
        Debug.DrawRay(transform.position, new Vector3(Mathf.Sign(error.x), Mathf.Sign(error.y), Mathf.Sign(error.z)));
        Vector3 correction = Vector3.zero;
        //Proportional (Move Towards target)
        correction += error * P;
        //Intergal (Correct tuning issues over time)
        correction += errorSum * I;
        //Derivitive (Don't over shoot)
        correction += Vector3.Project(myRigidbody.velocity, myRigidbody.velocity) * D * Time.fixedDeltaTime;//how fast are we changing our error; //maybe make use velocity
        myRigidbody.AddForce(correction);
    }
}
