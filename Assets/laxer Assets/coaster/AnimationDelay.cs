using UnityEngine;
using System.Collections;

public class AnimationDelay : MonoBehaviour {
	public float delay = 30;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Animation>()[GetComponent<Animation>().clip.name].time = delay;
		this.enabled = false;
	}
}
