using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// deforms the mesh of an object when it hits a collider
/// </summary>
public class MeshBounce : MonoBehaviour
{
    [Tooltip("Mass, m")]            public float m;
    [Tooltip("Spring constant, k")] public float k;
    float t; // time
    float v; // velocity
    float deltaV; // change in velocity
    float deltaY; // change in y
    float initialV; // initial velocity (y axis)
    float currentV; // current velocity (y axis)
    float totalT; // total time

    float height; // the height of the mesh
    Vector3 min, max; // the highest and lowest points of the mesh

    bool isSquishing; // is the object currently colliding with something?

    Rigidbody rb;
    Mesh mesh;
    List<Vector3> originalVerts, squishedVerts;

    void Start()
    {
        isSquishing = false;
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshFilter>().sharedMesh;
        originalVerts = new List<Vector3>();
        squishedVerts = new List<Vector3>();
        mesh.GetVertices(originalVerts);
        mesh.GetVertices(squishedVerts);

        CalculateHeight();
    }

    void CalculateHeight()
    {
        min = max = originalVerts[0];
        foreach(Vector3 cur in originalVerts)
        {
            if(cur.y < min.y) { min = cur; }
            else if(cur.y > max.y) { max = cur; }
        }
        height = max.y - min.y;

        Debug.Log("height: " + height);
    }

    void OnCollisionEnter(Collision collision)
    {
        isSquishing = true;
        t = 0; // initialize motion
        initialV = rb.velocity.y;
        Debug.Log("vi: " + initialV);

        deltaV = 2 * initialV; // change and go equally positive

        totalT = 2 * Mathf.PI * Mathf.Pow(m / k, 0.5f);
    }

    void Update()
    {
        if (!isSquishing) { return; }

        t += Time.deltaTime;

        CalculateVertices();
        mesh.SetVertices(squishedVerts);
    }

    void CalculateVertices()
    {
        Vector3 cur;

        // currentV = ; // ??????
        deltaV = initialV = currentV;

        // calculate max offset
        deltaY = (-m * deltaV) / (k * t);
        float scale = deltaY / height;

        // set vertices as a scale of the offset
        for(int i = 0; i < squishedVerts.Count; i++)
        {
            cur = squishedVerts[i];
            cur.y = Mathf.Lerp(min.y, max.y, scale);
            squishedVerts[i] = cur;
        }
    }

    private void OnApplicationQuit()
    {
        mesh.SetVertices(originalVerts);
    }
}
