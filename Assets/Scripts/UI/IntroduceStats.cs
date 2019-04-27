using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroduceStats : MonoBehaviour
{
	
	public float Delay;

	Timer timer;

	
	public List<GameObject> stats = new List<GameObject>();
	public AudioSource audio;

	int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
		timer = new Timer(Delay);

	}

    // Update is called once per frame
    void Update()
    {
		if (timer.IsDone()) {
			if (counter > stats.Count - 1) {
				return;
			}

			stats[counter].SetActive(true);
			audio.Stop();
			audio.Play();
			
			counter++;

			timer.Start();
		} else {

		}


    }
}
