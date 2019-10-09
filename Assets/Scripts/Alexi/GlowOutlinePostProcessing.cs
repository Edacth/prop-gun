using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GlowOutlinePostProcessing : MonoBehaviour
{
    [Tooltip("Main scene view camera")]
    [SerializeField]
    Camera mainCamera;
    [Tooltip("Physics object view camera")]
    [SerializeField]
    Camera objectCamera;
    [Tooltip("Object layer number")]
    [SerializeField]
    int objectLayer = 11;
    [SerializeField]
    RenderTexture _objects;
    [SerializeField]
    Material _blur;
    [SerializeField]
    Material _combiner;

    Camera cam;

    void Start()
    {
        mainCamera.cullingMask &= ~(1 << objectLayer); // take off layer
        objectCamera.cullingMask = 1 << objectLayer;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture _temp = RenderTexture.GetTemporary(new RenderTextureDescriptor(_objects.width, _objects.height));
        Graphics.Blit(_objects, _temp, _blur);
        _combiner.SetTexture("_ObjectTex", _temp);
        Graphics.Blit(source, destination, _combiner);
        RenderTexture.ReleaseTemporary(_temp);
    }

    void OnApplicationQuit()
    {
        mainCamera.cullingMask = ~0; // add layer back
    }
}
