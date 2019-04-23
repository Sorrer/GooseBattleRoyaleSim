using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GlobalGame : MonoBehaviour
{

	public static bool Paused = false;
	public static bool EnableEdgeScroll = false;
	public static PlayerMain playerMain;
	public static GameMapManager MapManager;

	public static bool ForceDrop = false;
	public static float CircleRadius = 50;
	public static Vector3 CircleCenter = Vector3.zero;

	public static GlobalGame _instance;

	public static float CircleDamage = 5;
	public static float CircleDamageTick = 1;

    public static int TotalNumSegments = 0;
    public static float CurrentCircleNum = 1;

	public Transform centerPos;
	public float radiPos;
	public static EventText eventText;

	void Start()
    {
		GlobalGame._instance = this;
		GlobalGame.CircleCenter = centerPos.position;
		GlobalGame.CircleRadius = radiPos;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public static bool RandomNavMeshPos(ref Vector3 pos) {
		
		Vector3 ranLoc = CircleRadius * Random.insideUnitSphere;
		ranLoc += CircleCenter;

		NavMeshHit navHit;
		if(!NavMesh.SamplePosition(ranLoc, out navHit, CircleRadius, -1)) {
			return false;
		}

		pos = navHit.position;

		return true;
	}
}
	