using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns objects
public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform spawnPoint;
    public float spawnTimer;
    public int maxSpawnCount;
    [SerializeField] public Vector3 force;
    public bool useCollision;
    public Transform parentTransform;

    float spawnElapsed;
    int spawnCount;

    // Start is called before the first frame update
    void Start()
    {
        spawnElapsed = spawnTimer;
        spawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (useCollision) { return; }
        spawnElapsed += Time.deltaTime;
        if (spawnElapsed >= spawnTimer)
        {
            Spawn();
            if (spawnCount >= maxSpawnCount) { enabled = false; }
        }
    }

    void Spawn()
    {
        spawnElapsed = 0f;
        spawnCount++;
        GameObject g = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity, parentTransform);
        if (force.magnitude != 0) { g.GetComponent<Rigidbody>().AddForce(force); }
    }

    void OnTriggerStay(Collider other)
    {     
        if (!useCollision) { return; }
        spawnElapsed += Time.deltaTime;
        if (spawnElapsed >= spawnTimer)
        {
            Spawn();
        }
    }
}
