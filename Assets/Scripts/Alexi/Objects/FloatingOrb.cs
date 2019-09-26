using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingOrb : InteractableObject
{
    public override void OnPointerEnter()
    {
        MarkActive();
    }

    public override void OnPointerExit()
    {
        UnmarkActive();
    }
}
