using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_DoorManager : MonoBehaviour {

	public KeyCode doorOpenCloseKey = KeyCode.E, keyPickupKey = KeyCode.E;
	public float doorReachDistance = 0.1f;
	public GameObject handImage;
	public AFPC_VirtualButton doorVButton, keyPickupVButton;

	private AFPC_PlayerMovement afpcPlayer;

	private AFPC_Door _door;
	private AFPC_DoorKey _doorKey;

	private bool doorPressed, doorKeyPressed;	// Door Pressed is true when the key required to open door without need of Door Key 
												// Door Key Pressed is true when the key required to pickup the Door Key is Pressed

	private bool CheckForDoorOrKey()
	{
		bool value = false;
		Ray ray = afpcPlayer.fpsCamera.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));
		RaycastHit hit;
		if (Physics.Raycast (ray.origin, afpcPlayer.fpsCamera.transform.forward, out hit, doorReachDistance,Physics.AllLayers)) {
			if (hit.collider.gameObject.GetComponent<AFPC_Door> ()) 
			{
				_door = hit.collider.gameObject.GetComponent<AFPC_Door> ();
				value = true;
				if (handImage != null)
					handImage.SetActive (true);
				if (doorVButton != null)
					doorVButton.gameObject.SetActive (true);
			} else if (hit.collider.gameObject.GetComponent<AFPC_DoorKey> ())
			{
				value = true;
				_doorKey = hit.collider.gameObject.GetComponent<AFPC_DoorKey> ();
				_door = _doorKey.door.GetComponent<AFPC_Door> ();
				if(handImage != null)
					handImage.SetActive (true);
				if(doorVButton != null)
					doorVButton.gameObject.SetActive(false);
				if (keyPickupVButton != null)
					keyPickupVButton.gameObject.SetActive (true);
			}else
			{
				value = false;
				_door = null;
				_doorKey = null;
				if(handImage != null)
					handImage.SetActive (false);
				if(doorVButton != null)
					doorVButton.gameObject.SetActive(false);
				if (keyPickupVButton != null)
					keyPickupVButton.gameObject.SetActive (false);
			}
		} else {
			value = false;
			_door = null;
			_doorKey = null;
			if(handImage != null)
				handImage.SetActive (false);
			if(doorVButton != null)
				doorVButton.gameObject.SetActive(false);
			if (keyPickupVButton != null)
				keyPickupVButton.gameObject.SetActive (false);
		}
		return value;		
	}

	// Use this for initialization
	void Start () {
		if (handImage != null)
			handImage.SetActive (false);

		if (doorVButton != null)
			doorVButton.gameObject.SetActive (false);
		if (keyPickupVButton != null)
			keyPickupVButton.gameObject.SetActive (false);
		afpcPlayer = GetComponent<AFPC_PlayerMovement> ();
	}

	private void GetInput()
	{

		#if UNITY_STANDALONE || UNITY_WEBGL
		if (Input.GetKeyDown (doorOpenCloseKey) && !doorPressed)
			doorPressed = true;
		if (Input.GetKeyDown (keyPickupKey) && !doorKeyPressed)
			doorKeyPressed = true;
		#endif


		#if UNITY_ANDROID || UNITY_IOS
		if (doorVButton != null)
		{
			doorVButton.buttonType = AFPC_VirtualButton.ButtonType.LateTriggerButton;
			doorPressed = doorVButton.value;
		}
		if (keyPickupVButton != null) 
		{
			keyPickupVButton.buttonType = AFPC_VirtualButton.ButtonType.LateTriggerButton;
			doorKeyPressed = keyPickupVButton.value;
		}
		#endif
			
	}

	// Update is called once per frame
	void Update () 
	{
		if (CheckForDoorOrKey ()) {

			GetInput ();

			if (_door != null) {
				_door.previouslyOpen = _door.doorOpen;
				if (doorPressed && !_door.requireKey) {
					if (!_door.doorOpen)
						_door.doorOpen = true;
					else
						_door.doorOpen = false;

					doorPressed = false;
				}
				else if (_door.hasDoorKey && _door.requireKey && doorPressed) {
					if (!_door.doorOpen)
						_door.doorOpen = true;
					else
						_door.doorOpen = false;

					doorPressed = false;
				}
			}

			if (doorKeyPressed && _doorKey == null)
				doorKeyPressed = false;
			
			if (doorKeyPressed && _doorKey != null) {
				
				_door.hasDoorKey = true;
				if (_door.hasDoorKey)
					_door.PickupKey (_doorKey.gameObject);
				doorPressed = false;
				doorKeyPressed = false;
			}
		} 
	}
}
