using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectLauncherNoPool : MonoBehaviour
{
    [SerializeField] Transform target;
    public float power;
    Vector3 dir;
    private void Start()
    {
        dir = target.position - transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb  =other.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(dir*power,ForceMode.Impulse);
    }
}
