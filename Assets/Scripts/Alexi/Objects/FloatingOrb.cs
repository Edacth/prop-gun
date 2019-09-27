using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingOrb : InteractableObject
{
    public override void OnPointerEnter()
    {
        PhysicsGun.currentObject = this;

        myMeshRenderer.material.color = Color.blue;
    }

    public override void OnPointerExit()
    {
        PhysicsGun.currentObject = null;

        myMeshRenderer.material.color = Color.white;
    }
}
