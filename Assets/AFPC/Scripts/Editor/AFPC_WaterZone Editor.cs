using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_WaterZone))]
[CanEditMultipleObjects]
public class AFPC_WaterZoneEditor : Editor {

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

		AFPC_WaterZone waterZone = (AFPC_WaterZone)target;
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ("NotificationBackground");
		showExtraFields.target = EditorGUILayout.Foldout (showExtraFields.target, "Water Settings", showExtraFields.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields.faded)) {
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			waterZone.fogColor = EditorGUILayout.ColorField ("Fog Color: ", waterZone.fogColor);
			waterZone.fogDensity = EditorGUILayout.FloatField ("Fog Density: ", waterZone.fogDensity);
			waterZone.fogMode = (FogMode)EditorGUILayout.EnumPopup ("Fog Mode: ", waterZone.fogMode, "ExposablePopupMenu");
			waterZone.flipPlane = EditorGUILayout.Toggle ("Flip Water Plane: ", waterZone.flipPlane);
			waterZone.useWaterSoundEffects = EditorGUILayout.Toggle ("Use Water Sound Effects: ", waterZone.useWaterSoundEffects);
			waterZone.useWaterSplashesAndRipples = EditorGUILayout.Toggle ("Use Water Splashes And Ripples: ", waterZone.useWaterSplashesAndRipples);
			if (waterZone.useWaterSoundEffects) {
				waterZone.timeToReapeatPlayerMovementSound = EditorGUILayout.FloatField ("Time To Repeat Player Movement Sound On Water Surface: ", waterZone.timeToReapeatPlayerMovementSound);
				waterZone.audioVolume = EditorGUILayout.FloatField ("Audio Volume: ", waterZone.audioVolume);
			}
			if (waterZone.useWaterSplashesAndRipples) {
				waterZone.magnitudeOfVelocityToInstantiateWaterEnterSplash = EditorGUILayout.FloatField ("Y Component of Velocity Above Which Instantiate Water Enter Splash: ", waterZone.magnitudeOfVelocityToInstantiateWaterEnterSplash);
				waterZone.waterEnterSplashEffectDestroyTime = EditorGUILayout.FloatField ("Water Enter Splash Effect Destroy Time: ", waterZone.waterEnterSplashEffectDestroyTime);
				waterZone.waterMovementSplashEffectDestroyTime = EditorGUILayout.FloatField ("Water Movement Splash Effect Destroy Time: ", waterZone.waterMovementSplashEffectDestroyTime);
				waterZone.waterStaticRippleEffectDestroyTime = EditorGUILayout.FloatField ("Water Static Ripple Effect Destroy Time: ", waterZone.waterStaticRippleEffectDestroyTime);
				EditorGUILayout.HelpBox ("Set the values of Destory Time of Effects same as the duration of the respective particle effects or just leave them", MessageType.Info, true);
			}
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();

		GUILayout.Box (GUIContent.none, "horizontalSlider");

		showExtraFields1.target = EditorGUILayout.Foldout (showExtraFields1.target, "References", showExtraFields1.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields1.faded)) {
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			waterZone.playerCamera = (Camera)EditorGUILayout.ObjectField ("Player Camera: ", waterZone.playerCamera, typeof(Camera), true);
			waterZone.imageEffectsCamera = (Camera)EditorGUILayout.ObjectField ("Image Effects Camera: ", waterZone.imageEffectsCamera, typeof(Camera), true);
			waterZone.waterPlane = (Transform)EditorGUILayout.ObjectField ("Water Plane: ", waterZone.waterPlane, typeof(Transform), true);
			if (waterZone.useWaterSoundEffects) {
				waterZone.waterEnterSound = (AudioClip)EditorGUILayout.ObjectField ("Water Enter Sound Effect: ", waterZone.waterEnterSound, typeof(AudioClip), true);
				waterZone.waterExitSound = (AudioClip)EditorGUILayout.ObjectField ("Water Exit Sound Effect: ", waterZone.waterExitSound, typeof(AudioClip), true);
				waterZone.playerUnderWaterSoundEffect = (AudioClip)EditorGUILayout.ObjectField ("Player UnderWater Sound Effect: ", waterZone.playerUnderWaterSoundEffect, typeof(AudioClip), true);

				var serializedObj = new SerializedObject (target);
				var property = serializedObj.FindProperty ("playerWaterMovementSoundOnWaterSurface");
				serializedObj.Update ();
				EditorGUILayout.PropertyField (property, true);
				serializedObj.ApplyModifiedProperties ();

			}
			if (waterZone.useWaterSplashesAndRipples) {
				waterZone.waterEnterSplash = (GameObject)EditorGUILayout.ObjectField ("Water Enter Splash: ", waterZone.waterEnterSplash, typeof(GameObject), true);
				waterZone.waterMovementSplash = (GameObject)EditorGUILayout.ObjectField ("Water Movement Splash: ", waterZone.waterMovementSplash, typeof(GameObject), true);
				waterZone.waterStaticRipple = (GameObject)EditorGUILayout.ObjectField ("Water Static Ripple: ", waterZone.waterStaticRipple, typeof(GameObject), true);
			}
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
			EditorUtility.SetDirty (waterZone);
	}
}
#endif