using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class AFPC_WaterZoneBuilder : EditorWindow {


	private GameObject waterPlane;
	private Vector3 spawnPos;
	private Vector3 waterSize;
	private bool automaticallyCalculateWaterSize = true;

	//Shortcut - CRTL + Shift + W
	[MenuItem("AFPC/Create/WaterZone %#w")]
	public static void ShowWindow()
	{
		GetWindow<AFPC_WaterZoneBuilder> ("Build Water Zone");
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical ("ShurikenEffectBg");

		waterPlane = (GameObject)EditorGUILayout.ObjectField ("WaterPlane To Spawn: ", waterPlane, typeof(GameObject), false);
		spawnPos = EditorGUILayout.Vector3Field ("WaterPlane Spawn Position: ", spawnPos);
		automaticallyCalculateWaterSize = EditorGUILayout.Toggle ("Auto Calculate Size:", automaticallyCalculateWaterSize);
		if(!automaticallyCalculateWaterSize)
			waterSize = EditorGUILayout.Vector3Field ("Water Zone Size: ", waterSize);
		GUILayout.Box (GUIContent.none, "horizontalSlider");
		if (GUILayout.Button ("Build Water Zone!")) 
		{
			BuildWater ();
		}

		EditorGUILayout.EndVertical ();
	}

	public void BuildWater()
	{
		if (waterPlane != null) {
			GameObject waterplane = Instantiate (waterPlane, spawnPos, waterPlane.transform.rotation) as GameObject;
			string waterPlaneNewName = waterplane.name;
			waterPlaneNewName = waterPlaneNewName.Remove (waterPlaneNewName.Length - 7, 7);
			waterplane.name = waterPlaneNewName;
			GameObject waterZone = new GameObject ();
			waterZone.name = "WaterZone";
			waterZone.AddComponent<AFPC_WaterZone> ();
			AFPC_WaterZone waterzone = waterZone.GetComponent<AFPC_WaterZone> ();
			waterzone.waterPlane = waterplane.transform;
			waterZone.AddComponent<BoxCollider> ();
			BoxCollider boxColl = waterZone.GetComponent<BoxCollider> ();
			boxColl.isTrigger = true;
			if (automaticallyCalculateWaterSize) {
				waterSize.z = waterplane.transform.localScale.y;
				waterSize.x = waterplane.transform.localScale.x;
				waterSize.y = waterplane.transform.position.y;
				boxColl.size = waterSize;
				waterZone.transform.position = waterplane.transform.position - new Vector3 (0f, boxColl.size.y / 2f, 0f);
			} else {
				boxColl.size = waterSize;
				waterZone.transform.position = waterplane.transform.position - new Vector3 (0f, boxColl.size.y / 2f, 0f);
			}
			waterZone.transform.parent = waterplane.transform;
		} else {
			Debug.LogError ("No Water Plane Set To Spawn!");
		}
	}
}
#endif