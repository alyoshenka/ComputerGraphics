using UnityEngine;

[ExecuteInEditMode]
public class FullscreenPostprocessing : MonoBehaviour
{
    [SerializeField] Material postprocessMaterial;
    [SerializeField] Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.DepthNormals;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // projection matrix = view to clip
        // invert to reverse
        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(cam.projectionMatrix, true).inverse;
        postprocessMaterial.SetMatrix("_ClipToView", clipToView);

        Graphics.Blit(source, destination, postprocessMaterial);
    }
}
