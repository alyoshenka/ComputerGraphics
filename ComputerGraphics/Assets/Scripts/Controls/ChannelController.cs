using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Controls RGB color channels using UI joystick
/// </summary>
public class ChannelController : MonoBehaviour
{
    public JoystickBall red, green, blue;
    public Material outline;
    [Range(0.01f, 0.5f)]
    public float stickMult;
    [Range(0.001f, 0.1f)]
    public float maxOffset = 0.01f;
    [Range(0.001f, 0.1f)]
    public float correctAllowance;

    // post processing
    PostProcessVolume m_Volume;
    CustomChromaticAberration m_ChromA;

    float[] offsets;

    // holders
    Vector3 holder;

    void Start()
    {
        outline.SetFloat("_OutlineThickness", 0.1f);

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

        offsets = new float[6];
    }

    void Update()
    {
        holder = red.Value() * stickMult;
        m_ChromA.RX.value = holder.x + offsets[0];
        m_ChromA.RY.value = holder.y + offsets[1];
        holder = green.Value() * stickMult;
        m_ChromA.GX.value = holder.x + offsets[2];
        m_ChromA.GY.value = holder.y + offsets[3];
        holder = blue.Value() * stickMult;
        m_ChromA.BX.value = holder.x + offsets[4];
        m_ChromA.BY.value = holder.y + offsets[5];

        if (WithinAllowance())
        {
            outline.SetFloat("_OutlineThickness", 0.5f);
        }
        else
        {
            outline.SetFloat("_OutlineThickness", 0.1f);
        }
    }

    // shuffle the color channel offsets
    public void ShuffleChannels()
    {
        // IEnumerator for them to look cool

        for(int i = 0; i < 6; i++)
        {
            offsets[i] = Random.Range(-maxOffset, maxOffset);
        }

        red.ResetPosition();
        green.ResetPosition();
        blue.ResetPosition();

        outline.SetFloat("_OutlineThickness", 0.1f);
    }

    bool WithinAllowance()
    {
        return Mathf.Abs(m_ChromA.RX - m_ChromA.GX) < correctAllowance
            && Mathf.Abs(m_ChromA.GX - m_ChromA.BX) < correctAllowance
            && Mathf.Abs(m_ChromA.RY - m_ChromA.GY) < correctAllowance
            && Mathf.Abs(m_ChromA.GY - m_ChromA.BY) < correctAllowance;
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
