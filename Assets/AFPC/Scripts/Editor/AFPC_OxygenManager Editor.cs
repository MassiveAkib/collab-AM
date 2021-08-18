using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_OxygenManager))]
[CanEditMultipleObjects]
public class AFPC_OxygenManagerEditor : Editor {

	private AnimBool showExtraFields, showExtraFields1;

	private void OnEnable()
	{
		showExtraFields = new AnimBool (true);
		showExtraFields.valueChanged.AddListener (Repaint);
		showExtraFields1 = new AnimBool (true);
		showExtraFields1.valueChanged.AddListener (Repaint);
	}
	public override void OnInspectorGUI()
	{

		AFPC_OxygenManager oxygenManager = (AFPC_OxygenManager)target;
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ("NotificationBackground");

		showExtraFields.target = EditorGUILayout.Foldout (showExtraFields.target,"Oxygen Settings", showExtraFields.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields.faded))
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			oxygenManager.currentOxygen = EditorGUILayout.IntField ("Current Player Oxygen: ", oxygenManager.currentOxygen);
			oxygenManager.maxOxygen = EditorGUILayout.IntField ("Maximum Player Oxygen: ", oxygenManager.maxOxygen);
			oxygenManager.minOxygen = EditorGUILayout.IntField ("Minimum Player Oxygen: ", oxygenManager.minOxygen);
			oxygenManager.timeToSmoothFillAmount = EditorGUILayout.FloatField ("Time To Smooth Oxygen Bar Fill Amount: ", oxygenManager.timeToSmoothFillAmount);
			oxygenManager.useoxygenRestoreSound = EditorGUILayout.Toggle ("Use Oxygen Restore Audio Effect: ", oxygenManager.useoxygenRestoreSound);
			if (oxygenManager.useoxygenRestoreSound)
				oxygenManager.oxygenRestoreVolume = EditorGUILayout.FloatField ("Oxygen Restore Audio Volume: ", oxygenManager.oxygenRestoreVolume);
			oxygenManager.useTextToShowOxygen = EditorGUILayout.Toggle ("Use Text To Show Oxygen: ", oxygenManager.useTextToShowOxygen);
			oxygenManager.useBarToShowOxygen = EditorGUILayout.Toggle ("Use Bar To Show Oxygen: ", oxygenManager.useBarToShowOxygen);
			oxygenManager.decreaseHealthOnLowOxygen = EditorGUILayout.Toggle ("Decrease Health on Reaching minOxygen: ", oxygenManager.decreaseHealthOnLowOxygen);
			if (oxygenManager.decreaseHealthOnLowOxygen)
				oxygenManager.amountToDecreaseHealthOnminOxygen = EditorGUILayout.IntField ("Amount Of Health To Decrease On Low Oxygen: ", oxygenManager.amountToDecreaseHealthOnminOxygen);
			oxygenManager.hideOxygenBar = EditorGUILayout.Toggle ("Hide Oxygen Bar: ", oxygenManager.hideOxygenBar);
			if (oxygenManager.hideOxygenBar) 
			{
				oxygenManager.hidePosition = EditorGUILayout.Vector3Field ("Oxygen Bar Hide Position: ", oxygenManager.hidePosition);
				oxygenManager.timeToHideOxygenBar = EditorGUILayout.FloatField ("Time To Hide Oxygen Bar: ", oxygenManager.timeToHideOxygenBar);
			}
			oxygenManager.oxygenToDecrease = EditorGUILayout.IntField ("Oxygen To Decrease: ", oxygenManager.oxygenToDecrease);
			oxygenManager.oxygenDecreaseTime = EditorGUILayout.FloatField ("Time To Decrease Oxygen: ", oxygenManager.oxygenDecreaseTime);
			oxygenManager.oxygenToIncrease = EditorGUILayout.IntField ("Oxygen To Increase: ", oxygenManager.oxygenToIncrease);
			oxygenManager.oxygenIncreaseTime = EditorGUILayout.FloatField ("Time To Increase Oxygen: ", oxygenManager.oxygenIncreaseTime);
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();

		GUILayout.Box (GUIContent.none, "horizontalSlider");


		showExtraFields1.target = EditorGUILayout.Foldout (showExtraFields1.target,"References", showExtraFields1.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields1.faded))
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			oxygenManager.oxygenMaster = (Image)EditorGUILayout.ObjectField ("Oxygen Master Image: ", oxygenManager.oxygenMaster, typeof(Image), true);
			if(oxygenManager.useTextToShowOxygen)
				oxygenManager.oxygenText = (Text)EditorGUILayout.ObjectField ("Oxygen Text: ", oxygenManager.oxygenText, typeof(Text), true);
			if(oxygenManager.useBarToShowOxygen)
				oxygenManager.oxygenBar = (Image)EditorGUILayout.ObjectField ("Oxygen Bar Image: ", oxygenManager.oxygenBar, typeof(Image), true);
			if (oxygenManager.useoxygenRestoreSound)
				oxygenManager.oxygenRestoreSound = (AudioClip)EditorGUILayout.ObjectField ("Oxygen Restore Audio Effect: ", oxygenManager.oxygenRestoreSound, typeof(AudioClip), false);
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();

		GUILayout.Box (GUIContent.none, "horizontalSlider");
		EditorGUILayout.EndVertical ();
		if (EditorGUI.EndChangeCheck ()) 
		{
			Undo.RegisterCompleteObjectUndo (target, "Changed Settings");
		}
		if (GUI.changed)
			EditorUtility.SetDirty (oxygenManager);
	}
}
#endif