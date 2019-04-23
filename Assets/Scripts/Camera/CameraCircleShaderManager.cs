using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraCircleShaderManager : MonoBehaviour
{
	public Transform CameraPos;

	public PostProcessProfile profile;
	public float padding;
	// Update is called once per frame
	void Update()
    {

		Grayscale effect;

		if(!profile.TryGetSettings<Grayscale>(out effect)) {
			return;
		}


		if (Vector2.Distance(new Vector2(CameraPos.position.x, CameraPos.position.z), new Vector2(GlobalGame.CircleCenter.x, GlobalGame.CircleCenter.z)) - padding >= GlobalGame.CircleRadius) {

			effect.enabled.value = true;

		} else {

			effect.enabled.value = false;

		}



    }
}
