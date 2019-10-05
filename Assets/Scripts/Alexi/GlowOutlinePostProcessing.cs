using UnityEngine;

public class GlowOutlinePostProcessing : MonoBehaviour
{
    [SerializeField]
    Shader _shader;

    Material _material;
    RenderTexture[] _rts;
    RenderBuffer[] _mrt;

   void OnEnable()
   {
        _material = new Material(_shader);
        _material.hideFlags = HideFlags.DontSave;
        _mrt = new RenderBuffer[2];
   }

    void OnDisable()
    {
        DestroyImmediate(_material);
        _material = null;
        _mrt = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var rt1 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default);
        var rt2 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default);

        _mrt[0] = rt1.colorBuffer;
        _mrt[1] = rt2.colorBuffer;

        Graphics.SetRenderTarget(_mrt, rt1.depthBuffer);
        Graphics.Blit(null, _material, 0);

        _material.SetTexture("_ObjectTex", rt1);

        Graphics.Blit(source, destination, _material, 1);

        RenderTexture.ReleaseTemporary(rt1);
        RenderTexture.ReleaseTemporary(rt2);
    }
}
