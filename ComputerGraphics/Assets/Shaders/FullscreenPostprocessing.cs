using UnityEngine;

public class FullscreenPostprocessing : MonoBehaviour
{
    [SerializeField]
    private Material postprocessMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postprocessMaterial);
    }
}
