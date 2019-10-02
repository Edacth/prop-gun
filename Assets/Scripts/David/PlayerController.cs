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
    [SerializeField] float overBounce = 1.05f;
    float verticalRotation = 0;
    float horizontalRotation = 90;
    bool jumpedThisFrame = false;
    Rigidbody rb;
    bool grounded = false;
    Vector3 fakeVelocity = Vector3.zero;
    
    private void Start()
    {
        verticalRotation = transform.eulerAngles.y;
        rb = GetComponent<Rigidbody>();
        lookDir = cam.transform.rotation;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        jumpedThisFrame = false;
        fakeVelocity += Physics.gravity*Time.fixedDeltaTime;
        transform.position += fakeVelocity*Time.fixedDeltaTime;
        // print(fakeVelocity);
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
            Accelerate((Vector3.ProjectOnPlane(cam.transform.forward * Input.GetAxisRaw("Vertical") + cam.transform.right * Input.GetAxisRaw("Horizontal"), Vector3.up)).normalized, groundAccel, groundSpeed);
            if (Input.GetAxis("Jump")>0)
            {
                Jump();
            } else
            {
                fakeVelocity -= new Vector3(fakeVelocity.x * groundedFriction * Time.fixedDeltaTime, 0, fakeVelocity.z * groundedFriction * Time.fixedDeltaTime);
            }
            
        } else
        {
            Accelerate((Vector3.ProjectOnPlane(cam.transform.forward * Input.GetAxisRaw("Vertical") + cam.transform.right * Input.GetAxisRaw("Horizontal"), Vector3.up)).normalized, airAccel, airSpeed);
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
        float speed = Vector3.Dot(fakeVelocity,wishDir);
        
        float addspeed = wishSpeed - speed;
        if (addspeed <= 0)
        {
            return;
        }
        addspeed = Mathf.Min(addspeed,accel*Time.fixedDeltaTime);

        fakeVelocity+=addspeed*wishDir;
        return;        
    }
    void Jump()
    {
        grounded = false;
        jumpedThisFrame = true;
        fakeVelocity = new Vector3(fakeVelocity.x,jumpPower,fakeVelocity.z);
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (jumpedThisFrame) return;
        Vector3 normal = collision.contacts[0].normal;
        if (Vector3.Dot(normal, Vector3.up) > 0.5f)
        {
            grounded = true;
        }
        float backOff = Vector3.Dot(fakeVelocity,normal);

        if(backOff < 0)
        {
            backOff *= overBounce;
        } else
        {
            backOff /= overBounce;
        }
        
        fakeVelocity-=normal*backOff;
    }
    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
