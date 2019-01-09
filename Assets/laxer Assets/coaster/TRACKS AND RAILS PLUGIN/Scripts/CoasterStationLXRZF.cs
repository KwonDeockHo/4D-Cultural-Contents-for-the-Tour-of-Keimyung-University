/*
 * Copyright 2017 IllusionLoop UG
 * info@illusionloop.com
 * */

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
//using System;

namespace IllusionLoop.CoasterPluginZF{

	[ExecuteInEditMode][SelectionBase][HelpURL("http://www.illusionloop.com/docs/animatedsteelcoaster.html")]
	public class CoasterStationLXRZF : MonoBehaviour {
		[HideInInspector] public bool info = true;
		[HideInInspector] public bool editMode = false;

		[Range(0,1)]public float curvePreservation = 0.7f;//not visible, built in maybe make this available in future
		public GameObject[] carts;
		public GameObject rail;
		public Mesh liftMesh;
		public Mesh liftTieMesh;
		public Mesh railMesh;
		public Mesh railTieMesh;
		public Material railMaterial;
		System.Type TrackCartZF;
		System.Type TrackZF;
		System.Type SimpleTransformZF;
		System.Type SpeedAndForceZF;
		System.Type StationZF;


		public enum State{NotLoaded, ZFMissing, Done};
		/*DON'T MESS WITH THIS!!!
		 * Just DON'T!
		 * */
		[HideInInspector][SerializeField]
		private State state = State.NotLoaded;
		
