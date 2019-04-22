using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventText : MonoBehaviour
{
	bool Applied = false;

	Timer timer;


	public TextMeshProUGUI textEle;

	private void Start() {
		GlobalGame.eventText = this;
	}

	public void ApplyText(string text, float time) {
		Applied = true;
		textEle.text = text;
		timer = new Timer(time);
		timer.Start();
	}

    // Update is called once per frame
    void Update()
    {
		if (Applied) {
			if (timer.IsDone()) {
				textEle.text = "";
				Applied = false;
				timer = null;
			}
		}
    }
}
