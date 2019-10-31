using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollObject : InteractableObject
{
    public bool isRoot = false;

    static List<Rigidbody> rbs;
    Vector3 origin;

    public new void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myMeshRenderer = GetComponent<MeshRenderer>();
        lineRenderer = GetComponent<LineRenderer>();

        interactableChecker = GameObject.FindObjectOfType<InteractableChecker>(); // change this
        lineRenderer.SetPosition(1, Vector3.zero);

        origin = transform.position;

        if (isRoot)
        {
            rbs = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
            foreach (Rigidbody rb in rbs)
            {
                if(rb.gameObject == gameObject) { continue; }

                rb.mass = 1.0f / 12;
                rb.gameObject.AddComponent<RagdollObject>();
                List<PhysicsGun.Mode> newModes = new List<PhysicsGun.Mode>();
                RagdollObject ro = rb.GetComponent<RagdollObject>();
                ro.compatibleModes = newModes;
                ro.isRoot = false;
                ro.Start();
            }
        }

        InteractableObjectCollectionManager.PushInteractable(this);
    }

    private void Update()
    {
        if(isRoot && transform.position.y < -5)
        {
            myRigidbody.velocity = Vector3.zero;
            foreach(Rigidbody rb in rbs) { rb.velocity = Vector3.zero; }
            transform.position = origin;
        }
    }

    public override void MarkActive()
    {
        base.MarkActive();
        SetLayerRecursively(transform, PhysicsValues.instance.objectOutlineLayer);
    }

    public override void UnmarkActive()
    {
        base.UnmarkActive();
        SetLayerRecursively(transform, PhysicsValues.instance.objectLayer);
    }

    void SetLayerRecursively(Transform tran, int layer)
    {
        tran.gameObject.layer = layer;
        for (int i = 0; i < tran.childCount; i++)
        {
            SetLayerRecursively(tran.GetChild(i), layer); // chungus
        }
    }
}
