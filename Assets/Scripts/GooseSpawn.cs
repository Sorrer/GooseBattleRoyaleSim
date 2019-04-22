using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseSpawn : MonoBehaviour
{

	public int AmountOfGeese = 10;
	public GameObject GooseObject;
	public Transform GeeseFolder;


	void Start() {
		SpawnAllGeese();
	}

    // Update is called once per frame
    void Update()
    {
        
    }


	void SpawnAllGeese() {
		for(int i =0; i < AmountOfGeese; i++) {
			SpawnGoose();
		}
	}

	void SpawnGoose() {

		Vector3 pos = Vector3.zero;
		GlobalGame.RandomNavMeshPos(ref pos);

		GameObject obj = Instantiate(GooseObject, GeeseFolder, false);
		cloneTransforms(obj.transform, GooseObject.transform);
		obj.transform.position = pos;
	}

	void cloneTransforms(Transform newPar, Transform oldPar) {
		//newPar.transform.localPosition = oldPar.transform.localPosition;
		newPar.transform.rotation = oldPar.transform.rotation;
		for (int i = 0; i < newPar.childCount; i++) {
			cloneTransforms(newPar.GetChild(i), oldPar.GetChild(i));
		}
	}
}
