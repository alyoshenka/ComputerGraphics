using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CustomChromaticAberrationRenderer), PostProcessEvent.AfterStack, "PostProcess/CustomChromaticAberration", false)]
public sealed class CustomChromaticAberration : PostProcessEffectSettings
{
    [Range(-0.01f, 0.01f)]
    public FloatParameter RX = new FloatParameter { value = 0 };
    [Range(-0.01f, 0.01f)]
    public FloatParameter RY = new FloatParameter { value = 0 };
    [Range(-0.01f, 0.01f)]
    public FloatParameter GX = new FloatParameter { value = 0 };
    [Range(-0.01f, 0.01f)]
    public FloatParameter GY = new FloatParameter { value = 0 };
    [Range(-0.01f, 0.01f)]
    public FloatParameter BX = new FloatParameter { value = 0 };
    [Range(-0.01f, 0.01f)]
    public FloatParameter BY = new FloatParameter { value = 0 };

    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value;
    }
}

public sealed class CustomChromaticAberrationRenderer : PostProcessEffectRenderer<CustomChromaticAberration>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("PostProcess/CustomChromaticAberration"));

        sheet.properties.SetFloat("_RX", settings.RX);
        sheet.properties.SetFloat("_RY", settings.RY);
        sheet.properties.SetFloat("_GX", settings.GX);
        sheet.properties.SetFloat("_GY", settings.GY);
        sheet.properties.SetFloat("_BX", settings.BX);
        sheet.properties.SetFloat("_BY", settings.BY);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}

// https://github.com/Unity-Technologies/PostProcessing/wiki/Writing-Custom-Effects