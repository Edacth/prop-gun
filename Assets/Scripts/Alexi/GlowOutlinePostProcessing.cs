using UnityEngine;

public class GlowOutlinePostProcessing : MonoBehaviour
{
    [SerializeField]
    Material readMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, readMaterial);
    }
}
