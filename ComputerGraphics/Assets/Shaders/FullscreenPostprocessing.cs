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
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.DepthNormals;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // start fresh
        GL.Clear(false, true, Color.clear, 0);

        Graphics.Blit(source, destination, postprocessMaterial);
    }
}
