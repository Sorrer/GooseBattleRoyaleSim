using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameMap
{
	public Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();

	public Dictionary<string, Entity> ColliderEntities = new Dictionary<string, Entity>();

	public string Name = "";



	//--------------------- Colliding Methods -------------------------

	// Note - If optimization required, try having collision to colliders only happen if within a distance


	public bool EnableFullCollision = false;
	public bool EnableCollision = false;
	/// <summary>
	/// Updates collision list using ColliderEntities (Or entities if EnableFullCollision is true)
	/// </summary>
	public void UpdateCollision() {
		if (EnableFullCollision) {
			this.UpdateCollision(Entities);
		} else {
			this.UpdateCollision(ColliderEntities);
		}
	}

	/// <summary>
	/// Updates collision list using defined list
	/// </summary>
	/// <param name="colliders">The list of colliders to test with</param>
	public void UpdateCollision(Dictionary<string, Entity> colliders) {
		UpdateCollision(colliders, false);
	}

	/// <summary>
	/// Updates collision list using defined list. (Optional: Only call the collider's methods)
	/// </summary>
	/// <param name="colliders">The list of colliders to test with</param>
	/// <param name="OnlyCallColliders">Only call collider's methods if true</param>
	public void UpdateCollision(Dictionary<string, Entity> colliders, bool OnlyCallColliders) {

		foreach(Entity e1 in colliders.Values) {
			foreach(Entity e2 in Entities.Values) {
				//EntityCollisionUtils.Collides(e1, e2, OnlyCallColliders);
			}
		}

	}

	/// <summary>
	/// Updates the collision of this single entity. Note that only this entity's collsion methods will be called;
	/// </summary>
	/// <param name="entity">Entity to be updated</param>
	public void UpdateEntityCollision(Entity entity) {
		Dictionary<string, Entity> e = new Dictionary<string, Entity>();
		e.Add(entity.UUID, entity);

		UpdateCollision(e);
	}

	/// <summary>
	/// Checks the collision of a single entity then return the isColliding
	/// </summary>
	/// <param name="e1">Collider</param>
	/// <returns>Colliding or not</returns>
	public bool Collides(Entity e1) {
		//e1.EnableBoundsCache();
		foreach (Entity e2 in Entities.Values) {
			//if (EntityCollisionUtils.Collides(e1, e2, true)) {
				return true;
			//}
		}

		return false;
	}

	/// <summary>
	/// Checks the collision of a single entity then return a list of colliding entities
	/// </summary>
	/// <param name="e1">Collider</param>
	/// <returns>Colliding Entities</returns>
	public List<Entity> CollidesOf(Entity e1) {
		List<Entity> cur = new List<Entity>();

		//e1.EnableBoundsCache();

		/*foreach (Entity e2 in Entities.Values) {
			if (EntityCollisionUtils.Collides(e1, e2, true)) {
				cur.Add(e2);
			}
		}*/

		return cur;
	}


	// ----------------------- Colliders add/remove ----------------------------------

	public void AddCollider(Entity entity) {
		if (entity.UUID == "UNKNOWN" || entity.UUID == "") {
			entity.SelfGenerateUUID();
		}

		this.ColliderEntities.Add(entity.UUID, entity);
	}

	/// <summary>
	/// Removes entity from colliders.
	/// </summary>
	/// <param name="entity">The entity to be removed</param>
	/// 
	public void RemoveCollider(Entity entity) {

		RemoveCollider(entity.UUID);

	}

	/// <summary>
	/// Removes entity from colliders.
	/// </summary>
	/// <param name="UUID">The UUID of the entity</param>
	public void RemoveCollider(string UUID) {

		if (ColliderEntities.Remove(UUID)) {
			// Do nothing
		} else {
			ConsoleLogger.debug("GameMap", "Removing " + UUID + " failed!");
		}

	}

	/// <summary>
	/// Removes entity from colliders. (Makes printing debug option if unsure if object is in list)
	/// </summary>
	/// <param name="UUID">The ID of the entity</param>
	/// <param name="print">To print if failed or not</param>
	public void RemoveCollider(string UUID, bool print) {

		if (ColliderEntities.Remove(UUID)) {
			// Do nothing
		} else {
			if (print)
				ConsoleLogger.debug("GameMap", "Removing " + UUID + " failed!");
		}

	}





	//--------------------- Entity Methods ----------------------------


	/// <summary>
	///	Find objects by UUID
	/// </summary>
	/// <param name="UUID"> ID of object trying to be found </param>
	/// <returns></returns>

	public Entity FindEntity(string UUID) {

		//Use default method to find entity
		Entity findings;
		Entities.TryGetValue(UUID, out findings);

		//Log if no objects of UUID is found (For debugging)
		if (findings == null) { ConsoleLogger.debug("Map", "Dictionary called! Found no objects of " + UUID); }

		//Return findings even if null
		return findings;

	}

	/// <summary>
	/// Adds entity to map. If UUID is "UNKNOWN" then a UUID is generated
	/// </summary>
	/// <param name="entity">The entity to be added</param>
	/// 
	public void AddEntity(Entity entity) {
		if(entity.UUID == "UNKNOWN") {
			entity.SelfGenerateUUID();
		}

		this.Entities.Add(entity.UUID, entity);
	}
	


	/// <summary>
	/// Removes entity from map.
	/// </summary>
	/// <param name="entity">The entity to be removed</param>
	/// 
	public void RemoveEntity(Entity entity) {

		RemoveEntity(entity.UUID);

	}

	/// <summary>
	/// Removes entity from map.
	/// </summary>
	/// <param name="UUID">The UUID of the entity</param>
	public void RemoveEntity(string UUID) {
		
		if (Entities.Remove(UUID)) {
			// Do nothing
		} else {
			ConsoleLogger.debug("GameMap", "Removing " + UUID + " failed!");
		}

	}

	
	public void RemoveEntity(string UUID, bool print) {

		if (Entities.Remove(UUID)) {
			// Do nothing
		} else {
			if(print)
			ConsoleLogger.debug("GameMap", "Removing " + UUID + " failed!");
		}

	}

}
