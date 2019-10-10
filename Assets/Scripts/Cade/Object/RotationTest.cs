using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    [Range(0, 360), SerializeField]
    float rotation = 0;
    public Vector3 point = new Vector3( 1, 0, 0);
    public Vector3 source = new Vector3(0, 1, 0);

    void Start()
    {
        
    }

    void Update()
    {
        float rotationInRadians = (rotation) * (Mathf.PI / 180); // Convert to radians
        float rotatedX = Mathf.Cos(rotationInRadians) * (point.x) - Mathf.Sin(rotationInRadians) * (point.z);
        float rotatedZ = Mathf.Sin(rotationInRadians) * (point.x) + Mathf.Cos(rotationInRadians) * (point.z);

        Debug.DrawRay(transform.position + source, new Vector3(rotatedX, 0, rotatedZ), Color.red);
        Debug.Log("Force Vector: " + point.ToString("F3") + "Rotation: " + rotation);
    }
}
