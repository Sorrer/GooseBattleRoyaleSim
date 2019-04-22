using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseRagdoll : MonoBehaviour
{


	public Transform root;
	public Animator ani;
	public Collider col;
	public bool EnableRagdoll = false;
	bool lastRagdoolStatus = false;

	bool resetRigidbodies = false;
	// Start is called before the first frame update

	Hashtable originalLocation, originalRotation;
		
	void Start()
    {
		ani.enabled = true;
		lastRagdoolStatus = !EnableRagdoll;

		originalLocation = new Hashtable();
		originalRotation = new Hashtable();
		saveOriginal(root);
	}

	void saveOriginal(Transform loc) {
		foreach(Transform child in loc) {
			saveOriginal(child);

			originalLocation.Add(child.name, child.localPosition);
			originalRotation.Add(child.name, child.localRotation);
		}
	}

    // Update is called once per frame
    void Update() {

		if (resetRigidbodies) {

			ani.enabled = !ani.enabled;
			ResetRigidbodies(root);
			resetRigidbodies = false;
		}

		if (EnableRagdoll != lastRagdoolStatus) {
			if (EnableRagdoll) {
				TurnOnRagdoll(root);
				col.enabled = false;
			} else {
				col.enabled = true;
				TurnOffRagdoll(root);
			}

			lastRagdoolStatus = EnableRagdoll;
		}

    }

	public void ResetRigidbodies(Transform obj) {
		foreach(Transform child in obj) {
			ResetRigidbodies(child);

			Rigidbody body = child.GetComponent<Rigidbody>();
			if(body != null) {
				body.isKinematic = !body.isKinematic;
			}

		}
	}

	public void TurnOnRagdoll(Transform obj) {
		ani.enabled = false;
		//resetRigidbodies = true;
		foreach(Transform child in obj) {
			TurnOnRagdoll(child);

			Rigidbody body = child.GetComponent<Rigidbody>();
			if (body != null) {

				body.isKinematic = false;
				body.velocity = new Vector3(0f, 0f, 0f);
				body.angularVelocity = new Vector3(0f, 0f, 0f);

				//print(child);
			}

		}
	}

	public void TurnOffRagdoll(Transform obj) {
		ani.enabled = true;

		foreach (Transform child in obj) {
			TurnOffRagdoll(child);


			Vector3 orgLoc;
			Quaternion orgRot;
			
			orgLoc = (Vector3)originalLocation[child.name];
			orgRot = (Quaternion)originalRotation[child.name];

			if (orgLoc != null) child.localPosition = orgLoc;
			if (orgRot != null) child.localRotation = orgRot;

			Rigidbody body = child.GetComponent<Rigidbody>();
			if (body != null) {
				body.isKinematic = true;
				
				body.velocity = new Vector3(0f, 0f, 0f);
				body.angularVelocity = new Vector3(0f, 0f, 0f);
			}

		}
	}


}
