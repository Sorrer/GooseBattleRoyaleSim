using System.Collections.Generic;
using UnityEngine;

public class GooseBehavious : MonoBehaviour {
	public SimpleNavigation navEle;
	public SphereCollider sC;
	public GameObject thisChild;

	public GameObject attackingEnemy = null;
	public GameObject attackerEnemy = null;

	public bool beingAttacked = false;
	public bool attacking = true;
	public bool attackingIsMiddle = false;

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

	private DamageSystem enemyDS = null;
	public GooseBehavious enemyGB = null;

	public Transform centerOfRing;

	private int currentGooseIndex = 0;

	private bool wandering = false;
	private Vector3 wanderLoc = Vector3.zero;

	// Start is called before the first frame update
	void Start() {
		thisChild = gameObject.transform.GetChild(0).gameObject;
		centerOfRing.position = new Vector3(0, transform.position.y, 0);
	}

	// Update is called once per frame
	void Update() {
		if (!damageSystem.IsDead) {
			IfEnemyDead();

			if (!focused || focusedObject == null) {
				//Debug.Log("Finding new Goose");
				SetAttackGoose();

				if ((!focused || focusedObject == null) && !wandering) {
					wandering = true;
				}
			}

			if (focusedObject != null) {
				wandering = false;
				//Debug.Log(gameObject.name + "setting destination to: " + focusedObject.name);
				navEle.thisAgent.SetDestination(focusedObject.transform.position);
			}

			if (wandering) {
				if (!walking) {
					FindWanderLoc(ref wanderLoc);
				}

				navEle.thisAgent.SetDestination(wanderLoc);
			}

			AliveAndKicking();
		} else {
			Kill();
		}
	}

	private void IfEnemyDead() {
		if (enemyDS != null) {
			if (enemyDS.IsDead) {
				//Debug.Log("My " + gameObject.name + " enemy: " + focusedObject.name + " is DEAD");
				Unfocous();
				gooseInRange.RemoveAt(currentGooseIndex);
			}
		}
	}

	private void AliveAndKicking() {
		float dist = Mathf.Abs((navEle.thisAgent.destination - transform.position).magnitude);
		walking = (dist >= navEle.thisAgent.stoppingDistance);
		attack = !walking && (focused || !wandering);
		AnimationsAndActions();
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
		currentGooseIndex = 0;

		for (; currentGooseIndex < gooseInRange.Count; currentGooseIndex++) {
			focusedObject = gooseInRange[currentGooseIndex];
			enemyGB = focusedObject.GetComponentInParent<GooseBehavious>();
			if (focusedObject != null && CanAttack(enemyGB)) {
				enemyDS = focusedObject.GetComponent<GooseEntity>().damageSystem;
				enemyGB.attackerEnemy = gameObject;
				enemyGB.beingAttacked = true;
				attackingEnemy = focusedObject;
				attacking = true;
				//Debug.Log("got enemy DS");
				focused = true;
			}
			if (focused) {
				break;
			} else {
				enemyGB = null;
				focusedObject = null;
			}
		}
	}

	private bool CanAttack(GooseBehavious toAttackGoose) {
		if (enemyGB.beingAttacked == true && enemyGB.attacking == true) {
			return false;
		}
		if (toAttackGoose.enemyGB != null) {
			//Debug.Log("Checked enemy for being middle");
			if (toAttackGoose.enemyGB.attacking == true && toAttackGoose.enemyGB.beingAttacked == true) {
				return false;
			}
		}
		return true;
	}

	private void SetWandering(bool stat) {
		if (stat) {
			wandering = true;
			wanderLoc = Vector3.zero;
		} else {
			wandering = false;

		}
	}

	private void FindWanderLoc(ref Vector3 wanderLoc) {
		GlobalGame.RandomNavMeshPos(ref wanderLoc);

		//ConsoleLogger.debug(this.name, "Found Wander Pos " + wanderLoc);
	}

	private void Kill() {
		if (enemyGB != null) {
			enemyGB.attackingEnemy = null;
		}
		deadList.Add(gameObject);
		Unfocous();
		ragdoll.EnableRagdoll = true;
		StopHonk();
		biteTrigger.EnableDamage = false;
	}

	private void Unfocous() {
		if (enemyGB != null) {
			enemyGB.attackerEnemy = null;
			enemyGB.beingAttacked = false;
		}
		attacking = false;

		focused = false;
		focusedObject = null;

		enemyDS = null;
		enemyGB = null;
	}

	private void CantAttack() {
		if (Mathf.Abs((transform.position - centerOfRing.position).magnitude) > navEle.thisAgent.stoppingDistance) {
			navEle.thisAgent.SetDestination(centerOfRing.position);
		}
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