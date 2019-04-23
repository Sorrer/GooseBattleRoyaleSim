﻿using System.Collections;
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

	bool gameDone = false;

    // Update is called once per frame
    void Update()
    {

		if (gameDone) {
		} else {
			//Check if game is over
			bool foundAlive = false;
			foreach (Entity e in MapManager.GetMap(GameConfig.MAP_GEESE).Entities.Values) {
				if (e.EntityID.Equals("Goose") ){
					GooseEntity goose = (GooseEntity)e;

					if(goose == null) {
						MapManager.RemoveEntity(GameConfig.MAP_GEESE, e);
						continue;
					}

					if(goose.transform.parent == playerMain.transform) {
						continue;
					}

					if (goose.damageSystem != null) {
						if (!goose.damageSystem.IsDead) {
							foundAlive = true;
						}
					}

				}
			}

			if (!foundAlive) {
				//End game
				gameDone = true;
				eventText.ApplyText("Winner Winner Goose Dinner", 10);
				ConsoleLogger.debug("GlobalGame", "Winner");

			}
		}

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
	