		// Use this for initialization
		void Start () {
			Initialize ();
		}
		//check for plugins and create all components
		public int Initialize(){//toDo: check only on editor startup or manually toDo: check all children before adding components
			if (state == State.Done) {
				return 1;//don't execute, if initialization is already complete
			}
			//check if tracks and rails components are installed
			if (CheckForZFTrack ()) {
			} else {
				return 0;//ZFtrack components missing -> show warning (inspector script)
			}
			//if all components were found -> add components to prefab; toDo: check all fields for null / wrong input
			if (TrackCartZF != null && TrackZF != null && SimpleTransformZF != null && StationZF != null) {
				
				//toDo: don't add components, if they are already there
				/*
				 * add a "track" component and all required components to the "rail" object.
				 * This is an invisible track that goes through the station.
				 * Used only for physics. Empty mesh.
				 * */
				#region build tracks
				MeshFilter mf = rail.AddComponent<MeshFilter> ();
				mf.hideFlags = HideFlags.NotEditable;
				//MeshRenderer mr = rail.AddComponent<MeshRenderer> ();
				rail.AddComponent<MeshRenderer> ();

				//get fields methods etc. here
				FieldInfo et = TrackZF.GetField ("endTransform");//required to set track length
				FieldInfo ti = TrackZF.GetField ("tieInterval");//required to set tie interval of the lift track
				FieldInfo rm = TrackZF.GetField ("railMesh");//used to set rail mesh of track component
				FieldInfo tm = TrackZF.GetField ("tieMesh");//used to set rail tie mesh of track component
				FieldInfo ac = TrackZF.GetField ("acceleration");//used to set rail tie mesh of track component
				FieldInfo brk = TrackZF.GetField ("brakes");//used to set rail tie mesh of track component
				PropertyInfo previousTrackField = TrackZF.GetProperty ("PrevTrack");//used to connect tracks
				PropertyInfo nextTrackField = TrackZF.GetProperty ("NextTrack");//used to connect tracks
				
				
				Object tr = rail.AddComponent (TrackZF);
				//set length of station track to 30 - matches the station prefab length
				et.SetValue (tr, System.Activator.CreateInstance (SimpleTransformZF, new Vector3 (0, 0, 30), new Quaternion ()));
				
				//create lift -> copy previous track and connect
				//toDo: make this part procedural?
				GameObject lift = Instantiate (rail, rail.transform.position + rail.transform.forward * 30, rail.transform.rotation) as GameObject;
				lift.transform.parent = transform;
				Object liftTrack = lift.GetComponent (TrackZF);
				//assign lift meshes
				rm.SetValue (liftTrack, liftMesh);
				tm.SetValue (liftTrack, liftTieMesh);
				ti.SetValue (liftTrack, 1);
				et.SetValue (liftTrack, System.Activator.CreateInstance (SimpleTransformZF, new Vector3 (0, 7, 18), Quaternion.Euler (-45, 0, 0)));
				//adjust lift speed and force
				object sAFLift = System.Activator.CreateInstance (SpeedAndForceZF);
				FieldInfo sfSpeed = SpeedAndForceZF.GetField("targetSpeed");
				FieldInfo sfForce = SpeedAndForceZF.GetField("maxForce");
				sfSpeed.SetValue(sAFLift,6);
				sfForce.SetValue(sAFLift,50);
				ac.SetValue (liftTrack, sAFLift);
				previousTrackField.SetValue (liftTrack, tr, null);
				nextTrackField.SetValue (tr, liftTrack, null);
				
				lift.GetComponent<MeshRenderer> ().material = railMaterial;

				//lift part 2
				GameObject lift2 = Instantiate (lift, lift.transform.TransformPoint(0,7,18), lift.transform.rotation*Quaternion.Euler (-45, 0, 0) ) as GameObject;
				lift2.transform.parent = transform;
				Object lift2Track = lift2.GetComponent (TrackZF);
				//assign lift data
				et.SetValue (lift2Track, System.Activator.CreateInstance (SimpleTransformZF, new Vector3 (0, 0, 30), Quaternion.Euler (0, 0, 0)));
				previousTrackField.SetValue (lift2Track, liftTrack, null);
				nextTrackField.SetValue (liftTrack, lift2Track, null);

				//lift part 3
				GameObject lift3 = Instantiate (lift2, lift2.transform.TransformPoint(0,0,30), lift2.transform.rotation*Quaternion.Euler (0, 0, 0) ) as GameObject;
				lift3.transform.parent = transform;
				Object lift3Track = lift3.GetComponent (TrackZF);
				//assign lift data
				et.SetValue (lift3Track, System.Activator.CreateInstance (SimpleTransformZF, new Vector3 (0, -7, 18), Quaternion.Euler (45, 0, 0)));
				previousTrackField.SetValue (lift3Track, lift2Track, null);
				nextTrackField.SetValue (lift2Track, lift3Track, null);

				//lift end
				GameObject lift4 = Instantiate (lift3, lift3.transform.TransformPoint(0,-7,18), lift3.transform.rotation*Quaternion.Euler (45, 0, 0) ) as GameObject;
				lift4.transform.parent = transform;
				Object lift4Track = lift4.GetComponent (TrackZF);
				//assign lift data
				object sAFRail = System.Activator.CreateInstance (SpeedAndForceZF);
				sfSpeed.SetValue(sAFRail,0);
				sfForce.SetValue(sAFRail,0);
				ac.SetValue (lift4Track, sAFRail);
				rm.SetValue (lift4Track, railMesh);
				tm.SetValue (lift4Track, railTieMesh);
				et.SetValue (lift4Track, System.Activator.CreateInstance (SimpleTransformZF, new Vector3 (0, -14, 13), Quaternion.Euler (70, 0, 0)));
				previousTrackField.SetValue (lift4Track, lift3Track, null);
				nextTrackField.SetValue (lift3Track, lift4Track, null);
				

				//create previous track -> copy track and connect
				GameObject feedRail = Instantiate (rail, rail.transform.position - rail.transform.forward * 30, rail.transform.rotation) as GameObject;
				feedRail.transform.parent = transform;
				Object feedRailTrack = feedRail.GetComponent (TrackZF);
				//assign lift meshes
				rm.SetValue (feedRailTrack, railMesh);
				tm.SetValue (feedRailTrack, railTieMesh);
				ti.SetValue (feedRailTrack, 1);
				//create brakes
				object sAFBrake = System.Activator.CreateInstance (SpeedAndForceZF);
				sfSpeed.SetValue(sAFBrake,15);
				sfForce.SetValue(sAFBrake,50);
				brk.SetValue (feedRailTrack, sAFBrake);
				ac.SetValue (feedRailTrack, sAFBrake);
				previousTrackField.SetValue (tr, feedRailTrack, null);
				nextTrackField.SetValue (feedRailTrack, tr, null);
				
				feedRail.GetComponent<MeshRenderer> ().material = railMaterial;
				#endregion

				//set lift active -> show handles etc [removed, does not work with selection base]
				//Selection.activeObject = lift;
				
				
				/*
				 * Add "station" component to "rail" object.
				 * This makes the train start and stop
				 * "cartsToStop" is set later in the carts section
				 * */
				Object station = rail.AddComponent (StationZF);
				
				
				/*
				 * Add "TrackCart" components to all carts
				 * coaster models are reversed, so set the "cartReversed" field to true
				 * */
				FieldInfo cp = TrackCartZF.GetField ("curvePreservation");
				FieldInfo cd = TrackCartZF.GetField ("clearingDistance");
				FieldInfo cr = TrackCartZF.GetField ("cartReversed");
				PropertyInfo ctr = TrackCartZF.GetProperty ("CurrentTrack");
				FieldInfo stopCart = StationZF.GetField ("cartsToStop");

				for (int ct = 0; ct < carts.Length; ct++) {//iterate through carts of the prefab
					Object no = carts [ct].AddComponent (TrackCartZF);
					cp.SetValue (no, curvePreservation);//curve preservation
					cr.SetValue (no, true);//cart reversed
					cd.SetValue (no, 1.5f);//prevent cart fromm colliding with the track, when it falls off
					ctr.SetValue (no, tr, null);//set current track to the station track
					//make the station stop the first cart
					if (ct == carts.Length - 1) {
						Object[] stopCarts = (Object[])System.Array.CreateInstance (TrackCartZF, 1);
						stopCarts.SetValue (no, 0);
						stopCart.SetValue (station, stopCarts);
					}
				}
				
				
				//finish initialization (don't execute again)
				PrefabUtility.DisconnectPrefabInstance (gameObject);
				//this.hideFlags = HideFlags.NotEditable;
				state = State.Done;
				this.enabled = false;

			} else {
				return -1;//some components missing -> show error(inspector script)
			}

			return 2;
		}
		
