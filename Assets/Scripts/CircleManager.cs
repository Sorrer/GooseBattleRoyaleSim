using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CircleManager : MonoBehaviour
{

	public Transform Circle;
	
	[SerializeField]
	public List<CircleSegment> circleSegment = new List<CircleSegment>();
	Timer segmentTimer;

	public bool waitingToClose = false;

	Vector3 maxScale;
	float maxRadi;

	public TextMeshProUGUI text;

    void Start()
    {
		waitingToClose = true;
		segmentTimer = new Timer(circleSegment[0].SecondsTillClose);
		segmentTimer.Start();

		maxRadi = GlobalGame.CircleRadius;
		maxScale = Circle.localScale;
	}
	
    void Update()
    {

		if (circleSegment.Count == 0) return;

		CircleSegment curSegment = circleSegment[0];


		if (waitingToClose) {
			text.text = FormatSeconds(curSegment.SecondsTillClose - segmentTimer.GetProgressTime());
			if (segmentTimer.IsDone()) {
				GlobalGame.eventText.ApplyText("Closing Circle", 4);

				segmentTimer.Start(curSegment.ClosingTime);
				waitingToClose = false;
			}
		} else {
			text.text = FormatSeconds(curSegment.ClosingTime - segmentTimer.GetProgressTime());
			float newScaleX = (((maxRadi - (segmentTimer.GetProgress() * (maxRadi -  curSegment.CloseRadiTo))) * maxScale.x )/ maxRadi);

			Circle.localScale = new Vector3(newScaleX, newScaleX, maxScale.z);

			GlobalGame.CircleRadius = maxRadi - (segmentTimer.GetProgress() * (maxRadi - curSegment.CloseRadiTo));

			if (segmentTimer.IsDone()) {

				print(segmentTimer.IsDone() + " " + segmentTimer.countTo);

				 newScaleX = ((curSegment.CloseRadiTo) * maxScale.x) / maxRadi;

				Circle.localScale = new Vector3(newScaleX, newScaleX, maxScale.z);

				GlobalGame.CircleRadius = curSegment.CloseRadiTo;
				print(curSegment.CloseRadiTo);


				circleSegment.RemoveAt(0);
				if (circleSegment.Count == 0) return;
				curSegment = circleSegment[0];

				GlobalGame.CircleDamage = curSegment.CircleDamage;
				GlobalGame.CircleDamageTick = curSegment.CircleDamageTick;


				GlobalGame.eventText.ApplyText("Next Closing in " + FormatSeconds(curSegment.SecondsTillClose), 4);
				segmentTimer.Start(curSegment.SecondsTillClose);
				waitingToClose = true;

				maxRadi = GlobalGame.CircleRadius;
				maxScale = Circle.localScale;


			}
		}
	}

	public string FormatSeconds(float seconds) {
		int minutes = Mathf.FloorToInt(seconds/60);
		string strMinutes = minutes + "";

		if (minutes < 10) {
			strMinutes = "0" + minutes;
		}

		seconds = Mathf.FloorToInt(seconds % 60);
		string strSeconds = seconds + "";
		if (seconds < 10) {
			 strSeconds = "0" + seconds;
		}

		return strMinutes + " : " + strSeconds;
	}

}


[System.Serializable]
public class CircleSegment {
	public float SecondsTillClose;
	public float ClosingTime;
	public float CloseRadiTo;
	public float CircleDamage;
	public float CircleDamageTick;
}