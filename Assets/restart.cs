using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restart : MonoBehaviour
{
    Vector3 start = Vector3.zero;
    void Awake()
    {
        start = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -30)
        {
            transform.position = start;
        }
    }
}