using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventText : MonoBehaviour
{
	bool Applied = false;

	Timer timer;


	bool priority = false;
	Color translateTo;
	bool colorTranslate = false;

	public TextMeshProUGUI textEle;

	private void Start() {
		GlobalGame.eventText = this;
	}


	public void PriorityText(string text, float time) {
		ApplyText(text, time);
		priority = true;

	}
	public void PriorityTextMajor(string text, float time) {
		ApplyText(text, time);
		textEle.color = Random.ColorHSV();
		priority = true;
		colorTranslate = true;

	}

	public void ApplyText(string text, float time) {
		if (priority) return;

		Applied = true;
		textEle.text = text;
		timer = new Timer(time);
		timer.Start();
	}

    // Update is called once per frame
    void Update()
    {
		if (Applied) {

			if (colorTranslate) {
				Vector3 RGBTranslate = new Vector3(translateTo.r, translateTo.g, translateTo.b);
				Vector3 RGBCur = new Vector3(textEle.color.r, textEle.color.g, textEle.color.b);

				if(Vector3.Distance (RGBCur, RGBTranslate) <= 0.01) {

				}

			}


			if (timer.IsDone()) {
				textEle.color = Color.white;
				textEle.text = "";
				Applied = false;
				timer = null;
				priority = false;

				colorTranslate = false;
			}
		}
    }
}
