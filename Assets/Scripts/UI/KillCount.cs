using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{
	public TextMeshProUGUI EventText;
	public TextMeshProUGUI text;

	int lastKills = GameStats.Kills;
	
    void Update()
    {
		text.text = "Killed: " + GameStats.Kills;        

		if(GameStats.Kills > lastKills) {

			if(GameStats.Kills < 2) {
				GlobalGame.eventText.ApplyText("You are the new kill leader with " + GameStats.Kills + " kill", 2);
			} else {
				GlobalGame.eventText.ApplyText("You are the new kill leader with " + GameStats.Kills + " kills", 2);
			}
			
		}

		

		lastKills = GameStats.Kills;
    }
}
