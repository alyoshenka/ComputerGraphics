using UnityEngine;

[ExecuteInEditMode]
public class EnableDepthWrite : MonoBehaviour
{
    void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
}
