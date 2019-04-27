using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
	public float HP;

	public bool IsDead = false;
	Timer timer = null;

	public AudioSource hurtSound;
	public AudioSource deathSound;

	private float origHP;

    private void Awake()
    {
        origHP = HP;
    }

    private void Update() {
		if (IsDead) return;

		if (Vector3.Distance(this.transform.position, GlobalGame.CircleCenter) > GlobalGame.CircleRadius) {

			if(timer == null) {
				timer = new Timer(GlobalGame.CircleDamageTick);
				timer.Start();
			}else if (timer.IsDone()) {
				timer.Start(GlobalGame.CircleDamageTick);
				this.ApplyDamage(GlobalGame.CircleDamage);
			}

		} else {
			timer = null;
		}
	}

	public void ApplyDamage(float amount) {
		if (IsDead) return;

		HP -= amount;

		if(HP <= 0) {
			IsDead = true;
			if (!deathSound.isPlaying)
				deathSound.Play();
		} else {

			if(!hurtSound.isPlaying)
			hurtSound.Play();
		}
	}
	
    public void ApplyRegen(float amount)
    {
        if(origHP > HP)
        {
            HP += amount;
            if (HP > origHP) HP = origHP;
        }
    }
}
