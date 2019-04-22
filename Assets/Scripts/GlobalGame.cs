using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GlobalGame : MonoBehaviour
{

	public static bool Paused = false;
	public static bool EnableEdgeScroll = false;
	public static PlayerMain Player;
	public static GameMapManager MapManager;

	public static bool ForceDrop = false;
	public static int CircleRadius = 50;

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public static bool WanderLoc(ref Vector3 pos) {
		Vector3 center = new Vector3();
		center.z = -32.2f;
		Vector3 ranLoc = CircleRadius * Random.insideUnitSphere;

		NavMeshHit navHit;
		if(!NavMesh.SamplePosition(ranLoc, out navHit, CircleRadius, -1)) {
			return false;
		}

		pos = navHit.position;

		return true;
	}
}
	