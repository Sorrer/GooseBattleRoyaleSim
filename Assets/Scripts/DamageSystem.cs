using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
	public float HP;

	public bool IsDead = false;
	Timer timer = null;

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
		}
	}
	
}
