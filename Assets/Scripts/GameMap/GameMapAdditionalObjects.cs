using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to add objects based on the scene
/// </summary>
public class GameMapAdditionalObjects : MonoBehaviour
{
	[SerializeField]
	List<GameMapAdditionalObject> ToAdd = new List<GameMapAdditionalObject>();


    void Start()
    {
		foreach (GameMapAdditionalObject obj in ToAdd) {
			foreach(Entity e in obj.entities) {
				GlobalGame.MapManager.AddEntity(obj.GameMap, e);
			}
		}    
    }
	
    void Update()
    {
        
    }

	[System.Serializable]
	private class GameMapAdditionalObject {

		public string GameMap;
		public List<Entity> entities = new List<Entity>();

	}
}
