using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class AFPC_SurfaceBuilder :  EditorWindow {

	private string surfaceName;

	//Shortcut - CRTL + Shift + S
	[MenuItem("AFPC/Create/New Surface %#s")]
	public static void ShowWindow()
	{
		GetWindow<AFPC_SurfaceBuilder> ("Build New Surface");
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical ("ShurikenEffectBg");

		surfaceName = EditorGUILayout.TextField ("Surface Name: ", surfaceName);

		GUILayout.Box (GUIContent.none, "horizontalSlider");
		if (GUILayout.Button ("Create Surface!"))
			CreateNewSurface ();

		EditorGUILayout.EndVertical ();
	}
		
	void CreateNewSurface()
	{
		if (surfaceName != null) {
			AFPC_Surface surface = (AFPC_Surface)ScriptableObject.CreateInstance<AFPC_Surface>();
			surface.name = surfaceName;
			string path = "Assets/AFPC/Content/Surfaces/" + surfaceName + ".asset";
			AssetDatabase.CreateAsset (surface, path);
		} else {
			Debug.LogError ("No Surface Name Set In Surface Creator Window! Please Set A name to create a new surface! ");
		}
	}
}
#endif