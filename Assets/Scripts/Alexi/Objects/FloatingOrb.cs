using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingOrb : InteractableObject
{
    public override void OnPointerEnter()
    {
        PhysicsGun.currentObject = this;
    }

    public override void OnPointerExit()
    {
        PhysicsGun.currentObject = null;
    }
}
