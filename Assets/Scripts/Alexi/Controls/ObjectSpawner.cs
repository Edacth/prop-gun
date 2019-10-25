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
        spawnElapsed += Time.deltaTime;
        if(spawnElapsed >= spawnTimer)
        {
            spawnElapsed = 0f;
            spawnCount++;
            Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);

            if(spawnCount >= maxSpawnCount) { gameObject.SetActive(false); }
        }
    }
}
