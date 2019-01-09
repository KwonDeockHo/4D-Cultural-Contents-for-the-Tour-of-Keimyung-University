using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour {

    private AudioSource sound;
	// Use this for initialization
	void Start () {
        sound = GetComponent<AudioSource>();	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Coater"))
            sound.Play();
            
    }
}
