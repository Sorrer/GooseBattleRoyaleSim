using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

	public Transform Container;

	public string UUID = "UNKNOWN";
	/// <summary>
	/// The id to reference to assets
	/// </summary>
	public string EntityID = "";
	
	/// <summary>
	/// If game map wants to detect collision, it will preload the bounds of colliders(only) and set it equal to null after it is done
	/// </summary>
	//[HideInInspector]
	//public EntityBounds BoundsCache = null;
	[HideInInspector]
	public bool BoundsCacheActivated = false;
	[HideInInspector]
	private Vector2 MaxRadiusPoint = Vector2.zero;
	[HideInInspector]
	private bool GottenMaxRadius = false;

	/*public EntityBounds GetBounds() {
		return EntityBoundsManager.GetBounds(EntityID);
	}*/

	public Vector2 Get2DOrigin() {
		return new Vector2(transform.position.x, transform.position.z);
	}
	/*
	public void EnableBoundsCache() {
		if (BoundsCacheActivated) return;
		this.BoundsCache = GetBounds();
		BoundsCacheActivated = true;
	}
	public void DisableBoundsCache() {
		this.BoundsCache = null;
		BoundsCacheActivated = false;
	}*/

	/// <summary>
	/// Max radius of bounds
	/// </summary>
	/// <returns></returns>
	/*public float GetBoundsRadius() {

		if (!GottenMaxRadius) {

			Vector2 max = Vector2.zero;
			float maxRadi = 0;

			foreach(CollisionLayerBoundPair c in GetBounds().Bounds) {
				float radi = Vector2.Distance(c.MaxRadiusPoint, Vector2.zero);
				if (radi > maxRadi) {
					max = c.MaxRadiusPoint;
					maxRadi = radi;
				}

			}

			this.MaxRadiusPoint = max;

			GottenMaxRadius = true;
		}

		Vector2 transformed = EntityCollsionDection.TransformPoint(MaxRadiusPoint, Get2DPoints(transform.lossyScale), transform.rotation.eulerAngles.y, Vector2.zero);
		return Vector2.Distance(transformed, Vector2.zero);
	}

	
	*/public static Vector2 Get2DPoints(Vector3 v) {
		return new Vector2(v.x, v.z);
	}/*

	/// <summary>
	/// Is not required, but should be set when present
	/// </summary>
	
	*/
	public DamageSystem damageSystem = null;

	public void SelfGenerateUUID() {
		UUID = EntityUUIDGenerator.generate();
	}

	// ----------- Settable methods ------------------

	/*public virtual void OnMapCollision(EntityCollision collision) {

	}*/


}




public class EntityUUIDGenerator {

	/// <summary>
	/// Generates a UUID
	/// </summary>
	/// <returns> Unique ID for entities </returns>

	public static string generate() {
		return System.Guid.NewGuid().ToString();
	}

}