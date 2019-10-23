using UnityEngine;

[ExecuteInEditMode]
public class FullscreenPostprocessing : MonoBehaviour
{
    [SerializeField]
    private Material postprocessMaterial;

    void Start()
    {
        Camera cam;
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.DepthNormals;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postprocessMaterial);
    }
}
