using System.Collections.Generic;
using UnityEngine;

public class GooseBehavious : MonoBehaviour {

	public SimpleNavigation navEle;
	public SphereCollider sC;
	public GameObject thisChild;

	public GameObject player;

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

	public bool walking = false;
	private bool attack = false;

	private float timer = 1;
	private float stopHonkTime = 0;

	private bool focused = false;
	public GameObject focusedObject = null;

	public DamageSystem enemyDS = null;
	public GooseBehavious enemyGB = null;

	private int currentGooseIndex = 0;

	private bool wandering = false;
	private Vector3 wanderLoc = Vector3.zero;
	private PlayerMain playerMainScript;

	private bool isAttackingPlayer = false;

    public float walkCheckTSpan = 1.0f;
    private float nextCheckTime = 0;
    private Vector3 lastCheckPos = Vector3.zero;
    private float fixTriggerTHold = 0;
    private float fixBias = 1.0f;

    public float defaultSearchRadius = 3.0f;
    public float finalSearchRadius = 10.0f;
    private float[] searchRadiuses;
    private bool searchRadiuiiAssigned = false;

    public float regenTickGap = 10.0f;
    public float regenAmount = 2.0f;
    private float nextRegenTime = 0;
    
	// Start is called before the first frame update
	void Start() {
		orgDamageHonk = honkEmitter.SphereDamage;
		orgDamageBite = biteTrigger.Amount;
        thisChild = gameObject.transform.GetChild(0).gameObject;
		playerMainScript = GlobalGame.playerMain;
		player = playerMainScript.gameObject;
        fixTriggerTHold = (navEle.thisAgent.speed * walkCheckTSpan) - fixBias;
	}

	// Update is called once per frame
	void Update() {

        HandleSearchRadius();

        if (!damageSystem.IsDead) {

            //BUNCH OF CHECKS TO AVOID TROUBLE
            LookEnemyInTheEYE();
			IfEnemyDead();
			BeingAttacked();
            FixStutter();

            //FINDING AND MOVING TO ENEMY IF AVAILABLE
            FindMoveAttEne();

            //SAVING ONE SELF TAKES PRECEDENCE OVER TAKING A LIFE
			SaveFromRing();

            //BORING LIFE
            Wander();

            //YAAYY I'M GOD
            if(wandering || !beingAttacked)
            {
                GiveLife();
            }

            //LETS JUST DO WHAT WE HAVE TO DO
			AliveAndKicking();

		} else {
            //I SHALL DIE
			Kill();
		}
	}

    private void LookEnemyInTheEYE()
    {
        if (attacking)
        {
            navEle.directLook = true;
        }
        else{
            navEle.directLook = false;
        }
    }

    private void HandleSearchRadius()
    {
        if (!searchRadiuiiAssigned)
        {
            searchRadiuses = new float[GlobalGame.TotalNumSegments];
            for (int i = 0; i < searchRadiuses.Length; i++)
            {
                searchRadiuses[i] = defaultSearchRadius;
                if (i == searchRadiuses.Length - 1)
                {
                    searchRadiuses[i] = finalSearchRadius;
                }
            }
        }

        if (GlobalGame.CurrentCircleNum != 0)
        {
            sC.radius = searchRadiuses[(int)GlobalGame.CurrentCircleNum - 1];
        }
        else
        {
            sC.radius = defaultSearchRadius;
        }
    }

	private void IfEnemyDead() {
        if(attackingEnemy == null)
        {
            attacking = false;
        }
		if (isAttackingPlayer) {
			if (playerMainScript.damageSystem.IsDead) {
				isAttackingPlayer = false;
				if (currentGooseIndex >= 0 && currentGooseIndex < gooseInRange.Count) {
					gooseInRange.RemoveAt(currentGooseIndex);
				}
				Unfocous();
			}
		} else {
			if (enemyDS != null) {
				if (enemyDS.IsDead) {
					//Debug.Log("My " + gameObject.name + " enemy: " + focusedObject.name + " is DEAD");
					if (currentGooseIndex >= 0 && currentGooseIndex < gooseInRange.Count) {
						gooseInRange.RemoveAt(currentGooseIndex);
					}
					Unfocous();
				}
			}
		}
	}

