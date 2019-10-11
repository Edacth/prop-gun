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
    

    public delegate void OnInteractableCheckDelegate();
    public static event OnInteractableCheckDelegate interactableCheckDelegate;

    private void Start()
    {
        layerMask = ~layerMask;
        lineRenderer = GetComponent<LineRenderer>();
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
        if (interactableCheckDelegate != null)
        {
            interactableCheckDelegate();
        }
    }

    public RaycastHit getRaycastHit()
    {
        return raycastHit;
    }
}