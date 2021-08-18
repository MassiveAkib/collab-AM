using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_SpawnManager))]
[CanEditMultipleObjects]
public class AFPC_SpawnManagerEditor : Editor {


	public override void OnInspectorGUI()
	{
		AFPC_SpawnManager spawnManager = (AFPC_SpawnManager)target;
		EditorGUILayout.BeginVertical ("NotificationBackground");
		spawnManager.dieDropForce = EditorGUILayout.FloatField ("Force To Exert On Head On Dying: ", spawnManager.dieDropForce);
		spawnManager.timeToApplyForce = EditorGUILayout.FloatField ("Time To Apply Die Drop Force on Dying: ", spawnManager.timeToApplyForce);
		spawnManager.fadePanel = (Image)EditorGUILayout.ObjectField("Fade Panel Image: ", spawnManager.fadePanel, typeof(Image), true);
		spawnManager.timeToRespawn = EditorGUILayout.FloatField ("Time in seconds to respawn: ", spawnManager.timeToRespawn);
		spawnManager.timeToFadeIn = EditorGUILayout.FloatField ("Time in seconds to FadeIn FadePanel: ", spawnManager.timeToFadeIn);
		spawnManager.timeToFadeOut = EditorGUILayout.FloatField ("Time in seconds to FadeOut FadePanel: ", spawnManager.timeToFadeOut);
		spawnManager.hasDroppablesOnDie = EditorGUILayout.Toggle ("Drop Gameobjects on Die:  ", spawnManager.hasDroppablesOnDie);
		if (spawnManager.hasDroppablesOnDie) 
		{
			var serializedObj = new SerializedObject (target);
			var property = serializedObj.FindProperty ("Droppables");
			serializedObj.Update ();
			EditorGUILayout.PropertyField (property, true);
			serializedObj.ApplyModifiedProperties ();
		}
		spawnManager.spawnAtDiePoint = EditorGUILayout.Toggle ("Spawn At Die Position: ", spawnManager.spawnAtDiePoint);
		if (!spawnManager.spawnAtDiePoint)
		{
			var serializedObj = new SerializedObject (target);
			var property = serializedObj.FindProperty ("spawnPoints");
			serializedObj.Update ();
			EditorGUILayout.PropertyField (property, true);
			serializedObj.ApplyModifiedProperties ();
		}

		EditorGUILayout.EndVertical ();
		if (GUI.changed)
			EditorUtility.SetDirty (spawnManager);
	}
}
#endif