using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// catches launched objects
/// </summary>
public class ObjectCatcher : MonoBehaviour
{
    int points; // score

    private void OnTriggerEnter(Collider other)
    {
        if(null != other.GetComponent<Projectile>()) { points++; }
    }
}
