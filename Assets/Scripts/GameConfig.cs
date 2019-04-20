using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig {

	/*
	public const string MAP_ENTITIES = "Entity";
	public const string MAP_BUILDINGS = "Buildings";
	public const string MAP_LIGHTS = "Lights";
	public const string MAP_REACTIVE = "Reactive";
	public const string MAP_SCENERY = "Scenery"; 
	public const string MAP_UNKNOWN = "UNKNOWN";
	*/
	public const string MAP_GEESE = "Geese";

	public readonly static string[] Maps = { MAP_GEESE, /*MAP_BUILDINGS, MAP_LIGHTS, MAP_REACTIVE, MAP_SCENERY, MAP_UNKNOWN */};
	public readonly static string[] TopDownMaps = { /*MAP_BUILDINGS, MAP_SCENERY*/};
}
