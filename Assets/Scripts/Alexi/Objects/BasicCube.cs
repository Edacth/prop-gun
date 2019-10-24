using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCube : InteractableObject // this script does not have a purpose
{
    public override void OnPointerEnter()
    {
        PhysicsGun.currentInteractingObject = PhysicsGun.currentPointingObject = this;
        PhysicsEffect.current.OnPointerEnter(this);
    }

    public override void OnPointerExit()
    {
        PhysicsGun.currentInteractingObject = PhysicsGun.currentPointingObject = null;
        PhysicsEffect.current.OnPointerExit(this);
    }
}
