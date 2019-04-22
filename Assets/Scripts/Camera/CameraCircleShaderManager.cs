using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraCircleShaderManager : MonoBehaviour
{
	public Transform CameraPos;

	public PostProcessProfile profile;

	// Update is called once per frame
	void Update()
    {

		Grayscale effect;

		if(!profile.TryGetSettings<Grayscale>(out effect)) {
			return;
		}


		if (Vector3.Distance(CameraPos.position, GlobalGame.CircleCenter) >= GlobalGame.CircleRadius) {

			effect.enabled.value = true;

		} else {

			effect.enabled.value = false;

		}



    }
}
