using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLauncher : MonoBehaviour
{
    public InteractableObject projectile;

    [SerializeField]
    public Vector3 force = new Vector3(-1, 1, 0);
    [Range(1, 5)]
    public float rate = 2;

    float elapsed;

    // Start is called before the first frame update
    void Start()
    {
        elapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > rate)
        {
            elapsed = 0;

            InteractableObject io = Instantiate(projectile, transform.position, Quaternion.identity)
                .GetComponent<InteractableObject>();
            
            io.myRigidbody.AddExplosionForce(force.magnitude, transform.position - force.normalized, 10);
        }
    }
}
