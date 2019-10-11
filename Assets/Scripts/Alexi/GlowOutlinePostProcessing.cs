using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GlowOutlinePostProcessing : MonoBehaviour
{
    [Tooltip("Main scene view camera")]
    [SerializeField] Camera mainCamera;
    [Tooltip("Physics object view camera")]
    [SerializeField] Camera objectCamera;
    [Tooltip("Object layer number")]
    [SerializeField] int objectLayer = 11;
    // [Tooltip("RenderTexture for objects")]
    // [SerializeField] RenderTexture _objects;
    [Tooltip("Object layer manipulation")]
    [SerializeField] Material _effect;
    // [Tooltip("Layering combine material")]
    // [SerializeField]  Material _combiner;
    [Tooltip("HIde objects from main camera?")]
    [SerializeField] bool hideObjects = false;

    [Header("Setup")]
    [Tooltip("Find objects and move them to object layer?")]
    [SerializeField] bool setObjectLayer = false;

    RenderTexture _secondaryDepth;

    public static int ObjectLayer { get; private set; }

    void Awake()
    {
        ObjectLayer = objectLayer;
    }

    void Start()
    {
        if (hideObjects) { mainCamera.cullingMask &= ~(1 << objectLayer); } // take off layer
        objectCamera.cullingMask = 1 << objectLayer;

        // for some reason runningwith this enabled once fixed the smearing
        // problem, despite mode always being set to solidcolor in editor
        //objectCamera.clearFlags = CameraClearFlags.SolidColor;

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
        // blur
        //RenderTexture _hori = RenderTexture.GetTemporary(new RenderTextureDescriptor(_objects.width, _objects.height));
        //RenderTexture _vert = RenderTexture.GetTemporary(new RenderTextureDescriptor(_objects.width, _objects.height));
        //Graphics.Blit(_objects, _vert, _blur, 0);
        //Graphics.Blit(_vert, _hori, _blur, 1);
        //_combiner.SetTexture("_ObjectTex", _hori);
        //Graphics.Blit(source, destination, _combiner);
        //RenderTexture.ReleaseTemporary(_hori);
        //RenderTexture.ReleaseTemporary(_vert);

        Graphics.Blit(source, destination, _effect);
      
    }

    void OnApplicationQuit()
    {
        mainCamera.cullingMask = ~0; // add layer back
    }
}
