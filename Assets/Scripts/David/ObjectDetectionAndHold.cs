using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectionAndHold : MonoBehaviour
{
    [SerializeField]
    Animator doorAnimatior;
    [SerializeField]
    GameObject[] acceptedObjects;
    List<GameObject> penetratingObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        other.attachedRigidbody.isKinematic = true;
        OpenDoor();
    }

    public void OpenDoor()
    {
        Debug.Log("Open Door");
        doorAnimatior.SetBool("Open", true);
    }

    public void CloseDoor()
    {
        Debug.Log("Close Door");
        doorAnimatior.SetBool("Open", false);
    }
}
