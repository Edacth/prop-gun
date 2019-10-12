using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class GlowOutlinePostProcessing : MonoBehaviour
{
    [Tooltip("Main scene view camera")]
    [SerializeField] Camera mainCamera;
    [Tooltip("Physics object view camera")]
    [SerializeField] Camera objectCamera;
    [Tooltip("Object layer number")]
    [SerializeField] int objectLayer = 11;
    [Tooltip("Object layer manipulation")]
    [SerializeField] Material _effect;
    [Tooltip("HIde objects from main camera?")]
    [SerializeField] bool hideObjects = false;
    [Tooltip("Outline colors for different modes")]
    [SerializeField] List<TypeColor> typeColors;
    [Header("Setup")]
    [Tooltip("Find objects and move them to object layer?")]
    [SerializeField] bool setObjectLayer = false;

    RenderTexture _secondaryDepth;
    Dictionary<PhysicsGun.Mode, Color> colorKey;

    public static int ObjectLayer { get; private set; }

    void Awake()
    {
        ObjectLayer = objectLayer;
        colorKey = new Dictionary<PhysicsGun.Mode, Color>();
        foreach (TypeColor tc in typeColors) { colorKey.Add(tc.mode, tc.outlineColor); }
    }

    void Start()
    {
        if (hideObjects) { mainCamera.cullingMask &= ~(1 << objectLayer); } // take off layer
        objectCamera.cullingMask = 1 << objectLayer;

        _secondaryDepth = new RenderTexture(objectCamera.pixelWidth, objectCamera.pixelHeight, 16, RenderTextureFormat.Depth);
        objectCamera.SetTargetBuffers(_secondaryDepth.colorBuffer, _secondaryDepth.depthBuffer);
        Shader.SetGlobalTexture("_ObjectDepth", _secondaryDepth);

        if (setObjectLayer)
        {
            foreach(InteractableObject io in FindObjectsOfType<InteractableObject>())
            {
                io.gameObject.layer = objectLayer;
            }
        }       
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _effect);     
    }

    void OnApplicationQuit()
    {
        mainCamera.cullingMask = ~0; // add layer back
    }

    public void SwitchOutlineColor(PhysicsGun.Mode newMode)
    {
        Color c;
        colorKey.TryGetValue(newMode, out c);
        _effect.SetColor("_Outline", c == Color.clear ? Color.black : c);
    }
}

[Serializable]
public class TypeColor
{
    public PhysicsGun.Mode mode;
    public Color outlineColor;
}
