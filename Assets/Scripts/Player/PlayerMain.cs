using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
	

	public ControlPlayer controlPlayer;
	public Animator ani;
	// Start is called before the first frame update
	void Start()
    {
		GlobalGame.Player = this;
		setBaseComponents();
		//ani = this.GetComponent<Animator>();
	}

	bool foundAllBaseComponents = false;
	bool flappedWings = false;

    // Update is called once per frame
    void Update()
    {
		if (!foundAllBaseComponents) {
			setBaseComponents();
		}

		ani.SetBool("Walking", controlPlayer.isWalking);

		if (controlPlayer.isDashing && !flappedWings) {
			flappedWings = true;
			ani.SetTrigger("FlapWings");
		}else if (!controlPlayer.isDashing && flappedWings) {
			flappedWings = false;
		}


		//Control Attacks

		//Bite
		if (Input.GetMouseButtonDown(0)) {
			ani.ResetTrigger("Bite");
			ani.SetTrigger("Bite");
		}

		//Honk
		if (Input.GetMouseButtonDown(1)) {
			ani.SetTrigger("StartHonk");
		}
		if (Input.GetMouseButtonUp(1)) {
			ani.SetTrigger("StopHonk");
		}


	}


	public void setBaseComponents() {
		if (foundAllBaseComponents) return;

		GetComponent<ControlPlayer>();


	}
}
