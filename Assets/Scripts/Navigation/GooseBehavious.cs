using System.Collections.Generic;
using UnityEngine;

public class GooseBehavious : MonoBehaviour
{
    public SimpleNavigation navEle;
    public SphereCollider sC;
    public GameObject thisChild;

    public List<GameObject> gooseInRange = new List<GameObject>();
    private bool gooseAdded = false;

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
    private GameObject focusedObject = null;

    private bool currentEnemyLState = false;

    // Start is called before the first frame update
    void Start()
    {
        focusedObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (gooseAdded)
        {
            if (!focused)
            {
                //Debug.Log("Finding new Goose");
                SetAttackGoose();
            }
            gooseAdded = false;
        }
        navEle.thisAgent.SetDestination(focusedObject.transform.position);
        Die();

        float dist = Mathf.Abs((navEle.thisAgent.destination - transform.position).magnitude);
        walking = (dist >= navEle.thisAgent.stoppingDistance);
        attack = !walking;
        AnimationsAndActions();
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Goose"))
        {
            if (collision.gameObject != thisChild)
            {
                //Debug.Log("Found a goose");
                gooseInRange.Add(collision.gameObject);
                gooseAdded = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gooseInRange.Contains(other.gameObject))
        {
            gooseInRange.Remove(other.gameObject);
            if(focusedObject == other.gameObject)
            {
                focused = false;
            }
            if(gooseInRange.Count <= 0)
            {
                gooseAdded = false;
            }
        }
    }

    private void SetAttackGoose()
    {
        if (gooseInRange.Count > 0 && !focused)
        {
            GetEnemyPos();
            //Debug.Log("Did this");
            //Debug.Log(gooseInRange[randIndex].transform.position);
        }
    }

    private void GetEnemyPos()
    {
        DamageSystem curSystem = null;
        bool foundAlive = false;
        while (!foundAlive)
        {
            int randIndex = Random.Range(0, gooseInRange.Count);
            focusedObject = gooseInRange[randIndex];
            if(focusedObject != null)
            {
                if (focusedObject.CompareTag("Player"))
                {
                    curSystem = focusedObject.GetComponent<PlayerMain>().damageSystem;
                }
                else if (focusedObject.CompareTag("Goose"))
                {
                    curSystem = focusedObject.GetComponent<GooseEntity>().damageSystem;
                }

                if (!curSystem.IsDead)
                {
                    //Debug.Log(focusedObject.name);
                    foundAlive = true;
                }
            }

        }
        focused = true;
    }
    

    private void AnimationsAndActions()
    {
        WalkAnim();
        Attack();
    }

    private void WalkAnim()
    {
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
    private void Attack()
    {
        bool honk = false;
        bool isBiing = ani.GetCurrentAnimatorStateInfo(1).IsName("Bite");
        if(currentTime < Time.time && !isBiing && finishedBiting)
        {
            int randHonkChance = Random.Range(1, 12);
            honk = randHonkChance > 10;
        }
        //Control Attacks

        //Bite
        if (attack)
        {
            if (!honk && currentTime < Time.time)
            {
                StopHonk();
                if (!ani.GetCurrentAnimatorStateInfo(1).IsName("Bite"))
                {
                    ani.ResetTrigger("Bite");
                    ani.SetTrigger("Bite");
                    finishedBiting = false;
                }

                if (isBiing && !this.biteTrigger.EnableDamage)
                {
                    this.biteTrigger.StartDamage();
                }
                else if (!isBiing && this.biteTrigger.EnableDamage)
                {
                    this.biteTrigger.StopDamage();
                    ani.ResetTrigger("Bite");
                    finishedBiting = true;
                }
            }
            else if(!honkEmitter.Emit && honk && !isBiing)
            {
                ani.SetBool("HONK", true);

                
                honkEmitter.StartEmit();
                currentTime = Time.time + 0.5f;
            }
            else if(honk && currentTime < Time.time && !isBiing)
            {
                StopHonk();
            }
        }
         
        
    }

    private void StopHonk()
    {
        ani.SetBool("HONK", false);
        //Debug.Log("Stop");

        if (honkEmitter.Emit)
            honkEmitter.StopEmit();
    }
    private void Die()
    {
        if (damageSystem.IsDead)
        {
            ragdoll.EnableRagdoll = true;
        }
        else
        {
            ragdoll.EnableRagdoll = false;
        }
        
    }
}
