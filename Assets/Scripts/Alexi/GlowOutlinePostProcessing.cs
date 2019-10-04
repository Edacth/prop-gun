using UnityEngine;

public class GlowOutlinePostProcessing : MonoBehaviour
{
    [SerializeField]
    Material postprocessMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postprocessMaterial);
    }

    private void OnPostRender()
    {
        
    }
}
