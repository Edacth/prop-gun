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
    [Tooltip("Object layer manipulation")]
    [SerializeField] Material _effect;
    [Tooltip("HIde objects from main camera?")]
    [SerializeField] bool hideObjects = false;
    [Tooltip("Outline colors for different modes")]
    [SerializeField] List<TypeColor> typeColors;

    RenderTexture _secondaryDepth;
    Dictionary<PhysicsGun.Mode, Color> colorKey;

    void Awake()
    {
        colorKey = new Dictionary<PhysicsGun.Mode, Color>();
        foreach (TypeColor tc in typeColors) { colorKey.Add(tc.mode, tc.outlineColor); }
    }

    void Start()
    {
        LayerMask mask = (1 << PhysicsValues.instance.objectOutlineLayer) | (1 << PhysicsValues.instance.objectGoThruLayer);
        if (hideObjects) { mainCamera.cullingMask &= ~mask; } // take off layer
        objectCamera.cullingMask = mask;

        _secondaryDepth = new RenderTexture(objectCamera.pixelWidth, objectCamera.pixelHeight, 16, RenderTextureFormat.Depth);
        objectCamera.SetTargetBuffers(_secondaryDepth.colorBuffer, _secondaryDepth.depthBuffer);
        Shader.SetGlobalTexture("_ObjectDepth", _secondaryDepth);      
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
