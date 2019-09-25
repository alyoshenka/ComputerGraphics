using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  controls particle system emission from UI
/// </summary>
public class RainControl : MonoBehaviour
{
    ParticleSystem rainSystem;
    public UnityEngine.UI.Slider rainSlider;
    public float minRain, maxRain;

    ParticleSystem.EmissionModule rainEmitter;

    void Start()
    {
        rainSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        rainEmitter = rainSystem.emission;
        rainEmitter.rateOverTime = Mathf.Lerp(minRain, maxRain, rainSlider.value);
    }
}
