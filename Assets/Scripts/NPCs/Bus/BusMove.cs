using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusMove : MonoBehaviour
{


	public Transform EndPoint;

    void FixedUpdate()
    {
		this.transform.position = Vector3.MoveTowards(this.transform.position, EndPoint.transform.position, 6 * Time.fixedDeltaTime);

		if(Vector3.Distance(this.transform.position, EndPoint.position) <= 6 * Time.fixedDeltaTime + 1) {
			GlobalGame.ForceDrop = true;

			Destroy(this);
		}
    }
}
