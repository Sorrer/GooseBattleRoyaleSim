using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
	

	public ControlPlayer controlPlayer;
	public Animator ani;

	public GooseRagdoll ragdoll;
	public DamageSystem damageSystem;
	public DamageTrigger biteTrigger;
	public GooseHonkSphereEmitter honkEmitter;
	Timer deathTimer;

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

		if (damageSystem.IsDead) {
			if(deathTimer == null) {
				ragdoll.EnableRagdoll = true;
				deathTimer = new Timer(5);
				deathTimer.Start();
			} else if(deathTimer.IsDone()){
				deathTimer = null;
				ragdoll.EnableRagdoll = false;
				damageSystem.HP = 101;
				damageSystem.IsDead = false;
			}
		}

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


			if(!honkEmitter.Emit)
			honkEmitter.StartEmit();
		}
		if (Input.GetMouseButtonUp(1)) {
			ani.SetTrigger("StopHonk");
			if (honkEmitter.Emit)
			honkEmitter.StopEmit();
		}

		bool isBiting = ani.GetCurrentAnimatorStateInfo(1).IsName("Bite");
		if (isBiting && !this.biteTrigger.EnableDamage) {
			this.biteTrigger.StartDamage();
		} else if(!isBiting && this.biteTrigger.EnableDamage) {
			this.biteTrigger.StopDamage();
		}

	}


	public void setBaseComponents() {
		if (foundAllBaseComponents) return;

		GetComponent<ControlPlayer>();


	}
}
