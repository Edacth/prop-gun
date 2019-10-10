using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkEmitterScript : MonoBehaviour
{
    float age = 0;

    void Update()
    {
        age += Time.deltaTime;
        if (age > 1)
        {
            Destroy(gameObject);
        }
    }
}
