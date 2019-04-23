using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
	

	public ControlPlayer controlPlayer;
	public Animator ani;

	public AttachTransform bus;
	public AttachTransform GooseBody;
	public GooseRagdoll GooseBodyDoll;

	public GooseRagdoll ragdoll;
	public DamageSystem damageSystem;
	public DamageTrigger biteTrigger;
	public GooseHonkSphereEmitter honkEmitter;
	Timer deathTimer,fallinTimer;

	public bool Droppin = false;
	[HideInInspector]
	public bool falling = false;
	// Start is called before the first frame update
	void Start()
    {
		GlobalGame.playerMain = this;
		setBaseComponents();
		//ani = this.GetComponent<Animator>();
	}

	bool foundAllBaseComponents = false;
	bool flappedWings = false;

    // Update is called once per frame
    void Update()
    {
		if (Droppin) {

			if (falling) {
				if (fallinTimer.IsDone()) {
					Droppin = false;
					falling = false;
					bus.Transforms.Remove(this.transform);

					controlPlayer.enabled = true;
					Vector3 pos = GooseBody.transform.position;
					//print(pos);
					pos.y = 1.105f;
					controlPlayer.playerController.enabled = false;
					this.transform.position = pos;
					controlPlayer.playerController.enabled = true;
					Destroy(GooseBody.transform.parent.gameObject);
					this.GetComponent<AttachTransform>().Transforms.Add(controlPlayer.CameraPos);
					return;
				}
			} else {

				controlPlayer.enabled = false;
			}


			if ((Input.GetKeyDown(KeyCode.Space) || GlobalGame.ForceDrop) && !falling) {
				controlPlayer.cameraMovement.ZoomPercentage = 0.25f;
				bus.Transforms.Remove(this.GooseBody.transform.parent);
				this.GetComponent<AttachTransform>().Transforms.Remove(controlPlayer.CameraPos);
				GooseBody.Transforms.Add(controlPlayer.CameraPos);
				falling = true;
				fallinTimer = new Timer(10);
				fallinTimer.Start();
				controlPlayer.enabled = false;
				GooseBodyDoll.EnableRagdoll = true;
			}

			return;
		}


		if (damageSystem.IsDead) {
			if(deathTimer == null) {
				ragdoll.EnableRagdoll = true;
				deathTimer = new Timer(5);
				deathTimer.Start();
				controlPlayer.enabled = false;
			} else if(deathTimer.IsDone()){
				deathTimer = null;
				ragdoll.EnableRagdoll = false;
				damageSystem.HP = 101;
				damageSystem.IsDead = false;

				controlPlayer.enabled = true;
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
			ani.SetBool("HONK", true);


			if(!honkEmitter.Emit)
			honkEmitter.StartEmit();
		}
		if (Input.GetMouseButtonUp(1)) {
			ani.SetBool("HONK", false);
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
