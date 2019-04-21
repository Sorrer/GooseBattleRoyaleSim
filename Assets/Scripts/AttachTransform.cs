using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTransform : MonoBehaviour {


	// Transforms that attach to this object (Via Position)
	public List<Transform> Transforms = new List<Transform>();
	public float SmoothTime = 0.3f;
	Vector3 velocity = Vector3.zero;
	public Vector3 offset = Vector3.zero;

	Hashtable transVel = new Hashtable();
	
	void Start () {
	}
	
	void FixedUpdate () {
		foreach (Transform tran in Transforms) {
			if(!transVel.Contains(tran)) {
				transVel.Add(tran, Vector3.zero);
			}
			Vector3 refin = ((Vector3)(transVel[tran]));

			tran.position = Vector3.SmoothDamp(tran.position, transform.position + offset, ref refin, SmoothTime, 10000, Time.fixedDeltaTime);
			transVel[tran] = refin;
		}
	}
}

