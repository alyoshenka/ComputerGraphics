using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Fade : MonoBehaviour
{
    public bool triggerFade;
    bool prevTriggerFade;

    public float fadeTime;

    ColorGrading cg;
    PostProcessVolume m_Volume;

    // Start is called before the first frame update
    void Start()
    {
        prevTriggerFade = triggerFade;

        cg = ScriptableObject.CreateInstance<ColorGrading>();
        cg.enabled.Override(true);
        cg.colorFilter.Override(Color.white);
        cg.saturation.Override(0);
        cg.contrast.Override(0);

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, cg);
    }

    // Update is called once per frame
    void Update()
    {
        if(prevTriggerFade != triggerFade) // if triggered
        {
            if (triggerFade) // go out
            {
                StartCoroutine("FadeOut");
            }
            else // come back
            {
                StartCoroutine("FadeIn");
            }
        }
        prevTriggerFade = triggerFade;
    }

    IEnumerator FadeOut()
    {
        float t;
        for(float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            t = i / fadeTime;

            cg.colorFilter.Interp(Color.white, Color.black, t);
            cg.saturation.value = Mathf.Lerp(0, 50, t);
            cg.contrast.value = Mathf.Lerp(0, 50, t);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float t;
        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            t = i / fadeTime;
            cg.colorFilter.Interp(Color.black, Color.white, t);
            cg.saturation.value = Mathf.Lerp(50, 0, t);
            cg.contrast.value = Mathf.Lerp(50, 0, t);
            yield return null;
        }
    }

    void OnDestroy()
    {
        if (null != m_Volume)
        {
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        }
    }

}
