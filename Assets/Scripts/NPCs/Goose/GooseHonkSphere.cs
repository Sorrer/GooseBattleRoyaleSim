using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseHonkSphere : MonoBehaviour
{

	float damage, lifeTime;
	Vector3 momentum;
	Timer lifeTimer;

	DamageSystem system;

	void Start() {
		lifeTimer = new Timer(lifeTime);
		lifeTimer.Start();
	}

	public void startSphere(float damage, float lifeTime, Vector3 momentum, DamageSystem parentSystem) {
		this.damage = damage;
		this.lifeTime = lifeTime;
		this.momentum = momentum;
		this.system = parentSystem;
		GetComponent<Rigidbody>().AddForce(momentum);
		lifeTimer = new Timer(lifeTime);
		lifeTimer.Start();
	}

    // Update is called once per frame
    void Update() {

		this.transform.localScale = transform.localScale + new Vector3(0.05f, 0, 0.05f);
		if (lifeTimer.IsDone()) {
			Destroy(this.gameObject);
		}   
    }

	bool damageSafety = false;

	private void OnTriggerEnter(Collider other) {
		if (damageSafety) return;
		DamageSystem system = other.GetComponent<DamageSystem>();
		if (system == null || system == this.system) return;
		
		system.ApplyDamage(damage);
		damageSafety = true;
		Destroy(this.gameObject);
	}
}
