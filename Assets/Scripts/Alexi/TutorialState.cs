using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState : MonoBehaviour
{
    List<PhysicsGun.Mode> modesLeftToTry;

    void Start()
    {
        modesLeftToTry = new List<PhysicsGun.Mode>();
        foreach(KeyValuePair<PhysicsGun.Mode, PhysicsEffect> effectPair in PhysicsGun.effects)
        {
            effectPair.Value.effectAppliedEvent += ApplyEffectAction;
            modesLeftToTry.Add(effectPair.Key);
        }
    }

    public void ApplyEffectAction()
    {

        Debug.Log(modesLeftToTry.Count + " modes left to try");

        if (modesLeftToTry.Remove(PhysicsGun.currentMode))
        {
            SectionCompleted();
        }
    }

    void SectionCompleted()
    {

    }
}
