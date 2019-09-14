using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CustomPostProcessRenderer), PostProcessEvent.AfterStack, "Custom/Test1", true)]
public sealed class CustomPostProcess : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

    [Range(-0.01f, 0.01f)]
    public FloatParameter offsetX = new FloatParameter { value = 0 };
    [Range(-0.01f, 0.01f)]
    public FloatParameter offsetY = new FloatParameter { value = 0 };
    [Range(-0.01f, 0.01f)]
    public FloatParameter offsetZ = new FloatParameter { value = 0 };

}

public sealed class CustomPostProcessRenderer : PostProcessEffectRenderer<CustomPostProcess>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("PostProcess/Test1"));
        sheet.properties.SetFloat("_Blend", settings.blend);

        sheet.properties.SetFloat("_OffsetX", settings.offsetX);
        sheet.properties.SetFloat("_OffsetY", settings.offsetY);
        sheet.properties.SetFloat("_OffsetZ", settings.offsetZ);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}

// https://github.com/Unity-Technologies/PostProcessing/wiki/Writing-Custom-Effects
