using UnityEngine;
using System.Collections;

public class AudioVelocityControl : MonoBehaviour {
	Vector3 lastpos = new Vector3();
	public AudioSource[] a_sources;
	public AudioSource lift_sound;
	public float[] min_pitch;
	public float[] min_volume;
	public float[] pitch_per_velocity;
	// Use this for initialization
	void Start () {
		lastpos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float diff = (transform.position - lastpos).magnitude / Time.deltaTime;
		lastpos = transform.position;
		for (int ct = 0; ct < a_sources.Length; ct++) {
			a_sources[ct].pitch = Mathf.Lerp(a_sources[ct].pitch,min_pitch[ct]+diff*pitch_per_velocity[ct],0.04f);
			a_sources[ct].volume = Mathf.Lerp(a_sources[ct].volume,Mathf.Clamp01(diff*pitch_per_velocity[ct])*7f+min_volume[ct],0.04f);
		}
	}

	public void EnableLift(){
		lift_sound.enabled = true;
	}
	public void DisableLift(){
		lift_sound.enabled = false;
	}
}
