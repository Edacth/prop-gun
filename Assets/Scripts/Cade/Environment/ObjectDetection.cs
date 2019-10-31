﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    [SerializeField]
    Animator doorAnimatior;
    [SerializeField]
    Transform door;
    [SerializeField]
    GameObject[] acceptedObjects;
    List<GameObject> penetratingObjects = new List<GameObject>();
    [SerializeField]
    bool animationTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < acceptedObjects.Length; i++)
        {   
            if (other.gameObject == acceptedObjects[i])
            {
                if (animationTrigger) {
                OpenDoor();
                }
                else
                {
                    door.gameObject.SetActive(false);
                }
            }
        }
        //Debug.Log(other.gameObject.name);
        penetratingObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        bool containsAcceptedObject = false;
        for (int i = 0; i < penetratingObjects.Count; i++)
        {
            if (penetratingObjects[i] == other.gameObject)
            {
                penetratingObjects.RemoveAt(i);
            }
        }
        // Close door if no more accepted object are inside
        for (int i = 0; i < penetratingObjects.Count; i++)
        {
            for (int j = 0; j < acceptedObjects.Length; j++)
            {
                if (penetratingObjects[i] == acceptedObjects[j])
                {
                    containsAcceptedObject = true;
                    break;
                }
            }
        }

        if (!containsAcceptedObject)
        {
            if (animationTrigger)
            {
                CloseDoor();
            } else {
                door.gameObject.SetActive(true);
            }
        }
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
