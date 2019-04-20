using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapManager : MonoBehaviour
{
	
	public Dictionary<string, GameMap> Maps = new Dictionary<string, GameMap>();



	public void Start() {
		GlobalGame.MapManager = this;

		//Generate Maps
		ConsoleLogger.debug("MapManager", "Starting");
		foreach(string map_name in GameConfig.Maps) {
			this.CreateNewMap(map_name);
		}

	}

	public void Update() {
		
		//Collision Check
		foreach(GameMap map in Maps.Values) {
			if (map.EnableCollision) {
				map.UpdateCollision();
			}
		}


		//DEBUG

		if (Input.GetKeyDown(KeyCode.F9)) {
			this.print();
		}


	}

	public void print() {
		foreach(KeyValuePair<string, GameMap> map in Maps) {
			ConsoleLogger.debug("[GameMapManager]", "Map: " + map.Key);

			foreach(KeyValuePair<string, Entity> entity in map.Value.Entities) {
				ConsoleLogger.debug("[GameMapManager]", "----->" + entity.Key + " | " + entity.Value);
			}
		}
	}




	// ------------------------- Utilities ---------------------------------------------------

	/// <summary>
	/// Creates and adds a new map to dictionary using name (Names must be unique)
	/// </summary>
	/// <param name="name">Unique map name</param>
	/// <returns>If name is aleady in dictionary, return false</returns>
	public bool CreateNewMap(string map_name) {
		return CreateNewMap(map_name, false);
	}


	/// <summary>
	/// Creates and adds a new map to dictionary using unique name. Also set state of collision checks
	/// </summary>
	/// <param name="map_name">Unique map name</param>
	/// <param name="CollisionCheck">False - Disabled | True - Enabled</param>
	/// <returns>If name is aleady in dictionary, return false</returns>
	public bool CreateNewMap(string map_name, bool CollisionCheck) {
		//Check names
		if (Maps.ContainsKey(map_name)) {
			return false;
		}
		GameMap created = new GameMap();
		created.Name = map_name;
		created.EnableCollision = CollisionCheck;
		Maps.Add(map_name, created);
		return true;
	}





	// ----------------- Getters/Setters ---------------------------------
	

	/// <summary>
	/// Get map based on the name.
	/// </summary>
	/// <param name="map_name">Name of map</param>
	/// <returns></returns>
	public GameMap GetMap(string map_name) {

		GameMap found;

		Maps.TryGetValue(map_name, out found);

		if(found == null) {
			ConsoleLogger.debug("GameMapManager", "Trying to get an invalid map, '" + map_name + "'!");
		}

		return found;

	}

	/// <summary>
	/// Enables/Disables the collision checking system of a single map.
	/// </summary>
	/// <param name="map_name">Map name</param>
	/// <param name="enabled_colision">Status to change to</param>
	/// <returns>Last state before chang</returns>
	public bool SetCollisionCheck(string map_name, bool enabled_colision) {
		GameMap map;

		map = GetMap(map_name);

		if(map == null) {
			ConsoleLogger.debug("GameMapManager", "Trying to change collision check of invalid map, '" + map_name + "'!");
			return false;
		} else {
			bool lastState = map.EnableCollision;
			map.EnableCollision = enabled_colision;
			return lastState;
		}
	}

	/// <summary>
	/// Enables/Disables the full (Entities on Entities) collision checking system of a single map.
	/// </summary>
	/// <param name="map_name">Map name</param>
	/// <param name="enabled_colision">Status to change to</param>
	/// <returns>Last state before chang</returns>
	public bool SetFullCollisionCheck(string map_name, bool enabled_colision) {
		GameMap map;

		map = GetMap(map_name);

		if (map == null) {
			ConsoleLogger.debug("GameMapManager", "Trying to change collision check of invalid map, '" + map_name + "'!");
			return false;
		} else {
			bool lastState = map.EnableCollision;
			map.EnableFullCollision = enabled_colision;
			return lastState;
		}
	}

	/// <summary>
	/// Get status of collision check of map
	/// </summary>
	/// <param name="map_name">Name of map</param>
	/// <returns>Status of map requested</returns>
	public bool GetCollisionStatus(string map_name) {

		GameMap map;

		map = GetMap(map_name);

		if(map == null) {
			ConsoleLogger.debug("GameMapManager", "Trying to check collision status of invalid map, '" + map_name + "'!");
			return false;
		} else {
			return map.EnableCollision;
		}
		
	}

	/// <summary>
	/// Get status of full collision check of map
	/// </summary>
	/// <param name="map_name">Name of map</param>
	/// <returns>Status of map requested</returns>
	public bool GetFullCollisionStatus(string map_name) {

		GameMap map;

		map = GetMap(map_name);

		if (map == null) {
			ConsoleLogger.debug("GameMapManager", "Trying to check collision status of invalid map, '" + map_name + "'!");
			return false;
		} else {
			return map.EnableFullCollision;
		}

	}
	/// <summary>
	/// Get the entity from the specific game map
	/// </summary>
	/// <param name="map_name">The name of the map that is holding the entity</param>
	/// <param name="uuid">The uuid of the entity to be found</param>
	/// <returns></returns>
	public Entity GetEntity(string map_name, string uuid) {
		GameMap map = GetMap(map_name);

		if(map == null) {
			ConsoleLogger.debug("GameMapManager", "Trying to get entity from an invalid map, '" + map_name + "'!");
			return null;
		}


		return map.FindEntity(uuid);
	}




	// ----------------- Add/Remove entities -----------------------------


	/// <summary>
	/// Adds entity to specific game map
	/// </summary>
	/// <param name="name">Map name</param>
	/// <param name="entity">Entity to be added</param>
	public void AddEntity(string map_name, Entity entity) {

		if (entity == null) print(entity);

		GameMap selectedMap;

		if(Maps.TryGetValue(map_name, out selectedMap)) {
			selectedMap.AddEntity(entity);
		} else {
			ConsoleLogger.debug("GameMapManager", "Map '" + name + "' does not exist yet entity wants to be added to it!");
		}


	}


	/// <summary>
	/// Removes entity from selected map.
	/// </summary>
	/// <param name="map_name">Name of the map to remove from</param>
	/// <param name="entity">Entity that needs to be removed</param>
	public void RemoveEntity(string map_name, Entity entity) {
		RemoveEntity(map_name, entity.UUID);
	}


	/// <summary>
	/// Removes entity from selected map.
	/// </summary>
	/// <param name="map_name">Name of the map to remove from</param>
	/// <param name="UUID">Entity that needs to be removed</param>
	public void RemoveEntity(string map_name, string UUID) {
		GameMap selectedMap;

		if (Maps.TryGetValue(map_name, out selectedMap)) {
			selectedMap.RemoveEntity(UUID);
		} else {
			ConsoleLogger.debug("GameMapManager", "Map '" + map_name + "' does not exist yet entity wants to be removed from it!");
		}
	}

	/// <summary>
	/// Removes entity from all maps (If map that contains entity is unknown) [Not recommended]
	/// </summary>
	/// <param name="UUID">UUID to remove from all maps</param>
	public void RemoveEntity(string UUID) {
		foreach(GameMap cur in Maps.Values) {

			cur.RemoveEntity(UUID, false);

		}
	}


	
	

}