	private void AliveAndKicking() {
		float dist = Mathf.Abs((navEle.thisAgent.destination - transform.position).magnitude);
		walking = (dist >= navEle.thisAgent.stoppingDistance);
		attack = !walking && (focused || !wandering);
		AnimationsAndActions();
	}

	private void BeingAttacked() {
		if (beingAttacked && !attacking) {
			int randInt = Random.Range(1, 11);
			if (randInt < 9) {
				if (CanAttack(attackerEnemy.GetComponent<GooseBehavious>())) {
					FocousOnAttacker();
				}
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

    private void FindMoveAttEne()
    {
        if (!focused || focusedObject == null)
        {
            //Debug.Log("Finding new Goose");
            SetAttackGoose();

            if ((!focused || focusedObject == null) && !wandering)
            {
                wandering = true;
            }
        }

        if (focusedObject != null)
        {
            wandering = false;
            //Debug.Log(gameObject.name + "setting destination to: " + focusedObject.name);
            navEle.thisAgent.SetDestination(focusedObject.transform.position);
        }
    }

	float orgDamageHonk = 0, orgDamageBite = 0;

	private void GetFirstEnemy() {
		currentGooseIndex = 0;

		for (; currentGooseIndex < gooseInRange.Count; currentGooseIndex++) {
			focusedObject = gooseInRange[currentGooseIndex];
			PlayerMain main = focusedObject.GetComponentInParent<PlayerMain>(); ;
			if (main != playerMainScript) {
				enemyGB = focusedObject.GetComponentInParent<GooseBehavious>();

				if (focusedObject != null && CanAttack(enemyGB)) {
					enemyDS = focusedObject.GetComponent<GooseEntity>().damageSystem;
					enemyGB.attackerEnemy = gameObject;
					enemyGB.beingAttacked = true;
					attackingEnemy = focusedObject;
					attacking = true;
					//Debug.Log("got enemy DS");
					focused = true;
					navEle.thisAgent.speed = 3.5f;
					ani.SetFloat("WalkSpeed", 1f);

					orgDamageHonk = honkEmitter.SphereDamage;
					orgDamageBite = biteTrigger.Amount;

                    if(GlobalGame.CurrentCircleNum != GlobalGame.TotalNumSegments)
                    {
                        honkEmitter.SphereDamage = 0.1f + (Random.value * 2);
                        biteTrigger.Amount = 0.1f + (Random.value * 2);
                    }
                    else
                    {
                        orgDamageHonk = honkEmitter.SphereDamage;
                        orgDamageBite = biteTrigger.Amount;
                    }
                }
			} else {
				if (focusedObject != null) {
					isAttackingPlayer = true;
					attackingEnemy = focusedObject;
					attacking = true;
					//Debug.Log("got enemy DS");
					focused = true;
					navEle.thisAgent.speed = 3.5f;
					ani.SetFloat("WalkSpeed", 1f);

					orgDamageHonk = honkEmitter.SphereDamage;
					orgDamageBite = biteTrigger.Amount;
				}
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
		if (enemyGB != null) {
			if (enemyGB.beingAttacked == true && enemyGB.attacking == true) {
				return false;
			}
			if (toAttackGoose.enemyGB != null) {
				//Debug.Log("Checked enemy for being middle");
				if (toAttackGoose.enemyGB.attacking == true && toAttackGoose.enemyGB.beingAttacked == true) {
					return false;
				}
			}
            if (!toAttackGoose.CanIBeAttacked())
            {
                return false;
            }
            
		}
		return true;
	}

    public bool CanIBeAttacked()
    {
        if (damageSystem.IsDead)
        {
            return false;
        }
        return true;
    }

	private void FindWanderLoc(ref Vector3 wanderLoc) {
		GlobalGame.RandomNavMeshPos(ref wanderLoc);

		//ConsoleLogger.debug(this.name, "Found Wander Pos " + wanderLoc);
	}

    private void Wander()
    {
        if (wandering)
        {
            StopAttackGlitch();
            if (!walking
                || Vector3.Distance(this.transform.position, this.navEle.thisAgent.destination) < this.navEle.thisAgent.stoppingDistance
                || Vector3.Distance(this.navEle.thisAgent.destination, GlobalGame.CircleCenter) > GlobalGame.CircleRadius)
            {
                FindWanderLoc(ref wanderLoc);
            }

            navEle.thisAgent.SetDestination(wanderLoc);
        }
    }

	private void SaveFromRing() {
		if (Mathf.Abs((transform.position - GlobalGame.CircleCenter).magnitude) > GlobalGame.CircleRadius) {
			wandering = true;
		}
	}

    private void GiveLife()
    {
        if(nextRegenTime < Time.time)
        {
            damageSystem.ApplyRegen(regenAmount);
            nextRegenTime = Time.time + regenTickGap;
        }
    }

	private void Kill() {
		if (enemyGB != null) {
			enemyGB.attackingEnemy = null;
		}
		deadList.Add(gameObject);
		Unfocous();
		ragdoll.EnableRagdoll = true;
		StopAttackGlitch();
		navEle.thisAgent.isStopped = true;
	}

	private void Unfocous() {
		if (focusedObject != player) {
			if (enemyGB != null) {
				enemyGB.attackerEnemy = null;
				enemyGB.beingAttacked = false;
			}
        }
        else
        {
            isAttackingPlayer = false;
        }
		attacking = false;

		navEle.thisAgent.speed = 1f;
		ani.SetFloat("WalkSpeed", 0.35f);
		focused = false;
		focusedObject = null;


		honkEmitter.SphereDamage = orgDamageHonk;
		biteTrigger.Amount = orgDamageBite ;


		enemyDS = null;
		enemyGB = null;
		walking = false;
	}

    private void FixStutter()
    {
        if (walking)
        {
            if(nextCheckTime < Time.time)
            {
                if(Mathf.Abs((transform.position - lastCheckPos).magnitude) < fixTriggerTHold)
                {
                    walking = false;
                }
                nextCheckTime = Time.time + walkCheckTSpan;
                lastCheckPos = transform.position;
            }
        }
    }

	private void StopAttackGlitch() {
		StopHonk();
		biteTrigger.EnableDamage = false;
	}

	private void FocousOnAttacker() {
		if (attackerEnemy != null) {
			focusedObject = attackerEnemy;
			attackingEnemy = attackerEnemy;
			enemyGB = attackerEnemy.GetComponent<GooseBehavious>();
			enemyDS = enemyGB.damageSystem;
			focused = true;
			attacking = true;

			orgDamageHonk = honkEmitter.SphereDamage;
			orgDamageBite = biteTrigger.Amount;

			honkEmitter.SphereDamage = 0.1f + (Random.value * 0.5f);
			biteTrigger.Amount = 0.1f + (Random.value * 1f);
		}
	}

	private void AnimationsAndActions() {
		WalkAnim();
		Attack();
	}

	private void WalkAnim() {
		ani.SetBool("Walking", walking);
	}

	bool finishedBiting = true;
	private void Attack() {
		if (focused) {
			bool honk = false;
			bool isBiing = ani.GetCurrentAnimatorStateInfo(1).IsName("Bite");
			if (stopHonkTime < Time.time && !isBiing && finishedBiting) {
				int randHonkChance = Random.Range(1, 12);
				honk = randHonkChance > 6;
			}
            float realDiff = Mathf.Abs((transform.position - navEle.thisAgent.destination).magnitude);
            if (realDiff > navEle.thisAgent.stoppingDistance)
            {
                honk = true;
            }
			//Control Attacks

			//Bite
			if (attack) {
				if (!honk && stopHonkTime < Time.time) {
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
				}else if (!honkEmitter.Emit && honk && !isBiing) {
					ani.SetBool("HONK", true);
					honkEmitter.StartEmit();
					stopHonkTime = Time.time + 0.35f;
				} else if (honk && stopHonkTime < Time.time && !isBiing) {
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