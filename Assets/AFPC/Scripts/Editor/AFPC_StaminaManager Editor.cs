using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_StaminaManager))]
[CanEditMultipleObjects]
public class AFPC_StaminaManagerEditor : Editor {

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

		AFPC_StaminaManager staminaManager = (AFPC_StaminaManager)target;
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ("NotificationBackground");

		showExtraFields.target = EditorGUILayout.Foldout (showExtraFields.target,"Stamina Settings", showExtraFields.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields.faded))
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			staminaManager.currentStamina = EditorGUILayout.IntField ("Current Player Stamina: ", staminaManager.currentStamina);
			staminaManager.maxStamina = EditorGUILayout.IntField ("Maximum Player Stamina: ", staminaManager.maxStamina);
			staminaManager.minStamina = EditorGUILayout.IntField ("Minimum Player Stamina: ", staminaManager.minStamina);
			staminaManager.timeToSmoothFillAmount = EditorGUILayout.FloatField ("Time To Smooth Stamina Bar Fill Amount: ", staminaManager.timeToSmoothFillAmount);
			staminaManager.useGaspSound = EditorGUILayout.Toggle ("Use Stamina Decrease Gasp Audio Effect: ", staminaManager.useGaspSound);
			if (staminaManager.useGaspSound)
				staminaManager.gaspVolume = EditorGUILayout.FloatField ("Stamina Decrease Gasp Audio Volume: ", staminaManager.gaspVolume);
			staminaManager.useTextToShowStamina = EditorGUILayout.Toggle ("Use Text To Show Stamina: ", staminaManager.useTextToShowStamina);
			staminaManager.useBarToShowStamina = EditorGUILayout.Toggle ("Use Bar To Show Stamina: ", staminaManager.useBarToShowStamina);
			staminaManager.hideStaminaBar = EditorGUILayout.Toggle ("Hide Stamina Bar: ", staminaManager.hideStaminaBar);
			if (staminaManager.hideStaminaBar) 
			{
				staminaManager.hidePosition = EditorGUILayout.Vector3Field ("Stamina Bar Hide Position: ", staminaManager.hidePosition);
				staminaManager.timeToHideStaminaBar = EditorGUILayout.FloatField ("Time To Hide Stamina Bar: ", staminaManager.timeToHideStaminaBar);
			}
			staminaManager.staminaToDecrease = EditorGUILayout.IntField ("Stamina To Decrease: ", staminaManager.staminaToDecrease);
			staminaManager.staminaDecreaseTime = EditorGUILayout.FloatField ("Time To Decrease Stamina: ", staminaManager.staminaDecreaseTime);
			staminaManager.staminaToIncrease = EditorGUILayout.IntField ("Stamina To Increase: ", staminaManager.staminaToIncrease);
			staminaManager.staminaIncreaseTime = EditorGUILayout.FloatField ("Time To Increase Stamina: ", staminaManager.staminaIncreaseTime);
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
			staminaManager.staminaMaster = (Image)EditorGUILayout.ObjectField ("Stamina Master Image: ", staminaManager.staminaMaster, typeof(Image), true);
			if(staminaManager.useTextToShowStamina)
				staminaManager.staminaText = (Text)EditorGUILayout.ObjectField ("Stamina Text: ", staminaManager.staminaText, typeof(Text), true);
			if(staminaManager.useBarToShowStamina)
				staminaManager.staminaBar = (Image)EditorGUILayout.ObjectField ("Stamina Bar Image: ", staminaManager.staminaBar, typeof(Image), true);
			if (staminaManager.useGaspSound)
				staminaManager.gaspSound = (AudioClip)EditorGUILayout.ObjectField ("Stamina Decrease Gasp Audio Effect: ", staminaManager.gaspSound, typeof(AudioClip), false);
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
			EditorUtility.SetDirty (staminaManager);
	}
}
#endif
