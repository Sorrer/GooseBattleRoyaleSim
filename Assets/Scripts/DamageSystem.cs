using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
	public float HP;

	public bool IsDead = false;

	public void ApplyDamage(float amount) {
		if (IsDead) return;

		HP -= amount;

		if(HP <= 0) {
			IsDead = true;
		}
	}
	
}
