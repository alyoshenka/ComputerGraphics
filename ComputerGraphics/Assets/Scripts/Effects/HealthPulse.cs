using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Apply cosmetic effects on low health
/// </summary>
public class HealthPulse : MonoBehaviour
{
    [Header("Health vars")]

    [Range(0, 100)]
    public float health;
    public float heartBeatSpeed;

    // postprocesing
    PostProcessVolume m_Volume;

    [Header("Health effect vals")]
    Vignette m_Vignette;
    [Range(0, 100)]
    public int vignetteBegin;
    [Range(0, 1)]
    public float vignetteMin = 0f;
    [Range(0, 1)]
    public float vignetteMax = 1f;
    public float vignetteOsc;

    ColorGrading m_ColorGrading;
    [Range(0, 100)]
    public int cgBegin;
    [Range(0, 1)]
    public float cgMin = 0f;
    [Range(0, -100)]
    public float cgMax = -100f;

    Bloom m_Bloom;
    [Range(0, 100)]
    public int bloomBegin;
    [Range(0, 1)]
    public float bloomMin = 0f;
    [Range(0, 100)]
    public float bloomMax = 100f;

    DepthOfField m_DepthOfFeild;
    [Range(0, 100)]
    public int dofBegin;
    public float focDistMin = 10f;
    public float focDistMax = 0.1f;

    [Header("Settings vars")]
    public float vignetteSmoothness = 0.3f;
    public float bloomDiffusion = 5f;

    void Start()
    {
        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        m_Vignette.intensity.Override(0f);

        m_ColorGrading = ScriptableObject.CreateInstance<ColorGrading>();
        m_ColorGrading.enabled.Override(true);
        m_ColorGrading.saturation.Override(0f);

        m_Bloom = ScriptableObject.CreateInstance<Bloom>();
        m_Bloom.enabled.Override(true);
        m_Bloom.diffusion.Override(bloomDiffusion);
        m_Bloom.intensity.Override(0f);

        m_DepthOfFeild = ScriptableObject.CreateInstance<DepthOfField>();
        m_DepthOfFeild.enabled.Override(true);
        m_DepthOfFeild.focusDistance.Override(focDistMin);

        PostProcessEffectSettings[] vals = { m_Vignette, m_ColorGrading, m_Bloom, m_DepthOfFeild };
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, vals);
    }

    void Update()
    {
        m_Vignette.intensity.value = Mathf.Lerp(vignetteMin, vignetteMax, Mathf.InverseLerp(vignetteBegin, 0, health));
        m_ColorGrading.saturation.value = Mathf.Lerp(cgMin, cgMax, Mathf.InverseLerp(cgBegin, 0, health));
        m_Bloom.intensity.value = Mathf.Lerp(bloomMin, bloomMax, Mathf.InverseLerp(bloomBegin, 0, health));
        m_DepthOfFeild.focusDistance.value = Mathf.Lerp(focDistMin, focDistMax, Mathf.InverseLerp(dofBegin, 0, health));

        health = Mathf.Clamp(health, 0, 100);
    }

    void OnDestroy()
    {
        if (null != m_Volume)
        {
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        }
    }
}
