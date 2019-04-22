using System.Collections.Generic;
using UnityEngine;

public class GooseBehavious : MonoBehaviour {
	public SimpleNavigation navEle;
	public SphereCollider sC;
	public GameObject thisChild;

	public static List<GameObject> deadList = new List<GameObject>();
	public List<GameObject> gooseInRange = new List<GameObject>();

	public Animator ani;

	public GooseRagdoll ragdoll;

	public DamageSystem damageSystem;
	public DamageTrigger biteTrigger;
	public GooseHonkSphereEmitter honkEmitter;

	private bool walking = false;
	private bool attack = false;

	private float timer = 1;
	private float currentTime = 0;

	private bool focused = false;
	public GameObject focusedObject = null;

	private bool wander = false;
	private Vector3 wanderLoc = Vector3.zero;

	private DamageSystem enemyDS = null;

	// Start is called before the first frame update
	void Start() {
		thisChild = gameObject.transform.GetChild(0).gameObject;
	}

	// Update is called once per frame
	void Update() {
		if (!damageSystem.IsDead) {
			IfEnemyDead();

			

			if (!focused || focusedObject == null) {
				//Debug.Log("Finding new Goose");
				SetAttackGoose();

				if((!focused || focusedObject == null) && !wander) {
					SetWander(true);
				}
			}

			if (focusedObject != null) {
				SetWander(false);
				//Debug.Log(gameObject.name + "setting destination to: " + focusedObject.name);
				navEle.thisAgent.SetDestination(focusedObject.transform.position);
			}

			if (wander) {
				if (!walking) {
					FindWanderLoc();
				}

				navEle.thisAgent.SetDestination(wanderLoc);
			}

			float dist = Mathf.Abs((navEle.thisAgent.destination - transform.position).magnitude);
			walking = (dist >= navEle.thisAgent.stoppingDistance);
			attack = !walking && focused;
			AnimationsAndActions();

			

		} else {
			Kill();
		}
	}

	private void IfEnemyDead() {
		if (enemyDS != null) {
			if (enemyDS.IsDead) {
				//Debug.Log("My " + gameObject.name + " enemy: " + focusedObject.name + " is DEAD");
				Unfocous();
				gooseInRange.RemoveAt(0);
			}
		}
	}

	private void UpdateGooseInRange() {
		for (int i = 0; i < gooseInRange.Count; i++) {
			if (deadList.Contains(gooseInRange[i])) {
				gooseInRange.RemoveAt(i);
			}
		}
	}

	private void SetAttackGoose() {
		UpdateGooseInRange();
		if (gooseInRange.Count > 0 && (!focused || focusedObject == null)) {
			GetFirstEnemy();
			//Debug.Log(gooseInRange[randIndex].transform.position);
		}
	}

	private void GetFirstEnemy() {
		focusedObject = gooseInRange[0];
		if (focusedObject != null) {
			enemyDS = focusedObject.GetComponent<GooseEntity>().damageSystem;
			//Debug.Log("got enemy DS");
			focused = true;
		}
	}

	private void SetWander(bool stat) {
		if (stat) {
			wander = true;
			FindWanderLoc();
		} else {
			wander = false;

		}
	}

	private void FindWanderLoc() {
		GlobalGame.WanderLoc(ref wanderLoc);

		ConsoleLogger.debug(this.name, "Found Wander Pos " + wanderLoc);
	}

	private void Kill() {
		deadList.Add(gameObject);
		Unfocous();
		ragdoll.EnableRagdoll = true;
		navEle.thisAgent.isStopped = true;
	}

	private void Unfocous() {
		focused = false;
		focusedObject = null;
		enemyDS = null;
	}

	private void AnimationsAndActions() {
		WalkAnim();
		Attack();
	}

	private void WalkAnim() {
		ani.SetBool("Walking", walking);
	}

	//private void DashAnim()
	//{
	//    if (controlPlayer.isDashing && !flappedWings)
	//    {
	//        flappedWings = true;
	//        ani.SetTrigger("FlapWings");
	//    }
	//    else if (!controlPlayer.isDashing && flappedWings)
	//    {
	//        flappedWings = false;
	//    }
	//}

	bool finishedBiting = true;
	private void Attack() {
		if (focused) {
			bool honk = false;
			bool isBiing = ani.GetCurrentAnimatorStateInfo(1).IsName("Bite");
			if (currentTime < Time.time && !isBiing && finishedBiting) {
				int randHonkChance = Random.Range(1, 12);
				honk = randHonkChance > 10;
			}
			//Control Attacks

			//Bite
			if (attack) {
				if (!honk && currentTime < Time.time) {
					StopHonk();
					if (!ani.GetCurrentAnimatorStateInfo(1).IsName("Bite")) {
						ani.ResetTrigger("Bite");
						ani.SetTrigger("Bite");
						finishedBiting = false;
					}

					if (isBiing && !this.biteTrigger.EnableDamage) {
						this.biteTrigger.StartDamage();
					} else if (!isBiing && this.biteTrigger.EnableDamage) {
						this.biteTrigger.StopDamage();
						ani.ResetTrigger("Bite");
						finishedBiting = true;
					}
				} else if (!honkEmitter.Emit && honk && !isBiing) {
					ani.SetBool("HONK", true);


					honkEmitter.StartEmit();
					currentTime = Time.time + 0.5f;
				} else if (honk && currentTime < Time.time && !isBiing) {
					StopHonk();
				}
			}
		}
	}

	private void StopHonk() {
		ani.SetBool("HONK", false);
		//Debug.Log("Stop");

		if (honkEmitter.Emit)
			honkEmitter.StopEmit();
	}


	private void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.CompareTag("Goose")) {
			//Debug.Log(collision.gameObject.name);
			if (collision.gameObject != thisChild) {
				//Debug.Log(gameObject.name + ": found a goose");
				gooseInRange.Add(collision.gameObject);
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (gooseInRange.Contains(other.gameObject)) {
			gooseInRange.Remove(other.gameObject);
			if (focusedObject == other.gameObject) {
				focused = false;
			}
		}
	}
}