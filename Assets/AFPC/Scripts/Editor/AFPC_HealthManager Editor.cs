using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_HealthManager))]
[CanEditMultipleObjects]
public class AFPC_HealthManagerEditor : Editor {

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

		AFPC_HealthManager healthManager = (AFPC_HealthManager)target;
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ("NotificationBackground");

		showExtraFields.target = EditorGUILayout.Foldout (showExtraFields.target,"Health Settings", showExtraFields.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields.faded))
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			healthManager.health = EditorGUILayout.IntField ("Current Player Health: ", healthManager.health);
			healthManager.maxHealth = EditorGUILayout.IntField ("Maximum Player Health: ", healthManager.maxHealth);
			healthManager.minHealth = EditorGUILayout.IntField ("Minimum Player Health: ", healthManager.minHealth);
			healthManager.healthDecreaseScreenColor = EditorGUILayout.ColorField ("Health Decrease Screen Color: ", healthManager.healthDecreaseScreenColor);
			healthManager.timeToSmoothFillAmount = EditorGUILayout.FloatField ("Time To Smooth Health Bar Fill Amount: ", healthManager.timeToSmoothFillAmount);
			healthManager.useHealthDecreaseAudioEffects = EditorGUILayout.Toggle ("Use Health Decrease Audio Effects: ", healthManager.useHealthDecreaseAudioEffects);
			if (healthManager.useHealthDecreaseAudioEffects)
				healthManager.audioVolume = EditorGUILayout.FloatField ("Health Decrease Effect Audio Volume: ", healthManager.audioVolume);
			healthManager.hasFallDamage = EditorGUILayout.Toggle ("Apply Fall Damage: ", healthManager.hasFallDamage);
			if (healthManager.hasFallDamage) 
			{
				healthManager.heightInAitWithNoDamage = EditorGUILayout.FloatField ("Maximum Fall distance in Air With No Fall Damage: ", healthManager.heightInAitWithNoDamage);
				healthManager.damagePerUnitHeightInAir = EditorGUILayout.IntField ("Damage Per Unit Fall Distance In Air: ", healthManager.damagePerUnitHeightInAir);
			}
			healthManager.useTextToShowHealth = EditorGUILayout.Toggle ("Use Text To Show Health: ", healthManager.useTextToShowHealth);
			healthManager.useBarToShowHealth = EditorGUILayout.Toggle ("Use Bar To Show Health: ", healthManager.useBarToShowHealth);
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
			healthManager.healthDecreaseEffect = (Image)EditorGUILayout.ObjectField ("Health Decrease Effect Image: ", healthManager.healthDecreaseEffect, typeof(Image), true);
			healthManager.kickBack = (GameObject)EditorGUILayout.ObjectField ("KickBack Gameobject: ", healthManager.kickBack, typeof(GameObject), true);
			healthManager.healthMaster = (Image)EditorGUILayout.ObjectField ("Health Master Image: ", healthManager.healthMaster, typeof(Image), true);
			if(healthManager.useTextToShowHealth)
				healthManager.healthText = (Text)EditorGUILayout.ObjectField ("Health Text: ", healthManager.healthText, typeof(Text), true);
			if(healthManager.useBarToShowHealth)
				healthManager.healthBar = (Image)EditorGUILayout.ObjectField ("Health Bar Image: ", healthManager.healthBar, typeof(Image), true);
			if (healthManager.useHealthDecreaseAudioEffects)
				healthManager.healthDecreaseAudio = (AudioClip)EditorGUILayout.ObjectField ("Health Decrease Audio Effect: ", healthManager.healthDecreaseAudio, typeof(AudioClip), false);
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
			EditorUtility.SetDirty (healthManager);
	}
}
#endif
