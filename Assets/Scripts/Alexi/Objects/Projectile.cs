using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a projectile object
public class Projectile : InteractableObject
{
    [Tooltip("Assign bounce material on start?")]
    public bool startBouncy = false;

    float lifetime;
    ObjectLauncher recyclePoint;

    void Start()
    {
        if (startBouncy)
        {
            PhysicMaterial pm = new PhysicMaterial();
            pm.bounciness = 1;
            pm.bounceCombine = PhysicMaterialCombine.Maximum;
            myCollider.material = pm;
        }    
    }

    public override void OnPointerEnter()
    {
        PhysicsGun.currentObject = this;
        PhysicsEffect.current.OnPointerEnter(this);
        Debug.Log(name + " selected");
    }

    public override void OnPointerExit()
    {
        PhysicsGun.currentObject = null;
        PhysicsEffect.current.OnPointerExit(this);
        Debug.Log(name + " unselected");
    }

    /// <summary>
    /// sets a timer and destroys object after
    /// </summary>
    /// 
    /// <param name="_lifetime">
    /// time until destrction
    /// </param>
    /// <param name="_recyclePoint">
    /// where to recycle the object
    /// </param>
    public void StartLifetimeCountdown(float _lifetime, ObjectLauncher _recyclePoint)
    {
        recyclePoint = _recyclePoint;
        lifetime = _lifetime;
        StartCoroutine("WaitForLifetime");
    }

    IEnumerator WaitForLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        recyclePoint.RecycleProjectile(this);
    }
}
