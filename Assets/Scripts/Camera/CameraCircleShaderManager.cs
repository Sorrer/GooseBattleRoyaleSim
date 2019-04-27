using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraCircleShaderManager : MonoBehaviour
{
	public Transform CameraPos;

	public PostProcessProfile profile;
	public float padding;

	public float flip;
	public Color up;
	public Color down;


	// Update is called once per frame
	void Update()
    {

		Grayscale effect;

		if(!profile.TryGetSettings<Grayscale>(out effect)) {

			Grayscale scale = ScriptableObject.CreateInstance<Grayscale>();
			scale.blend.Override(flip);
			scale.UPcolor.Override(up);
			scale.DOWNcolor.Override(down);

			profile.AddSettings(scale);

			ConsoleLogger.debug("CANT FIND GRAYSCALE");
			return;
		}


		if (Vector2.Distance(new Vector2(CameraPos.position.x, CameraPos.position.z), new Vector2(GlobalGame.CircleCenter.x, GlobalGame.CircleCenter.z)) - padding >= GlobalGame.CircleRadius) {

			effect.enabled.Override(true);

		} else {

			effect.enabled.Override(false);

		}



    }
}
