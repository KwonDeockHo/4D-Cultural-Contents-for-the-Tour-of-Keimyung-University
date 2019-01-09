using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[HelpURL("http://www.illusionloop.com/docs/animatedsteelcoaster.html")][SelectionBase]
public class CoasterControllerSWLXR : MonoBehaviour {

	public bool info = true;
	[HideInInspector] [Range(1,4)] public int trains = 2;
	[HideInInspector] [Range(0.25f,2f)] public float animationSpeed = 1.0f;
	[Tooltip("Deactivated copy of a train. Used to instantiate all trains but the first one.")]
	public GameObject[] trainPrefab;
	[Tooltip("The train that is activated in editor. Use this train to add camera etc.")]
	public GameObject[] firstTrain;

	List<Animation> cartAnimations;
	List<int> trainIndices;

	public float[] startDelay = new float[]{0,48.5f,24.25f,17.25f};

	// Use this for initialization
	void Start () {
		if (Check()) {
		} else {
			return;
		}
		cartAnimations = new List<Animation> ();
		trainIndices = new List<int> ();
		for (int ct = 0; ct < trainPrefab.Length; ct++) {
			Animation anim = trainPrefab[ct].GetComponent<Animation>();
			foreach (AnimationState state in anim) {
				state.speed = animationSpeed;
			}
		}
		for (int ct = 0; ct < firstTrain.Length; ct++) {
			Animation anim = firstTrain[ct].GetComponent<Animation>();
			cartAnimations.Add(anim);
			trainIndices.Add(0);
			foreach (AnimationState state in anim) {
				state.speed = animationSpeed;
			}
		}

		for (int ct = 0; ct < trains-1; ct++) {
			for (int ctt = 0; ctt < trainPrefab.Length; ctt++) {
				GameObject go = Instantiate(trainPrefab[ctt], trainPrefab[ctt].transform.position,trainPrefab[ctt].transform.rotation) as GameObject;
				go.SetActive(true);
				go.GetComponent<AnimationDelay>().delay = startDelay[trains-1]*(ct+1);
				go.transform.parent = transform;

				Animation anim = go.GetComponent<Animation>();
				cartAnimations.Add(anim);
				trainIndices.Add(ct+1);

				foreach (AnimationState state in anim) {
					state.speed = animationSpeed;
				}
			}
		}

		this.enabled = false;
	}

	public void Reset(){
		if (cartAnimations != null) {
			foreach (Animation anim in cartAnimations) {
				bool isfirst = false;
				foreach (GameObject crt in firstTrain) {
					if(crt == anim.gameObject){
						isfirst = true;
					}
				}
				if(isfirst == false){
					Destroy(anim.gameObject);
				}
			}
			Start ();
			Rewind();

		}
	}

	public void Rewind(){
		if (cartAnimations != null) {
			foreach (Animation anim in cartAnimations) {
				AnimationDelay ad = anim.GetComponent<AnimationDelay>();
				if(ad!= null){
					foreach (AnimationState state in anim) {
						state.time = 0 + ad.delay;
					}
				}
			}
		}
	}

	public void FastForward(float time, bool normalizedTime){
		if (cartAnimations != null) {
			foreach (Animation anim in cartAnimations) {
				if (normalizedTime) {
					AnimationDelay ad = anim.GetComponent<AnimationDelay>();
					if(ad!= null){
						foreach (AnimationState state in anim) {
							state.normalizedTime = time + ad.delay;
						}
					}
				} else {
					AnimationDelay ad = anim.GetComponent<AnimationDelay>();
					if(ad!= null){
						foreach (AnimationState state in anim) {
							state.time = time + ad.delay;
						}
					}
				}
			}
		}
	}

	public void SetSpeed(float speed){
		if (cartAnimations != null) {
			foreach (Animation anim in cartAnimations) {
				foreach (AnimationState state in anim) {
					state.speed = speed;
				}
			}
		}
	}

	public bool Check(){
		if (trainPrefab == null) {
			Debug.Log ("Unable to execute.\n Train prefabs are missing. Try to reload prefab, reassign script or assign train prefabs manually.");
			return false;
		}
		for (int ct = 0; ct < trainPrefab.Length; ct++) {
			if (trainPrefab [ct] == null) {
				Debug.Log ("Unable to execute.\n Train prefab cart nr: " + ct.ToString () + " is missing. please assign a prefab.");
				return false;
			} else if (trainPrefab [ct].GetComponent<Animation> () == null || trainPrefab [ct].GetComponent<AnimationDelay> () == null) {
				Debug.Log ("Unable to execute.\n Animation or animation delay component is missing on prefab cart " + ct.ToString () + ". A prefab cart must use these components.");
				return false;
			}
		}
		return true;
	}
	public bool Check( out string message){
		if (trainPrefab == null) {
			message = "Unable to execute.\n Train prefabs are missing. Try to reload prefab, reassign script or assign train prefabs manually.";
			return false;
		}
		for (int ct = 0; ct < trainPrefab.Length; ct++) {
			if(trainPrefab[ct] == null){
				message = "train prefab cart nr: " + ct.ToString() + " is missing. please assign a prefab.";
				return false;
			} else if(trainPrefab[ct].GetComponent<Animation>()== null || trainPrefab[ct].GetComponent<AnimationDelay>()== null){
				message = "Animation or animation delay component is missing on prefab cart " + ct.ToString() + ". A prefab cart must use these components.";
				return false;
			}
		}
		message = "";
		return true;
	}

	// Update is called once per frame
	/*void Update () {
	
	}*/
}
