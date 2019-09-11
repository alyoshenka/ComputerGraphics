using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurPulse : MonoBehaviour
{
    PostProcessVolume m_Volume;
    MotionBlur m_Blur;

    void Start()
    {
        m_Blur = ScriptableObject.CreateInstance<MotionBlur>();
        m_Blur.enabled.Override(true);
        m_Blur.shutterAngle.Override(300);
        m_Blur.sampleCount.Override(10);

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Blur);
    }

    void Update()
    {
       //  m_Blur.shutterAngle.value = (int)(Mathf.Sin(Time.realtimeSinceStartup) * 300);
    }

    void OnDestroy()
    {
        if (null != m_Volume)
        {
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        }
    }
}
