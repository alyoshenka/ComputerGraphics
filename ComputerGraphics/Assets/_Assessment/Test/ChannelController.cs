using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChannelController : MonoBehaviour
{
    public JoystickBall red, green, blue;
    [Range(0.001f, 0.05f)]
    public float maxOffset = 0.01f;

    // post processing
    PostProcessVolume m_Volume;
    CustomChromaticAberration m_ChromA;

    void Start()
    {
        // initialize post processing 
        m_ChromA = ScriptableObject.CreateInstance<CustomChromaticAberration>();
        m_ChromA.enabled.Override(true);
        m_ChromA.RX.Override(0);
        m_ChromA.RY.Override(0);
        m_ChromA.GX.Override(0);
        m_ChromA.GY.Override(0);
        m_ChromA.BX.Override(0);
        m_ChromA.BY.Override(0);

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_ChromA);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShuffleChannels();
            Debug.Log(m_ChromA.RX.value);
        }
    }

    // shuffle the color channel offsets
    public void ShuffleChannels()
    {
        // IEnumerator for them to look cool

        m_ChromA.RX.value = Random.Range(-maxOffset, maxOffset);
        m_ChromA.RY.value = Random.Range(-maxOffset, maxOffset);
        m_ChromA.GX.value = Random.Range(-maxOffset, maxOffset);
        m_ChromA.GY.value = Random.Range(-maxOffset, maxOffset);
        m_ChromA.BX.value = Random.Range(-maxOffset, maxOffset);
        m_ChromA.BY.value = Random.Range(-maxOffset, maxOffset);
    }

    // clean up post processing
    void OnDestroy()
    {
        if (null != m_Volume)
        {
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        }
    }
}
