using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PIDfollow3D : MonoBehaviour
{
    Rigidbody rb;
    public float P,I,D;
    private Vector3 lastError;
    private Vector3 error;
    private Vector3 errorSum = Vector3.zero;
    public Transform target; //maybe use a transform?
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        error = target.position - transform.position; //how far off target are we on each axis
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastError = error;
        error = target.position - transform.position; //how far off target are we on each axis
        errorSum += error * Time.fixedDeltaTime;
        //Debug.DrawRay(transform.position, errorSum);
        Debug.DrawRay(transform.position,  Vector3.Project(rb.velocity, rb.velocity) * D * Time.fixedDeltaTime, Color.red);
        Debug.DrawRay(transform.position, rb.velocity, Color.blue);
        Debug.DrawRay(transform.position, new Vector3(Mathf.Sign(error.x), Mathf.Sign(error.y), Mathf.Sign(error.z)));
        Vector3 correction = Vector3.zero;
        //Proportional (Move Towards target)
        correction += error*P;
        //Intergal (Correct tuning issues over time)
        correction += errorSum * I;
        //Derivitive (Don't over shoot)
        correction += Vector3.Project(rb.velocity,rb.velocity) * D * Time.fixedDeltaTime;//how fast are we changing our error; //maybe make use velocity
        rb.AddForce(correction);
        
    }
}
