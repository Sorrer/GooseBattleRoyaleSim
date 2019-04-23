using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseEntity : Entity
{


    // Start is called before the first frame update
    void Start()
    {
		if(this.EntityID.Equals("Goose")) {
			GlobalGame.MapManager.AddEntity(GameConfig.MAP_GEESE, this);
		}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
