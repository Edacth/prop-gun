using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GlowOutlinePostProcessing : MonoBehaviour
{
    [SerializeField]
    RenderTexture _objects;
    [SerializeField]
    Material _combiner;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Object")); // take off layer
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _combiner.SetTexture("_ObjectTex", _objects);
        Graphics.Blit(source, destination, _combiner);
    }
}
