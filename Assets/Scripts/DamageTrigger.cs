using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
	public DamageSystem self;
	public bool EnableDamage = false;
	[SerializeField]
	private float Amount = 0;

	List<DamageSystem> currentlyDamaged = new List<DamageSystem>();

	public void StartDamage() {
		StartDamage(this.Amount);
	}

	public void StartDamage(float amount) {

		print("Enabling");

		this.Amount = amount;
		EnableDamage = true;

		foreach(DamageSystem system in currentlyDamaged) {
			system.ApplyDamage(this.Amount);

			print("Bit " + system.transform.name);
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
				print("Bit " + other.transform.name);
				system.ApplyDamage(this.Amount);
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
