using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(KeyValuePair<PhysicsGun.Mode, PhysicsEffect> effectPair in PhysicsGun.effects)
        {
            effectPair.Value.effectAppliedEvent += Thing;
        }
    }

    public void Thing()
    {
        Debug.Log("Thing");
    }
}
