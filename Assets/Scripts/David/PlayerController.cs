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
    float speed = 0;
    Rigidbody[] rbs;

    // action events
    public delegate void JumpEvent();
    public static event JumpEvent jumpEvent;
 
    private void Start()
    {
        rbs = transform.GetComponentsInChildren<Rigidbody>();
        verticalRotation = transform.eulerAngles.y;
        rb = GetComponent<Rigidbody>();
        lookDir = cam.transform.rotation;
    }

    void Die()
    {
        GetComponentInChildren<Animator>().enabled = false;

        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].isKinematic = false;
            rbs[i].transform.GetComponent<Collider>().enabled = true;
        }
    }
    private void Update()
    {
        if (Input.GetButton("Jump")&&grounded)
        {
            Jump();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        jumpedThisFrame = false;
        fakeVelocity += Physics.gravity*Time.fixedDeltaTime;
        rb.MovePosition(transform.position+fakeVelocity*Time.fixedDeltaTime);
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
           
            if (jumpedThisFrame)
            {
                Die();
                
                Accelerate((Vector3.ProjectOnPlane(cam.transform.forward * Input.GetAxisRaw("Vertical") + cam.transform.right * Input.GetAxisRaw("Horizontal"), Vector3.up)).normalized, airSpeed, airAccel);
            } else
            {
                Accelerate((Vector3.ProjectOnPlane(cam.transform.forward * Input.GetAxisRaw("Vertical") + cam.transform.right * Input.GetAxisRaw("Horizontal"), Vector3.up)).normalized, groundSpeed, groundAccel);
                fakeVelocity -= new Vector3(fakeVelocity.x * groundedFriction * Time.fixedDeltaTime, 0, fakeVelocity.z * groundedFriction * Time.fixedDeltaTime);
            }
            
        } else
        {
            Accelerate((Vector3.ProjectOnPlane(cam.transform.forward * Input.GetAxisRaw("Vertical") + cam.transform.right * Input.GetAxisRaw("Horizontal"), Vector3.up)).normalized, airSpeed, airAccel);
        }
        
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(0,0,100,100), speed.ToString());
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
        this.speed = speed;
        float addspeed = wishSpeed - speed;
        if (addspeed <= 0)
        {
            return;
        }
        float accelSpeed = accel * Time.fixedDeltaTime;
        if (accelSpeed<addspeed)
        {
            addspeed = accelSpeed;
        }

        fakeVelocity+=addspeed*wishDir;
        return;        
    }
    void Jump()
    {
        grounded = false;
        jumpedThisFrame = true;
        fakeVelocity = new Vector3(fakeVelocity.x,jumpPower,fakeVelocity.z);

        jumpEvent?.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (jumpedThisFrame) return;
        Vector3 normal = collision.contacts[0].normal;
        if (Vector3.Dot(normal, Vector3.up) > 0.7f)
        {
            grounded = true;
        }
        float backOff = Vector3.Dot(fakeVelocity,normal);

        if(backOff < 0)
        {
            backOff *= overBounce;
        } else
        {
            return;
            backOff /= overBounce;//honestly why does quake do this? makes no sense to me
        }
        if (collision.collider.attachedRigidbody)
        {
            
            collision.collider.attachedRigidbody.AddForceAtPosition(backOff*normal, collision.contacts[0].point,ForceMode.VelocityChange);
        }
        fakeVelocity-=normal*backOff;
    }
    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
