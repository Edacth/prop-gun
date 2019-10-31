using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZKillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.transform.gameObject);
    }
}
