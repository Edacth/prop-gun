using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    InteractableChecker interactableChecker;
    MeshRenderer _meshRenderer;
    Color unselectedColor = Color.white;
    Color selectedColor = Color.green;
    bool selected = false;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        
    }

    void CheckIfSelected()
    {
        if (interactableChecker.getRaycastHit().transform == transform && !selected)
        {

            Debug.Log(gameObject.name + " is selected");
            _meshRenderer.material.color = selectedColor;
            selected = true;
        }
        else if (interactableChecker.getRaycastHit().transform != transform && selected)
        {
            _meshRenderer.material.color = unselectedColor;
            selected = false;
        }
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
