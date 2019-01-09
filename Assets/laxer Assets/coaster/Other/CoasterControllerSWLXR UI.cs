#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CoasterControllerSWLXR))]
public class CoasterControllerSWLXRUI : Editor {
	bool toggleAdvanced = false;
	bool toggleRuntime = false;
	float fftime = 0;
	bool ffnormalized = false;
	float aspeed = 1;
	public override void OnInspectorGUI(){
		CoasterControllerSWLXR ccontroller = (CoasterControllerSWLXR)target;


		if (ccontroller.info == true) {
			string msg = "";
			if(ccontroller.Check(out msg) == false)
				EditorGUILayout.HelpBox (msg, MessageType.Error);
			else{
				EditorGUILayout.HelpBox ("Use the sliders to adjust the coaster settings.\nIf you use the coaster prefab, you have to click the button below before starting the game. Otherwise settings will be reverted.\nSettings will be applied at the start of the game.\nPlease read the documentation before using advanced settings.", MessageType.Info);
			}

		}
		ccontroller.trains = EditorGUILayout.IntSlider ("Trains", ccontroller.trains, 1, 4);
		ccontroller.animationSpeed = EditorGUILayout.Slider ("Animation Speed", ccontroller.animationSpeed, 0.25f, 2f);
		if (GUILayout.Button ("Break prefab connection")) {
			PrefabUtility.DisconnectPrefabInstance( ccontroller.gameObject);
		}

		toggleAdvanced =  EditorGUILayout.Foldout(toggleAdvanced,"Advanced Options");

		if (toggleAdvanced) {
			DrawDefaultInspector ();
		}
		toggleRuntime =  EditorGUILayout.Foldout(toggleRuntime,"Runtime Methods");
		if (toggleRuntime) {
			if (GUILayout.Button ("Reset()")) {
				ccontroller.Reset();
			}
			if (GUILayout.Button ("Rewind()")) {
				ccontroller.Rewind();
			}
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button (new GUIContent("FastForward()","FastForward(float time, bool normalizedTime) // fast forwards all animations to 'time'. NormalizedTime uses a time value between 0 and 1"))) {
				ccontroller.FastForward(fftime, ffnormalized);
			}
			if(ffnormalized){
				fftime = EditorGUILayout.Slider(fftime,0f,1f);
			}else{
			fftime = EditorGUILayout.Slider(fftime,0f,69.007f);
			}
			ffnormalized = EditorGUILayout.Toggle("normalized",ffnormalized);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button (new GUIContent("SetSpeed()","SetSpeed(float speed) // change animation speed"))) {
				ccontroller.SetSpeed(aspeed);
			}
			aspeed = EditorGUILayout.Slider(aspeed,-2f,2f);
			EditorGUILayout.EndHorizontal();
		}

	}
}
#endif