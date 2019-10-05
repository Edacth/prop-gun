using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    float lerpGoal = 0;
    float lerpFraction = 0 ;

    [SerializeField]
    Vector3 normalOffset = new Vector3(0.216f, -0.636f, 1.026f);
    [SerializeField]
    Vector3 scopedOffset = new Vector3(0f, -0.579f, 0.702f);
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lerpGoal = 1;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            lerpGoal = 0;
        }

        if (lerpFraction < lerpGoal)
        {
            lerpFraction += 2.0f * Time.deltaTime;
            lerpFraction = Mathf.Clamp01(lerpFraction);
        }
        else if (lerpFraction > lerpGoal)
        {
            lerpFraction -= 2.0f * Time.deltaTime;
            lerpFraction = Mathf.Clamp01(lerpFraction);
        }

        transform.localPosition = Vector3.Lerp(normalOffset, scopedOffset, lerpFraction);
    }
}
