using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCube : InteractableObject
{
    public override void OnPointerEnter()
    {
        PhysicsGun.currentObject = this;

        visual?.ShowDisplay(transform.position);

        PhysicsEffect.current.OnPointerEnter(this);
    }

    public override void OnPointerExit()
    {
        PhysicsGun.currentObject = null;

        visual?.HideDisplay();

        PhysicsEffect.current.OnPointerExit();
    }
}
