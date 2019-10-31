using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// represents an interactable physics object
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(LineRenderer))]
[Serializable]
public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    [Tooltip("All modes that can interact with this object")]
    public List<PhysicsGun.Mode> compatibleModes;
    public bool shouldAddRestartScript = true;

    public Rigidbody myRigidbody { get; private set; }
    public Collider myCollider { get; private set; }

    // Cade
    [SerializeField]
    InteractableChecker interactableChecker = null;
    public MeshRenderer myMeshRenderer { get; private set; }
    LineRenderer lineRenderer;
    Color unselectedColor = Color.white;
    Color selectedColor = Color.green;
    bool selected = false; // is this object currently selected?
    bool selectable; // can this object be selected? (mode-wise) 

    public void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myMeshRenderer = GetComponent<MeshRenderer>();
        lineRenderer = GetComponent<LineRenderer>();

        InteractableObjectCollectionManager.PushInteractable(this);

        interactableChecker = GameObject.FindObjectOfType<InteractableChecker>(); // change this
        lineRenderer.SetPosition(1, Vector3.zero);

        if (shouldAddRestartScript) { gameObject.AddComponent<restart>(); }
    }

    void Update()
    {
        // ToDo: Rewrite logic
        if(null != PhysicsGun.currentInteractingObject) { PhysicsEffect.current.OnPointerStay(PhysicsGun.currentInteractingObject); }


        // if (Input.GetKeyDown(KeyCode.F)) { Debug.Log(PhysicsGun.currentInteractingObject == null ? "null" : PhysicsGun.currentInteractingObject.name); }
    }

    /// <summary>
    /// update with new selection
    /// </summary>
    void UpdateSelection()
    {
        if (interactableChecker.getRaycastHit().transform == transform && !selected)
        {
            PhysicsGun.currentPointingObject = this;
            if (selectable)
            {
                PhysicsGun.currentInteractingObject = this;
                OnPointerEnter();
            }
        }
    }

    /// <summary>
    /// called on selection enter
    /// </summary>
    public virtual void OnPointerEnter()
    {
        PhysicsEffect.current.OnPointerEnter(this);
        selected = true;
    }

    /// <summary>
    /// called on selection exit
    /// </summary>
    public virtual void OnPointerExit()
    {
        PhysicsEffect.current.OnPointerExit(this);
        selected = false;
    }

    /// <summary>
    /// shows this object can be interacted with
    /// </summary>
    public void MarkActive()
    {
        gameObject.layer = PhysicsValues.instance.objectOutlineLayer;
        selectable = true;
    }

    /// <summary>
    /// takes away interactable indication
    /// </summary>
    public void UnmarkActive()
    {
        gameObject.layer = PhysicsValues.instance.objectLayer; // default
        selectable = false;
    }

    // Subscribing Delegate
    private void OnEnable()
    {
        InteractableChecker.interactableCheckDelegate += UpdateSelection;
    }

    // Unsubsribing Delegate
    private void OnDisable()
    {
        InteractableChecker.interactableCheckDelegate -= UpdateSelection;
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

    public void setLineActive(bool enabled)
    {
        lineRenderer.enabled = enabled;
    }

    public void SetLineDirection(Vector3 direction)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + direction);
    }

    void OnDestroy()
    {
        InteractableObjectCollectionManager.PopInteractable(this);
    }
}
