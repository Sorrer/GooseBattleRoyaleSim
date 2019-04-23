using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThankBusDriver : MonoBehaviour
{

	Timer textTimer;
	public TextMeshProUGUI DroppingText;



	void Update()
    {

		if(textTimer == null) {
			if (Input.GetKeyDown(KeyCode.B)) {
				GlobalGame.eventText.ApplyText("You thanked bus driver. Nice. (+10 Kills)", 4);
				GameStats.ThankedBusDriver = true;
				GameStats.Kills += 10;
				DroppingText.text = "";
				Destroy(this);
			}

			if (GlobalGame.playerMain.falling) {
				GlobalGame.eventText.ApplyText("Didnt thank the bus driver. (-1 Kill)", 5);
				GameStats.Kills--;
				DroppingText.text = "";
				Destroy(this);
			}
		}
    }
}
