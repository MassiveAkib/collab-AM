using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_PlayerGrabber))]
[CanEditMultipleObjects]
public class AFPC_PlayerGrabberEditor : Editor {

	public override void OnInspectorGUI()
	{
		AFPC_PlayerGrabber afpcPlayer = (AFPC_PlayerGrabber)target;
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.BeginVertical ("NotificationBackground");
		afpcPlayer.grabRange = EditorGUILayout.FloatField ("Grabbing Range: ", afpcPlayer.grabRange);
		afpcPlayer.fpsCam = (Camera)EditorGUILayout.ObjectField("Player Camera: ", afpcPlayer.fpsCam, typeof(Camera), true);
		afpcPlayer.handImage = (GameObject)EditorGUILayout.ObjectField ("Hand Image Gameobject: ", afpcPlayer.handImage, typeof(GameObject), true);
		afpcPlayer.grabKey = (KeyCode)EditorGUILayout.EnumPopup ("Grabbing Key: ", afpcPlayer.grabKey, "ExposablePopupMenu");
		afpcPlayer.throwKey = (KeyCode)EditorGUILayout.EnumPopup ("Throwing Key: ", afpcPlayer.throwKey, "ExposablePopupMenu");
		afpcPlayer.dropGrabbedObjectsOnRunning = EditorGUILayout.Toggle ("Drop Grabbed Gameobject On Running:  ", afpcPlayer.dropGrabbedObjectsOnRunning);
		afpcPlayer.hasWeaponHolder = EditorGUILayout.Toggle ("Has Weapon Holder: ", afpcPlayer.hasWeaponHolder);
		#if UNITY_ANDROID || UNITY_IOS
		afpcPlayer.grabVButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Grab Virtual Button: ", afpcPlayer.grabVButton, typeof(AFPC_VirtualButton), true);
		afpcPlayer.dropVButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Drop Virtual Button: ", afpcPlayer.dropVButton, typeof(AFPC_VirtualButton), true);
		afpcPlayer.throwVButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Throw Virtual Button: ", afpcPlayer.throwVButton, typeof(AFPC_VirtualButton), true);
		#endif
		if(afpcPlayer.hasWeaponHolder)
			afpcPlayer.weaponHolder = (GameObject)EditorGUILayout.ObjectField ("Weapon Holder Gameobject: ", afpcPlayer.weaponHolder, typeof(GameObject), true);
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
