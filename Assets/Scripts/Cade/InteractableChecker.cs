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

    public delegate void OnInteractableCheckDelegate();
    public static event OnInteractableCheckDelegate interactableCheckDelegate;

    void Update()
    {
        Debug.DrawRay(sourceTransform.position, sourceTransform.forward * checkDistance, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
        InteractableCheck();
    }

    void InteractableCheck()
    {
        Physics.Raycast(sourceTransform.position, sourceTransform.forward, out raycastHit, checkDistance);
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
