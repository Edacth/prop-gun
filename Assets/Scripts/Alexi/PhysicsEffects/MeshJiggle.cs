using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=UxLJ6XewTVs

// attach to every jelly object
public class MeshJiggle : MonoBehaviour
{
    public float bounceSpeed;
    public float fallForce;
    public float stiffness;

    MeshFilter meshFilter;
    Mesh mesh;

    JellyVertex[] jellyVertices;
    Vector3[] currentMeshVertices;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        GetVertices();
    }

    public void GetVertices()
    {
        jellyVertices = new JellyVertex[mesh.vertices.Length];
        currentMeshVertices = new Vector3[mesh.vertices.Length];
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            jellyVertices[i] = new JellyVertex(i, mesh.vertices[i], mesh.vertices[i], Vector3.zero);
            currentMeshVertices[i] = mesh.vertices[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVertices();
    }

    void UpdateVertices()
    {
        // update allllllll the vertices
        for(int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].UpdateVelocity(bounceSpeed);
            jellyVertices[i].Settle(stiffness);

            jellyVertices[i].currentPosition += jellyVertices[i].currentVelocity * Time.deltaTime;
            currentMeshVertices[i] = jellyVertices[i].currentPosition;
        }

        mesh.vertices = currentMeshVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] collisionPoints = collision.contacts;
        for(int i = 0; i < collisionPoints.Length; i++)
        {
            Vector3 inputPoint = collisionPoints[i].point + (collisionPoints[i].point * 0.1f);
            ApplyPressureToPoint(inputPoint, fallForce);
        }
    }

    public void OnRaycastHit(Vector3 position)
    {
        ApplyPressureToPoint(position, fallForce);
    }

    void ApplyPressureToPoint(Vector3 _point, float _pressure)
    {
        for(int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].ApplyPressureToVertex(transform, _point, _pressure);
        }
    }
}

public class JellyVertex
{
    public int index;
    public Vector3 initialPosition, currentPosition, currentVelocity;

    public JellyVertex(int _idx, Vector3 _init, Vector3 _cur, Vector3 _vel)
    {
        index = _idx;
        initialPosition = _init;
        currentPosition = _cur;
        currentVelocity = _cur;
    }

    public Vector3 GetCurrentDisplacement()
    {
        return currentPosition - initialPosition;
    }

    public void UpdateVelocity(float _bounceSpeed)
    {
        currentVelocity = currentVelocity - GetCurrentDisplacement() * _bounceSpeed * Time.deltaTime;
    }

    public void Settle(float _stiffness)
    {
        currentVelocity *= 1f - _stiffness * Time.deltaTime;
    }

    public void ApplyPressureToVertex(Transform _transform, Vector3 _position, float _pressure)
    {
        // distance fom vertex to input position (where mesh was touched)
        Vector3 distanceVertexPoint = currentPosition - _transform.InverseTransformPoint(_position);

        float adaptedPressure = _pressure / (1f * distanceVertexPoint.sqrMagnitude);
        float velocity = adaptedPressure * Time.deltaTime;
        currentVelocity += distanceVertexPoint.normalized * velocity;
    }
}
