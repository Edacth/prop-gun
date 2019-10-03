using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charecterAnimator : MonoBehaviour
{
    public Transform cam;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z);
    }
}
