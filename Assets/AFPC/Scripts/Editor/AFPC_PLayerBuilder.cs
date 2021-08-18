using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class AFPC_PLayerBuilder : EditorWindow {

	private AFPC_PlayerMovement.PlayerType typeOfPlayer;
	private AFPC_PlayerMovement.FootstepsDetectionMode footstepDetection;
	private enum PlayerPlatform
	{
		Standalone, Mobile
	} 
	private PlayerPlatform playerPlatform;
	private Vector3 spawnPos = Vector3.zero;
	private GameObject player, HUD, EventSystem;
	private float groundDistance = 0f;
	//Shortcut - CRTL + Shift + K
	[MenuItem("AFPC/Create/Player %#k")]
	public static void ShowWindow()
	{
		GetWindow<AFPC_PLayerBuilder> ("Build Player");
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical ("ShurikenEffectBg");
		typeOfPlayer = (AFPC_PlayerMovement.PlayerType)EditorGUILayout.EnumPopup ("Type Of Player: ", typeOfPlayer, "ExposablePopupMenu");
		playerPlatform = (PlayerPlatform)EditorGUILayout.EnumPopup ("Platform: ", playerPlatform, "ExposablePopupMenu");
		spawnPos = EditorGUILayout.Vector3Field ("Player Spawn Position: ", spawnPos);
		if(typeOfPlayer == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer)
			footstepDetection = (AFPC_PlayerMovement.FootstepsDetectionMode)EditorGUILayout.EnumPopup ("Footstep Detection Mode: ", footstepDetection, "ExposablePopupMenu");
		GUILayout.Box (GUIContent.none, "horizontalSlider");
		if (GUILayout.Button ("Build Player!")) 
		{
			BuildPlayer ();
		}
		if (player != null)
		{
			groundDistance = EditorGUILayout.FloatField ("Ground Distance: ", groundDistance);
			if (GUILayout.Button ("Place On Ground")) 
			{
				if(groundDistance > 0)
					PutOnGround (groundDistance);
				else
					Debug.LogError("Set Ground Distance To Greater Than 0.");
			}
		}

		EditorGUILayout.EndVertical ();
	}

	private void PutOnGround(float dist)
	{
		AFPC_PlayerMovement afpcPlayer = player.GetComponent<AFPC_PlayerMovement> ();
		Vector3 origin = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z);
		RaycastHit hit = new RaycastHit ();
		if (Physics.SphereCast (origin, afpcPlayer.playerCapsule.radius, Vector3.down, out hit, dist, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
			player.transform.position = hit.point + new Vector3 (0f, afpcPlayer.playerCapsule.height * player.transform.localScale.y / 2f, 0f);
		} else {
			Debug.Log ("Distance Not Enough To Touch The Ground!");
		}
	}

	private void BuildPlayer()
	{
		GameObject temp = (GameObject)(AssetDatabase.LoadAssetAtPath ("Assets/AFPC/Content/Prefabs/AFPC_Player.prefab", typeof(GameObject)));
		player = Instantiate (temp, spawnPos, Quaternion.identity) as GameObject;
		player.transform.SetAsLastSibling ();
		player.name = player.name.Remove (player.name.Length - 7, 7);
		player.GetComponent<AFPC_PlayerMovement> ().playerType = typeOfPlayer;
		if (typeOfPlayer == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) 
		{
			player.GetComponent<AFPC_PlayerMovement> ().footstepsDetectionMode = footstepDetection;
			if (playerPlatform == PlayerPlatform.Standalone)
			{
				GameObject temp1 = (GameObject)(AssetDatabase.LoadAssetAtPath ("Assets/AFPC/Content/Prefabs/HUD_STANDALONE.prefab", typeof(GameObject)));
				HUD = Instantiate (temp1, Vector3.zero, Quaternion.identity) as GameObject;
				HUD.transform.SetAsLastSibling ();
				HUD.name = HUD.name.Remove (HUD.name.Length - 7, 7);
				GameObject temp2 = (GameObject)(AssetDatabase.LoadAssetAtPath ("Assets/AFPC/Content/Prefabs/EventSystem_STANDALONE.prefab", typeof(GameObject)));
				EventSystem = Instantiate (temp2, Vector3.zero, Quaternion.identity) as GameObject;
				EventSystem.transform.SetAsLastSibling ();
				EventSystem.name = EventSystem.name.Remove (EventSystem.name.Length - 7, 7);
			} else if (playerPlatform == PlayerPlatform.Mobile) 
			{
				GameObject temp1 = (GameObject)(AssetDatabase.LoadAssetAtPath ("Assets/AFPC/Content/Prefabs/HUD_MOBILE.prefab", typeof(GameObject)));
				HUD = Instantiate (temp1, Vector3.zero, Quaternion.identity) as GameObject;
				HUD.transform.SetAsLastSibling ();
				HUD.name = HUD.name.Remove (HUD.name.Length - 7, 7);
				GameObject temp2 = (GameObject)(AssetDatabase.LoadAssetAtPath ("Assets/AFPC/Content/Prefabs/EventSystem_MOBILE.prefab", typeof(GameObject)));
				EventSystem = Instantiate (temp2, Vector3.zero, Quaternion.identity) as GameObject;
				EventSystem.transform.SetAsLastSibling ();
				EventSystem.name = EventSystem.name.Remove (EventSystem.name.Length - 7, 7);
			}
		}
	}
}
#endif