using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_PickupManager))]
[CanEditMultipleObjects]
public class AFPC_pickupManager_Editor : Editor {

	public override void OnInspectorGUI()
	{

		AFPC_PickupManager afpcPlayer = (AFPC_PickupManager)target;
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ("NotificationBackground");
		afpcPlayer.pickupDistance = EditorGUILayout.FloatField ("Pickup Range: ", afpcPlayer.pickupDistance);
		afpcPlayer.handImage = (GameObject)EditorGUILayout.ObjectField ("Hand Image Gameobject: ", afpcPlayer.handImage, typeof(GameObject), true);

		#if UNITY_STANDALONE || UNITY_WEBGL
		afpcPlayer.pickupKey = (KeyCode)EditorGUILayout.EnumPopup ("Pickup Key: ", afpcPlayer.pickupKey, "ExposablePopupMenu");
		#endif
		
		#if UNITY_ANDROID || UNITY_IOS
		afpcPlayer.pickupVButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Pickup Virtual Button: ", afpcPlayer.pickupVButton, typeof(AFPC_VirtualButton), true);
		#endif
		
		EditorGUILayout.EndVertical ();
		if (EditorGUI.EndChangeCheck ()) 
		{
			Undo.RegisterCompleteObjectUndo (target, "Changed Settings");
		}
		if (GUI.changed)
			EditorUtility.SetDirty (afpcPlayer);
	}
}
#endif