		bool CheckForZFTrack(){//check if tracks and rails plugin is installed and assign types
			//are all required tracks + rails components there?
			if (System.Type.GetType ("ZenFulcrum.Track.TrackCart") != null && System.Type.GetType("ZenFulcrum.Track.Track") != null && System.Type.GetType("ZenFulcrum.Track.SimpleTransform") != null && System.Type.GetType("ZenFulcrum.Track.Station") != null) {
				TrackCartZF = System.Type.GetType ("ZenFulcrum.Track.TrackCart");
				TrackZF = System.Type.GetType("ZenFulcrum.Track.Track");
				SimpleTransformZF = System.Type.GetType("ZenFulcrum.Track.SimpleTransform");
				SpeedAndForceZF = TrackZF.GetNestedType("SpeedAndForce");
				StationZF = System.Type.GetType("ZenFulcrum.Track.Station");
				//all tracks and rails components were found and assigned -> return true
				return true;
			}
			//if components are missing -> reset and return false
			TrackCartZF = null;
			TrackZF = null;
			StationZF = null;
			SimpleTransformZF = null;

			state = State.ZFMissing;
			return false;
		}

		public State getState(){//used for inspector. return true, when all tracks and rails objects have been added.
			return state;
		}

		void OnDrawGizmos(){//toDo: draw info or error icon here. Didn't find out how to include gizmos in an asset package -> removed

		}
	}//end class
}//end namespace
#endif