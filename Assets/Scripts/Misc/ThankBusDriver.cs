using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThankBusDriver : MonoBehaviour
{

	Timer textTimer;
	public TextMeshProUGUI text;
	public TextMeshProUGUI DroppingText;



	void Update()
    {

		if(textTimer == null) {
			if (Input.GetKeyDown(KeyCode.B)) {
				textTimer = new Timer(4);
				GameStats.ThankedBusDriver = true;
				text.text = "You thanked bus driver. Nice. (+10 Kills)";
				GameStats.Kills += 10;
				DroppingText.text = "";

			}

			if (GlobalGame.Player.falling) {
				textTimer = new Timer(5);
				text.text = "Didn't thank the bus driver. (-1 Kill)";
				GameStats.Kills--;
				DroppingText.text = "";
			}
		} else {
			if (textTimer.IsDone()) {
				text.text = "";
				Destroy(this);
			}
		}
    }
}
