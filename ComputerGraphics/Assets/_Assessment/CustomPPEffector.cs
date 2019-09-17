using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CustomPPEffector : MonoBehaviour
{
    PostProcessVolume m_Volume;
    CustomPostProcess m_ChromA;

    // Start is called before the first frame update
    void Start()
    {
        m_ChromA = ScriptableObject.CreateInstance<CustomPostProcess>();
        m_ChromA.enabled.Override(true);
        m_ChromA.offsetX.Override(0);
        m_ChromA.offsetY.Override(0);
        m_ChromA.offsetZ.Override(0);
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_ChromA);
    }

    // Update is called once per frame
    void Update()
    {
        m_ChromA.offsetX.value = Mathf.Sin(Time.realtimeSinceStartup);
        Debug.Log(m_ChromA.offsetX.value);
    }

    void OnDestroy()
    {
        if (null != m_Volume)
        {
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        }
    }
}
