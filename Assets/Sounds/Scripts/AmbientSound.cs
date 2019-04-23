using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public AudioClip[] ambience;

    private AudioSource amAdS;

    // Start is called before the first frame update
    void Start()
    {
        amAdS = gameObject.GetComponent<AudioSource>();
        if(ambience.Length > 0)
        {
            amAdS.clip = ambience[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!amAdS.isPlaying)
        {
            amAdS.clip = GetRandAudioClip();
            amAdS.Play();
        }
    }

    private AudioClip GetRandAudioClip()
    {
        int randIndex = Random.Range(0, ambience.Length);
        return ambience[randIndex];
    }
}
