using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    Transform puzzlePiecePrefab;
    Transform puzzlePiece = null;
    // Update is called once per frame
    void Update() //may be slow but is much cleaner than using a callback system and will cause fewer problems in the future
    {
        if(puzzlePiece == null)
        {
            puzzlePiece = Instantiate(puzzlePiecePrefab,transform.position, Quaternion.identity);
        }
    }
}
