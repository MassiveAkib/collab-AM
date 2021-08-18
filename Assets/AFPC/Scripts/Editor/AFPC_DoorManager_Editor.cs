using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_DoorManager))]
[CanEditMultipleObjects]
public class AFPC_DoorManager_Editor : Editor {

	public override void OnInspectorGUI()
	{

		AFPC_DoorManager doorManager = (AFPC_DoorManager)target;
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ("NotificationBackground");
		doorManager.doorReachDistance = EditorGUILayout.FloatField ("Door Reach Distance: ", doorManager.doorReachDistance);
		doorManager.handImage = (GameObject)EditorGUILayout.ObjectField ("Hand Image Gameobject: ", doorManager.handImage, typeof(GameObject), true);
		doorManager.doorOpenCloseKey = (KeyCode)EditorGUILayout.EnumPopup ("Door Open/Close Key: ", doorManager.doorOpenCloseKey, "ExposablePopupMenu");
		doorManager.keyPickupKey = (KeyCode)EditorGUILayout.EnumPopup ("Door Key Pickup Key: ", doorManager.keyPickupKey, "ExposablePopupMenu");

		#if UNITY_ANDROID || UNITY_IOS
			doorManager.doorVButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("DoorOpen/Close Virtual Button: ", doorManager.doorVButton, typeof(AFPC_VirtualButton), true);
			doorManager.keyPickupVButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("KeyPickup Virtual Button: ", doorManager.keyPickupVButton, typeof(AFPC_VirtualButton), true);
		#endif

		EditorGUILayout.EndVertical ();

		if (EditorGUI.EndChangeCheck ()) 
		{
			Undo.RegisterCompleteObjectUndo (target, "Changed Settings");
		}

		if (GUI.changed)
			EditorUtility.SetDirty (doorManager);
	}
}
#endif
