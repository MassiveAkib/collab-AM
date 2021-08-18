using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class AFPC_LadderBuilder : EditorWindow {

	private GameObject Ladder;
	private Vector3 spawnPos;
	private Vector3 ladderSize;

	//Shortcut - CRTL + Shift + L
	[MenuItem("AFPC/Create/Ladder %#l")]
	public static void ShowWindow()
	{
		GetWindow<AFPC_LadderBuilder> ("Build Ladder");
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical ("ShurikenEffectBg");
		Ladder = (GameObject)EditorGUILayout.ObjectField ("Ladder To Spawn: ", Ladder, typeof(GameObject), false);
		spawnPos = EditorGUILayout.Vector3Field ("Ladder Spawn Position: ", spawnPos);
		ladderSize = EditorGUILayout.Vector3Field ("Ladder Size: ", ladderSize);
		GUILayout.Box (GUIContent.none, "horizontalSlider");
		if (GUILayout.Button ("Build Ladder!")) 
		{
			BuildLadder ();
		}

		EditorGUILayout.EndVertical ();
	}
		

	private void BuildLadder()
	{
		if (Ladder != null) {
			GameObject ladder = Instantiate (Ladder, spawnPos, Ladder.transform.rotation) as GameObject;
			string ladderNewName = ladder.name;
			ladderNewName =	ladderNewName.Remove (ladderNewName.Length - 7, 7);
			ladder.name = ladderNewName;
			GameObject climbEnter = new GameObject ();
			climbEnter.name = "ClimbEnterArea";
			climbEnter.transform.parent = ladder.transform;
			climbEnter.transform.localPosition = Vector3.zero;
			climbEnter.AddComponent<BoxCollider> ();
			BoxCollider boxColl = climbEnter.GetComponent<BoxCollider> ();
			boxColl.size = ladderSize;
			boxColl.isTrigger = true;
			climbEnter.AddComponent<AFPC_Player_ClimbEnter> ();
		} else {
			Debug.LogError ("No Ladder Set to spawn!");
		}
	}
}
#endif