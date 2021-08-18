using UnityEditor;
using UnityEngine;

public class AFPC_PickupItemBuilder : EditorWindow {

	private GameObject pickupGameObject;
	private Vector3 spawnPos;
	private enum PickupType
	{
		Health, Stamina, Oxygen
	}
	private PickupType pickupType;

	//Shortcut - CRTL + Shift + M
	[MenuItem("AFPC/Create/Pickup Item %#m")]
	public static void ShowWindow()
	{
		GetWindow<AFPC_PickupItemBuilder> ("Build Pickup Item");
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical ("ShurikenEffectBg");
		pickupType = (PickupType)EditorGUILayout.EnumPopup ("Pickup Type: ", pickupType, "ExposablePopupMenu");
		spawnPos = EditorGUILayout.Vector3Field ("Pickup Spawn Position: ", spawnPos);
		pickupGameObject = (GameObject)EditorGUILayout.ObjectField ("Pickup Gameobject: ", pickupGameObject, typeof(GameObject), false);
		GUILayout.Box (GUIContent.none, "horizontalSlider");
		if (GUILayout.Button ("Build Pickup Item!")) 
		{
			BuildPickupItem ();
		}
		EditorGUILayout.EndVertical ();
	}

	private void BuildPickupItem()
	{
		GameObject pickup = Instantiate (pickupGameObject, spawnPos, pickupGameObject.transform.rotation) as GameObject;
		pickup.name = pickup.name.Remove (pickup.name.Length - 7, 7);
		pickup.transform.SetAsLastSibling ();
		if (pickupType == PickupType.Health)
			pickup.AddComponent<AFPC_HealthPickup> ();
		else if (pickupType == PickupType.Oxygen)
			pickup.AddComponent<AFPC_OxygenPickup> ();
		else if (pickupType == PickupType.Stamina)
			pickup.AddComponent<AFPC_StaminaPickup> ();
		pickup.AddComponent<BoxCollider> ();
	}
}
