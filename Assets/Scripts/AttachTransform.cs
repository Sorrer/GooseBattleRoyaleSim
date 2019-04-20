using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTransform : MonoBehaviour {


	// Transforms that attach to this object (Via Position)
	public List<Transform> Transforms = new List<Transform>();
	public float SmoothTime = 0.3f;
	Vector3 velocity = Vector3.zero;
	public Vector3 offset = Vector3.zero;
	void Start () {
		
	}
	
	void FixedUpdate () {
		foreach (Transform tran in Transforms) {
			tran.position = Vector3.SmoothDamp(tran.position, transform.position + offset, ref velocity, SmoothTime, 10000, Time.fixedDeltaTime);
		}
	}
}
