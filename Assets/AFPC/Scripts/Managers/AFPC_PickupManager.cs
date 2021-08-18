using UnityEngine;

public class AFPC_PickupManager : MonoBehaviour {

	#if UNITY_STANDALONE || UNITY_WEBGL
	public KeyCode pickupKey = KeyCode.F;
	#endif

	public GameObject handImage;
	public float pickupDistance = 5f;

	private AFPC_PlayerMovement afpcPlayer;
	public AFPC_VirtualButton pickupVButton;

	private bool pickupPressed; // True if Pickup key or Pickup virtual button is pressed  
	private AFPC_ItemPickup pickup;

	// Use this for initialization
	void Start () 
	{
		if (handImage != null)
			handImage.SetActive (false);	
		afpcPlayer = GetComponent<AFPC_PlayerMovement> ();
		#if UNITY_ANDROID || UNITY_IOS
		if (pickupVButton != null)
			pickupVButton.buttonType = AFPC_VirtualButton.ButtonType.TriggerButton;
		#endif
	}
	private void GetInput()
	{
		#if UNITY_STANDALONE || UNITY_WEBGL
		pickupPressed = Input.GetKeyDown(pickupKey);
		#endif

		#if UNITY_ANDROID || UNITY_IOS
		if(pickupVButton != null)
		pickupPressed = pickupVButton.value;
		#endif
	}

	// Update is called once per frame
	void Update () {
		GetInput ();

		if (CheckForPickup()) 
		{
			if (pickupPressed)
			{
				if (pickup != null)
					pickup.pickedUp = true;
			}
		}
	}

	private bool CheckForPickup()
	{
		bool value = false;
		Ray ray = afpcPlayer.fpsCamera.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));
		RaycastHit hit;
		if (Physics.Raycast (ray.origin, afpcPlayer.fpsCamera.transform.forward, out hit, pickupDistance,Physics.AllLayers ,QueryTriggerInteraction.Ignore)) {
			if (hit.collider.gameObject.GetComponent<AFPC_ItemPickup> ()) {
				pickup = hit.collider.gameObject.GetComponent<AFPC_ItemPickup> ();
				value = true;
				if(handImage != null)
					handImage.SetActive (true);
				if(pickupVButton != null)
					pickupVButton.gameObject.SetActive(true);
			} else {
				value = false;
				if(handImage != null)
					handImage.SetActive (false);
				if(pickupVButton != null)
					pickupVButton.gameObject.SetActive(false);
			}
		} else {
			value = false;
			if(handImage != null)
				handImage.SetActive (false);
			if(pickupVButton != null)
				pickupVButton.gameObject.SetActive(false);
		}
		return value;
	}
}
