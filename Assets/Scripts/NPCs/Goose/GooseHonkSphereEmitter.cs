using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseHonkSphereEmitter : MonoBehaviour
{


	public bool Emit = false;
	public bool FromParent = false;

	public GameObject honkSpherePrefab;

	public float EmitSpeed = 1;
	public float EmitAmount = 1;
	public float SphereLifetime = 1;
	public float SphereDamage = 1;

	public DamageSystem parentSystem;


	public bool EmitSound = false;

	public AudioSource sound;

	Timer timer;

	bool lastEmit = false;

	public void StartEmit() {
		Emit = true;
		lastEmit = true;
		timer = new Timer(SphereLifetime/EmitAmount);
		timer.Start();

		if (EmitSound) {
			sound.Play();
		}
	}

	public void StopEmit() {
		Emit = false;
		lastEmit = false;
		timer.Stop();

		if(sound != null) {
			sound.Stop();
		}
	}

	private void Update() {
		if(Emit != lastEmit) {
			if (Emit) {
				StartEmit();
			} else StopEmit();
			lastEmit = Emit;
		}


		if (Emit) {
			if (timer.IsDone()) {

				GameObject honkSphere = Instantiate(honkSpherePrefab);
				honkSphere.transform.position = this.transform.position;
				honkSphere.transform.rotation = Quaternion.LookRotation(transform.right);
				GooseHonkSphere honkScript = honkSphere.GetComponent<GooseHonkSphere>();
				honkScript.FromParent = this.FromParent;
				honkScript.startSphere(SphereDamage, SphereLifetime, transform.forward * EmitSpeed, parentSystem);


				timer.Start();
			}
		}
	}
}
