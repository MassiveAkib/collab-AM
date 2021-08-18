using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AFPC_FootstepsManager))]
[CanEditMultipleObjects]
public class AFPC_FootstepsManagerEditor : Editor {

	public override void OnInspectorGUI()
	{
		AFPC_FootstepsManager foostepsManager = (AFPC_FootstepsManager)target;
		EditorGUILayout.BeginVertical ("NotificationBackground");
		base.DrawDefaultInspector();
		EditorGUILayout.HelpBox ("Make Sure the order of SurfacesOnTerrain is same as the order in Terrain Painting Panel\nTo See the textures applied to the terrain, Go to the paintbrush icon in your terrain\nThe Left-Most texture will be the texture with index 0.\nThe next to it will have an index of 1.\n", MessageType.Info, true);
		EditorGUILayout.EndVertical ();

		if (GUI.changed)
			EditorUtility.SetDirty (foostepsManager);
	}
}
