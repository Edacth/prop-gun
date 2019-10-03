using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingOrb : InteractableObject
{
    public override void OnPointerEnter()
    {
        PhysicsGun.currentObject = this;

        visual?.ShowDisplay(transform.position);
        
        // This might be a really dumb way to do this. SORRY ALEXI
        PhysicsEffect.current.OnPointerEnter(this);
    }

    public override void OnPointerExit()
    {
        PhysicsGun.currentObject = null;

        visual?.HideDisplay();

        // This might be a really dumb way to do this. SORRY ALEXI
        PhysicsEffect.current.OnPointerExit();
    }
}
