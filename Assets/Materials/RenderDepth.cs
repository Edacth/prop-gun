using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderDepth : MonoBehaviour
{
    public Camera targetCamera;
    public DepthTextureMode depthTextureMode;
    [HideInInspector]
    public Material _depthTextureMaterial;
    public Material depthTextureMaterial
    {
        get
        {
            if (_depthTextureMaterial == null)
            {
                _depthTextureMaterial = new Material(Shader.Find("Debug/DepthTexture"));
                _depthTextureMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return _depthTextureMaterial;
        }
    }

    private RenderTexture depthTextureRT;
    private Texture2D displayTexture;

    void Start()
    {
        depthTextureRT = new RenderTexture(Screen.width, Screen.height, 0);
        displayTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false); // TODO
    }

    void OnEnable()
    {
        targetCamera.depthTextureMode = depthTextureMode;
        targetCamera.targetTexture = depthTextureRT;
    }

    void OnDisable()
    {
        targetCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    void OnDestroy()
    {
        Destroy(depthTextureRT);
        Destroy(displayTexture);
    }

    void Reset()
    {
        targetCamera = GetComponent<Camera>();
    }

}
