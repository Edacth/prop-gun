using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingOrb : InteractableObject
{
    public override void OnPointerEnter()
    {
        PhysicsGun.currentObject = this;

        visual?.ShowDisplay(transform.position);
    }

    public override void OnPointerExit()
    {
        PhysicsGun.currentObject = null;

        visual?.HideDisplay();
    }
}
