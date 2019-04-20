using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseBehavious : MonoBehaviour
{
    public SimpleNavigation navEle;
    public SphereCollider sC;
    public GameObject thisChild;

    public List<GameObject> gooseInRange = new List<GameObject>();
    private bool gooseAdded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gooseAdded)
        {
            SetToAttackGoose();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Goose") && collision.gameObject != thisChild)
        {
            Debug.Log("Found a goose");
            gooseInRange.Add(collision.gameObject);
            gooseAdded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gooseInRange.Contains(other.gameObject))
        {
            gooseInRange.Remove(other.gameObject);
        }
    }

    private void SetToAttackGoose()
    {
        if (gooseInRange.Count > 0)
        {
            int randIndex = Random.Range(0, gooseInRange.Count);
            navEle.thisAgent.SetDestination(gooseInRange[randIndex].transform.position);
            Debug.Log(gooseInRange[randIndex].transform.position);
        }
    }
}
