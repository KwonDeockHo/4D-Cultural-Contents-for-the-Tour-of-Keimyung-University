/*
 * Copyright 2017 IllusionLoop UG
 * info@illusionloop.com
 * */
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace IllusionLoop.CoasterPluginZF{

	[CustomEditor(typeof(CoasterStationLXRZF))]
	public class CoasterStationUILXRZF : Editor {

		public override void OnInspectorGUI(){
			CoasterStationLXRZF station = (CoasterStationLXRZF)target;
			if (station.info) {
				//inspector when initialization is complete
				if (station.getState () == CoasterStationLXRZF.State.Done) {
					EditorGUILayout.HelpBox ("Quick start guide below. For more detailed instructions and information click the help icon or open the documentation / readme file.", MessageType.Info);

					EditorGUILayout.HelpBox ("Select the end of the coaster rail to start editing the track.\n" +
						"You can adjust the lift speed and force by changing the 'Acceleration' parameters of the 'Track' component.\n" +
						"\nTo add a new curve click the 'Create Next +' button on the track editing panel in the inspector.\n" +
						"\nTo finish the track click 'Snap to Nearest' on the track editing panel in the inspector.\n" +
						"\nTo change the track meshes use one of the rail models from the folder:\n" +
						"'Assets/Laxer Assets/Coaster/Tracks And Rails Plugin/Models/\n" +
						"Click the expand arrow of one of these models to reveale the 'rail' or 'tie' mesh.\n" +
						"Select a track piece and drag one of these meshes into the 'Rail Mesh' or 'Tie Mesh' slot of the 'Track' component", MessageType.Info);
					EditorGUILayout.HelpBox ("Do not 'Apply' or 'Revert' this prefab!", MessageType.Warning, true);
			
					//inspector when prefab is still in asset browser
				} else if (station.getState () == CoasterStationLXRZF.State.NotLoaded) {
					EditorGUILayout.HelpBox ("Drag this prefab into your scene. It will automatically add all required components of 'Tracks and Rails' so that you can start to create the track. \nDo not modify this prefab! For the case, that this brefab has been changed, there is a backup version in the 'backup.zip' file.", MessageType.Info);
					EditorGUILayout.HelpBox ("Click the button below to visit the asset store page for 'Tracks and Rails' by Zen Fulcrum LLC.", MessageType.Info);
					if (GUILayout.Button ("Visit Asset Store")) {
						if (EditorUtility.DisplayDialogComplex ("Visit Asset Store?", "Click 'OK' to open the Asset Store webpage for 'Tracks and Rails' by Zen Fulcrum LLC\n" +
							"https://www.assetstore.unity3d.com/en/#!/content/33512", "OK", "Cancel", "NO!") == 0) {
							Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/content/33512");
						}
					}

					//inspector when initialization failed
				} else if (station.getState () == CoasterStationLXRZF.State.ZFMissing) {
					EditorGUILayout.HelpBox ("'Tracks and Rails' asset was not found. Please make sure 'Tracks and Rails' by ZenFulcrum LLC is installed properly and click 'retry'.", MessageType.Error);
					if (GUILayout.Button (new GUIContent ("Retry", "Retry to find 'tracks and rails' asset and add required components."))) {
						int returnValue = station.Initialize ();
						switch (returnValue) {
						case -1: //not all components found
							EditorUtility.DisplayDialog ("Error: Missing Components", "It seems like some components of 'Tracks and Rails' could not be found. Please make sure the asset is installed correctly.", "OK");
							break;
						case 0: // zf asset missing
							EditorUtility.DisplayDialog ("Error: Missing Components", "It seems like some components of 'Tracks and Rails' could not be found. Please make sure the asset is installed correctly.", "OK");
							break;
						case 1: // already finished
							EditorUtility.DisplayDialog ("Error: already initialized", "Prefab initialization already complete", "OK");
							break;
						case 2: // completed successfully
							Debug.Log ("Successfully imported coaster prefab. Select the station for more information.", station.gameObject);
							break;
						}
					}
					EditorGUILayout.HelpBox ("Click the button below to visit the asset store page for 'Tracks and Rails' by Zen Fulcrum LLC.", MessageType.Info);
					if (GUILayout.Button ("Visit Asset Store")) {
						if (EditorUtility.DisplayDialogComplex ("Visit Asset Store?", "Click 'OK' to open the Asset Store webpage for 'Tracks and Rails' by Zen Fulcrum LLC\n" +
							"https://www.assetstore.unity3d.com/en/#!/content/33512", "OK", "Cancel", "NO!") == 0) {
							Application.OpenURL ("https://www.assetstore.unity3d.com/en/#!/content/33512");
						}
					}
				}
				//DrawDefaultInspector ();
			}
			if (station.editMode) {
				DrawDefaultInspector();
			}
			GUILayout.BeginHorizontal ();
			station.info = GUILayout.Toggle(station.info,"info");
			station.editMode = GUILayout.Toggle(station.editMode,"edit mode");
			GUILayout.EndHorizontal ();
		}
	}//end class

	[CustomEditor(typeof(CoasterStationControllerLXRZF))]
	public class CoasterStationControlUILXRZF : Editor {
		public override void OnInspectorGUI(){
			CoasterStationControllerLXRZF station = (CoasterStationControllerLXRZF)target;

			if (station.info) {
				EditorGUILayout.HelpBox ("You can call the following methods of this script at runtime or click the buttons below: \n" +
					"ResetTrain()\n" +
					"SendTrain()", MessageType.Info);
				if(GUILayout.Button("Reset Train")){
					station.ResetTrain();
				}
				if(GUILayout.Button("Send Train")){
					station.SendTrain();
				}

			}
			if (station.editMode) {
				DrawDefaultInspector();
			}
			GUILayout.BeginHorizontal ();
			station.info = GUILayout.Toggle(station.info,"info");
			station.editMode = GUILayout.Toggle(station.editMode,"edit mode");
			GUILayout.EndHorizontal ();
		}

	}//end class
}//end namespace
#endif
