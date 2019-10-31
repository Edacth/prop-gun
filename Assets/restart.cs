using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restart : MonoBehaviour
{
    Vector3 start = Vector3.zero;
    public float zplane = 0;
    void Awake()
    {
        start = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < zplane)
        {
            transform.position = start;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}