using UnityEngine;
using System.Collections;
using System.Reflection;

namespace IllusionLoop.CoasterPluginZF{
	[HelpURL("http://www.illusionloop.com/docs/animatedsteelcoaster.html")]
	public class CoasterStationControllerLXRZF : MonoBehaviour {
		[HideInInspector] public bool info = true;
		[HideInInspector] public bool editMode = false;
		public GameObject[] carts;
		public GameObject stationTrack;
		Vector3[] stpos;
		Quaternion[] strot;

		System.Type TrackCartZF;
		System.Type TrackZF;
		System.Type StationZF;
		// Use this for initialization
		void Start () {
			if (gameObject.activeInHierarchy == false)
				return;
			stpos = new Vector3[carts.Length];
			strot = new Quaternion[carts.Length];
			for (int ct = 0; ct < carts.Length; ct++) {
				stpos[ct] = carts[ct].transform.position;
				strot[ct] = carts[ct].transform.rotation;
			}
		}
		
		// Update is called once per frame
		public void ResetTrain () {
			if (gameObject.activeInHierarchy == false)
				return;
			if(CheckForZFTrack()){
				CoasterStationLXRZF station = GetComponent<CoasterStationLXRZF>();
				if(station != null){

					PropertyInfo ctr = TrackCartZF.GetProperty ("CurrentTrack");

					for (int ct = 0; ct < carts.Length; ct++) {
						carts[ct].GetComponent<Rigidbody>().velocity = Vector3.zero;
						carts[ct].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
						carts[ct].transform.position = stpos[ct];
						carts[ct].transform.rotation = strot[ct];

						ctr.SetValue(carts[ct].GetComponent(TrackCartZF),station.rail.GetComponent(TrackZF),null);
					}
				}else{
					Debug.LogWarning("failed to reset coaster train, station script could not be found or is not initialized correctly");
				}
			}else{
				Debug.LogWarning("failed to reset coaster train, ZFTrack is missing");
			}
		}

		public void SendTrain(){
			if (gameObject.activeInHierarchy == false)
				return;
			if (CheckForZFTrack ()) {
				stationTrack.GetComponent(StationZF).SendMessage("Send");
			}
		}

		bool CheckForZFTrack(){//check if tracks and rails plugin is installed and assign types
			if (gameObject.activeInHierarchy == false)
				return false;
			//are all required tracks + rails components there?
			if (System.Type.GetType ("ZenFulcrum.Track.TrackCart") != null && System.Type.GetType("ZenFulcrum.Track.Track") != null && System.Type.GetType("ZenFulcrum.Track.Station") != null) {
				TrackCartZF = System.Type.GetType ("ZenFulcrum.Track.TrackCart");
				TrackZF = System.Type.GetType("ZenFulcrum.Track.Track");
				StationZF = System.Type.GetType("ZenFulcrum.Track.Station");
				//all tracks and rails components were found and assigned -> return true
				return true;
			}
			//if components are missing -> reset and return false
			TrackCartZF = null;
			TrackZF = null;
			StationZF = null;

			return false;
		}
	}
}
