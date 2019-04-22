using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GrayscaleRenderer), PostProcessEvent.AfterStack, "Custom/ScreenColored")]
public sealed class Grayscale : PostProcessEffectSettings {
	[Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
	public FloatParameter blend = new FloatParameter { value = 0.5f };

	public ColorParameter UPcolor = new ColorParameter { };
	public ColorParameter DOWNcolor = new ColorParameter { };
}

public sealed class GrayscaleRenderer : PostProcessEffectRenderer<Grayscale> {
	public override void Render(PostProcessRenderContext context) {
		var sheet = context.propertySheets.Get(Shader.Find("Custom/ScreenColored"));


		sheet.properties.SetFloat("_FlipMidpoint", settings.blend);
		sheet.properties.SetColor("_FlipUpColor", settings.UPcolor);
		sheet.properties.SetColor("_FlipDownColor", settings.DOWNcolor);


		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}