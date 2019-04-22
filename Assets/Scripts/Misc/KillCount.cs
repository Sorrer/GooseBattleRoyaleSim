using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{
	public TextMeshProUGUI EventText;
	public TextMeshProUGUI text;

	int lastKills = GameStats.Kills;

	Timer killLeader = null;
	string lastEvenText = "";
    void Update()
    {
		text.text = "Killed: " + GameStats.Kills;        

		if(GameStats.Kills > lastKills) {
			lastEvenText = "You are the new kill leader - " + GameStats.Kills + " kills";
			EventText.text = lastEvenText;

			killLeader = new Timer(2);
		}

		if(killLeader != null && killLeader.IsDone()) {
			if (EventText.text.Equals(lastEvenText)) {
				EventText.text = "";
			}

			killLeader = null;
		}

		lastKills = GameStats.Kills;
    }
}
