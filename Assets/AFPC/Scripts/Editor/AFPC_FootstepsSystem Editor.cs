using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_FootstepsSystem))]
[CanEditMultipleObjects]
public class AFPC_FootstepsSystemEditor : Editor {

	public override void OnInspectorGUI()
	{
		AFPC_FootstepsSystem foostepsSystem = (AFPC_FootstepsSystem)target;
		EditorGUILayout.BeginVertical ("NotificationBackground");
		base.DrawDefaultInspector ();
		EditorGUILayout.EndVertical ();

		if (GUI.changed)
			EditorUtility.SetDirty (foostepsSystem);
	}
}
#endif