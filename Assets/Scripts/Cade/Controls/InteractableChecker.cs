using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableChecker : MonoBehaviour
{
    [SerializeField]
    Transform sourceTransform = null;
    [SerializeField]
    float checkDistance = 5.0f;
    RaycastHit raycastHit;
    LayerMask layerMask = 1 << 2;

    LineRenderer lineRenderer;

    Transform oldHit;

    public delegate void OnInteractableCheckDelegate();
    public static event OnInteractableCheckDelegate interactableCheckDelegate;

    private void Start()
    {
        layerMask = ~layerMask; // invert

        lineRenderer = GetComponent<LineRenderer>();
        Physics.Raycast(sourceTransform.position, sourceTransform.forward, out raycastHit, checkDistance, layerMask);
        oldHit = raycastHit.transform;
    }

    void Update()
    {
        Debug.DrawRay(sourceTransform.position, sourceTransform.forward * checkDistance, Color.red);
        lineRenderer.SetPosition(0, transform.InverseTransformPoint(sourceTransform.position));
        lineRenderer.SetPosition(1, Vector3.forward * checkDistance);

        InteractableCheck();
    }

    void InteractableCheck()
    {
        Physics.Raycast(sourceTransform.position, sourceTransform.forward, out raycastHit, checkDistance, layerMask);
        if (oldHit != raycastHit.transform) { CheckInteractableHelper(); }
    }

    public void CheckInteractableHelper()
    {
        if (interactableCheckDelegate != null)
        {
            // if its new thing, current thing exits (if not null)
            if (PhysicsGun.currentInteractingObject != null && raycastHit.transform != PhysicsGun.currentInteractingObject.transform) { PhysicsGun.currentInteractingObject.OnPointerExit(); }
            //if not hitting interactable
            if (raycastHit.transform == null || (raycastHit.transform.gameObject.layer != 10 && raycastHit.transform.gameObject.layer != 11))
            {
                PhysicsGun.currentPointingObject = PhysicsGun.currentInteractingObject = null;
            }
            else
            {
                interactableCheckDelegate();
            }

            oldHit = raycastHit.transform;

            if (PhysicsGun.currentInteractingObject != null && PhysicsGun.currentInteractingObject.transform != oldHit) { PhysicsGun.currentInteractingObject = null; }
        }
    }

    public RaycastHit getRaycastHit()
    {
        return raycastHit;
    }
}