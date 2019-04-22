using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{

	public bool isParentDamager = false;

	public DamageSystem self;
	public bool EnableDamage = false;
	[SerializeField]
	private float Amount = 0;

	List<DamageSystem> currentlyDamaged = new List<DamageSystem>();

	public void StartDamage() {
		StartDamage(this.Amount);
	}

	public void StartDamage(float amount) {



		this.Amount = amount;
		EnableDamage = true;

		foreach(DamageSystem system in currentlyDamaged) {
			bool deaded = system.IsDead;
			system.ApplyDamage(this.Amount);
			if (this.isParentDamager && system.IsDead != deaded) {
				GameStats.Kills++;
			}

		}
	}	

	public void StopDamage() {
		this.EnableDamage = false;
		//currentlyDamaged.Clear();
	}
	
	private void OnTriggerEnter(Collider other) {
		DamageSystem system = other.GetComponent<DamageSystem>();

		if(system != null && system != self) {
			if (currentlyDamaged.Contains(system)) return;


			currentlyDamaged.Add(system);
			

			if (EnableDamage) {
				//print("Bit " + other.transform.name);
				bool deaded = system.IsDead;
				system.ApplyDamage(this.Amount);
				if(this.isParentDamager && system.IsDead != deaded) {
					GameStats.Kills++;
				}
			}
		}
	}


	private void OnTriggerExit(Collider other) {
		

		DamageSystem system = other.GetComponent<DamageSystem>();
		
		if(system != null && system != self) {
			if (!currentlyDamaged.Contains(system)) return;
			currentlyDamaged.Remove(system);
		}
	}
	

}
