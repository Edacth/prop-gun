using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Quaternion lookDir;
    [SerializeField] Camera cam; //i don't know how to make this required
    Vector3 lastMousePos;
    Vector3 mouseDelta;
    [SerializeField] float horizontalSensitivity = 0.5f;
    [SerializeField] float verticalSensitivity = 0.5f;
    [SerializeField] float maxAngle = 8;
    [SerializeField] float groundedFriction = 0.5f; //in -% per second
    [SerializeField] float jumpPower = 5f;
    [SerializeField] float groundAccel = 5f;
    [SerializeField] float airAccel = 1f;
    [SerializeField] float groundSpeed = 5f;
    [SerializeField] float airSpeed = 2f;
    float verticalRotation = 0;
    float horizontalRotation = 90;
    Rigidbody rb;
    bool grounded = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lookDir = cam.transform.rotation;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lastMousePos = Input.mousePosition;
        //lookDir = cam.transform.rotation;
        
        //print(maxRotation);
        horizontalRotation += Input.GetAxisRaw("Mouse X") * horizontalSensitivity;
        verticalRotation += Input.GetAxisRaw("Mouse Y") * verticalSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation,-90+maxAngle,90-maxAngle);
        cam.transform.rotation = Quaternion.Euler(0, horizontalRotation, 0) * Quaternion.AngleAxis(verticalRotation, lookDir * Vector3.right); //Remeber changing the order changes the result
        if(grounded)
        {
            if (Input.GetAxis("Jump")>0)
            {
                Jump();
            } else
            {
                rb.velocity -= new Vector3(rb.velocity.x * groundedFriction * Time.fixedDeltaTime, 0, rb.velocity.z * groundedFriction * Time.fixedDeltaTime);
            }
            Accelerate((Vector3.ProjectOnPlane(cam.transform.forward * Input.GetAxisRaw("Vertical") + cam.transform.right * Input.GetAxisRaw("Horizontal"), Vector3.up)).normalized, groundAccel, 4);
        } else
        {
            Accelerate((Vector3.ProjectOnPlane(cam.transform.forward * Input.GetAxisRaw("Vertical") + cam.transform.right * Input.GetAxisRaw("Horizontal"), Vector3.up)).normalized, airAccel, 4);
        }
        
    }
    void Accelerate(Vector3 wishDir, float wishSpeed, float accel)
    {
        if (Mathf.Approximately(wishDir.magnitude, 0))
        {
            return;
        }
        #if DEBUG
        if (!Mathf.Approximately(wishDir.magnitude,1))
        {
            throw new System.Exception("normalize your inputs SHEESH (magnitude: " + wishDir.magnitude + ")");
        }
        #endif
        float speed = Vector3.Dot(rb.velocity,wishDir);
        
        float addspeed = wishSpeed-speed * Time.fixedDeltaTime * accel;
        if(!(speed > wishSpeed && addspeed>0))
        {
            rb.velocity+=addspeed*wishDir;
        }
        return;        
    }
    void Jump()
    {
        grounded = false;
        rb.velocity += new Vector3(0,jumpPower,0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
