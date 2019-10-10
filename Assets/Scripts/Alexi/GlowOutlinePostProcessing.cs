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
    [Tooltip("RenderTexture for objects")]
    [SerializeField] RenderTexture _objects;
    [Tooltip("Object effect material")]
    [SerializeField] Material _blur;
    [Tooltip("Layering combine material")]
    [SerializeField]  Material _combiner;

    [Header("Setup")]
    [Tooltip("Find objects and move them to object layer?")]
    [SerializeField] bool setObjectLayer = false;

    void Start()
    {
        mainCamera.cullingMask &= ~(1 << objectLayer); // take off layer
        objectCamera.cullingMask = 1 << objectLayer;

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
        RenderTexture _hori = RenderTexture.GetTemporary(new RenderTextureDescriptor(_objects.width, _objects.height));
        RenderTexture _vert = RenderTexture.GetTemporary(new RenderTextureDescriptor(_objects.width, _objects.height));
        Graphics.Blit(_objects, _vert, _blur, 0);
        Graphics.Blit(_vert, _hori, _blur, 1);
        _combiner.SetTexture("_ObjectTex", _hori);
        Graphics.Blit(source, destination, _combiner);
        RenderTexture.ReleaseTemporary(_hori);
        RenderTexture.ReleaseTemporary(_vert);
    }

    void OnApplicationQuit()
    {
        mainCamera.cullingMask = ~0; // add layer back
    }
}
