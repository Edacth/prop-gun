using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killZplane : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }
}
