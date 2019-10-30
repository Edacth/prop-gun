using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForceToolAdjuster : MonoBehaviour
{
    [SerializeField]
    Vector3 forceValueToSet = new Vector3(0, 1000, 0);
    TextMeshPro myText;

    private void Start()
    {
        myText = transform.GetChild(0).GetComponent<TextMeshPro>();
        myText.text = "Enter me to set 'Add Force' vector to:\n" + forceValueToSet.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PhysicsValues>().force = forceValueToSet;
        }
    }
}
 