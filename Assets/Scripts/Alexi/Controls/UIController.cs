using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// controls ui
/// </summary>
public class UIController : MonoBehaviour
{
    [Tooltip("Score holder")] [SerializeField] TextMeshProUGUI scoreText;
    [Tooltip("Instructions")] [SerializeField] GameObject instructions;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) { instructions.SetActive(!instructions.activeSelf); }
    }
}
