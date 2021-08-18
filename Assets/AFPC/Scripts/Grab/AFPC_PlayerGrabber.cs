using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AFPC_PlayerGrabber : MonoBehaviour {

	public float grabRange = 5f;
	public GameObject handImage;
	public Camera fpsCam;
	public bool dropGrabbedObjectsOnRunning = true;
	public bool hasWeaponHolder = false;
	public GameObject weaponHolder;	// if in case you have a weapon holder, and while grabbing, you want to disable it then fill this field else not
	public KeyCode grabKey = KeyCode.F, throwKey = KeyCode.Mouse0;
	public AFPC_VirtualButton grabVButton, throwVButton, dropVButton;

	private float spring = 83f;
	private float damper = 1.5f;
	private float drag = 9.0f, startingDrag;
	private float angularDrag = 4.6f, startingAngularDrag;
	private bool rigidbodyStartingGravityBool, rigidbodyStartingIsKinematicBool;
	private AFPC_Grab grabbedObject;
	private AudioSource grabbedObjectAudioSource;
	private GameObject grabbedGameObject;
	private Rigidbody grabbedGameObjectRgbd;
	private SpringJoint grabberGameObjectSpringJoint;
	private GameObject springJointMaster;
	private Vector3 currentV1;
	private bool checkForHand = true;
	private AFPC_PlayerMovement afpcPlayer;
	private int initChilds = 0;

	// Use this for initialization
	void Start () {
		if(handImage != null)
			handImage.SetActive (false);
		if (GetComponent<AFPC_PlayerMovement> ())
			afpcPlayer = GetComponent<AFPC_PlayerMovement> ();
		#if UNITY_ANDROID || UNITY_IOS
		if(dropVButton != null)
		{
			dropVButton.buttonType = AFPC_VirtualButton.ButtonType.TriggerButton;
			dropVButton.gameObject.SetActive(false);
		}
		if(throwVButton != null)
		{	
			throwVButton.buttonType = AFPC_VirtualButton.ButtonType.TriggerButton;
			throwVButton.gameObject.SetActive(false);
		}
		if(grabVButton != null)
		{
			grabVButton.buttonType = AFPC_VirtualButton.ButtonType.TriggerButton;
			grabVButton.gameObject.SetActive(false);
		}

		if(fpsCam != null)
			initChilds = fpsCam.transform.childCount;
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		CheckHand ();

		#if UNITY_STANDALONE || UNITY_WEBGL
		if (Input.GetKeyDown (grabKey)) {
			if(fpsCam.transform.childCount > initChilds)
				DropAllObjects();
			else
				Grab ();
		} else if ((Input.GetKeyUp (grabKey) || (dropGrabbedObjectsOnRunning && afpcPlayer.IsRunning)) && grabbedGameObject != null) {
			Drop ();
		} else if (Input.GetKeyDown (throwKey) && grabbedGameObject != null) {
			Throw ();
		}
		#endif
		#if UNITY_ANDROID || UNITY_IOS
		if(grabVButton != null && dropVButton != null && throwVButton != null)
		{
			if(grabVButton.value)
			{
				if(fpsCam.transform.childCount > initChilds)
					DropAllObjects();
				else
					Grab();
				grabVButton.value = false;
			} else if((dropVButton.value || (dropGrabbedObjectsOnRunning && afpcPlayer.IsRunning)) && grabbedGameObject != null){
				Drop();	
				dropVButton.value = false;
			} else if(throwVButton.value && grabbedGameObject != null){
				Throw();
				throwVButton.value = false;
			}
		}
		#endif

	}

	void CheckHand()
	{
		if (checkForHand) {
			Ray ray = fpsCam.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));
			RaycastHit hit;
			if (Physics.Raycast (ray.origin, fpsCam.transform.forward, out hit, grabRange,Physics.AllLayers ,QueryTriggerInteraction.Ignore)) {
				if (hit.collider.gameObject.GetComponent<AFPC_Grab> () && hit.collider.gameObject.GetComponent<Rigidbody> ()) {
					if(handImage != null)
						handImage.SetActive (true);
					if(grabVButton != null)
						grabVButton.gameObject.SetActive(true);
				} else {
					if(handImage != null)
						handImage.SetActive (false);
					if(grabVButton != null)
						grabVButton.gameObject.SetActive(false);
				}
			} else {
				if(handImage != null)
					handImage.SetActive (false);
				if(grabVButton != null)
					grabVButton.gameObject.SetActive(false);
			}
		}
	}

	void Throw()
	{
		if (grabbedObject.throwSound != null) 
		{
			grabbedObjectAudioSource.clip = grabbedObject.throwSound;
			grabbedObjectAudioSource.Play ();
		}
		grabbedGameObjectRgbd.isKinematic = rigidbodyStartingIsKinematicBool;
		grabbedGameObjectRgbd.useGravity = rigidbodyStartingGravityBool;
		grabbedGameObjectRgbd.drag = startingDrag;
		grabbedGameObjectRgbd.angularDrag = startingAngularDrag;
		grabberGameObjectSpringJoint.connectedBody.AddForceAtPosition (fpsCam.transform.forward * grabbedObject.throwForce, springJointMaster.transform.position, ForceMode.Impulse);
		grabberGameObjectSpringJoint.connectedBody = null;
		grabbedGameObject = null;
		grabbedGameObjectRgbd = null;
		Destroy (springJointMaster);
		if(weaponHolder != null && hasWeaponHolder)
			weaponHolder.SetActive (true);
		checkForHand = true;

		#if UNITY_ANDROID || UNITY_IOS
		if(dropVButton != null)
			dropVButton.gameObject.SetActive(false);
		if(throwVButton != null)
			throwVButton.gameObject.SetActive(false);
		if(grabVButton != null)
			grabVButton.gameObject.SetActive(false);
		#endif
	}

	void Drop()
	{
		if (grabbedGameObject.GetComponent<AFPC_Grab> ())
		{
			grabberGameObjectSpringJoint.connectedBody = null;
			grabbedGameObjectRgbd.isKinematic = rigidbodyStartingIsKinematicBool;
			grabbedGameObjectRgbd.useGravity = rigidbodyStartingGravityBool;
			grabbedGameObjectRgbd.drag = startingDrag;
			grabbedGameObjectRgbd.angularDrag = startingAngularDrag;
			grabbedGameObject = null;
			grabbedGameObjectRgbd = null;
			Destroy (springJointMaster);
			if(weaponHolder != null && hasWeaponHolder)
				weaponHolder.SetActive (true);
			checkForHand = true;

			#if UNITY_ANDROID || UNITY_IOS
			if(dropVButton != null)
				dropVButton.gameObject.SetActive(false);
			if(throwVButton != null)
				throwVButton.gameObject.SetActive(false);
			if(grabVButton != null)
				grabVButton.gameObject.SetActive(false);
			#endif
		}
	}

	void DropAllObjects()
	{
		#if UNITY_ANDROID || UNITY_IOS
		if(dropVButton != null)
			dropVButton.gameObject.SetActive(false);
		if(throwVButton != null)
			throwVButton.gameObject.SetActive(false);
		if(grabVButton != null)
			grabVButton.gameObject.SetActive(false);
		#endif

		foreach(Transform grabObject in fpsCam.transform)
		{
			grabObject.GetComponent<Rigidbody> ().isKinematic = rigidbodyStartingIsKinematicBool;
			grabObject.GetComponent<Rigidbody> ().useGravity = rigidbodyStartingGravityBool;
			if(weaponHolder != null && hasWeaponHolder)
				weaponHolder.SetActive (true);
		}
	}

	void DoGrabbing(RaycastHit hit)
	{
		checkForHand = false;
		#if UNITY_ANDROID || UNITY_IOS
		if(dropVButton != null)
			dropVButton.gameObject.SetActive(true);
		if(throwVButton != null)
			throwVButton.gameObject.SetActive(true);
		if(grabVButton != null)
			grabVButton.gameObject.SetActive(false);
		#endif
		if(handImage != null)
			handImage.SetActive (false);
		if(weaponHolder != null && hasWeaponHolder)
			weaponHolder.SetActive (false);
		springJointMaster = new GameObject ("springJointMaster");
		springJointMaster.transform.position = fpsCam.transform.position + fpsCam.transform.forward;
		springJointMaster.AddComponent<Rigidbody> ();
		rigidbodyStartingGravityBool = hit.collider.gameObject.GetComponent<Rigidbody> ().useGravity;
		rigidbodyStartingIsKinematicBool = hit.collider.gameObject.GetComponent<Rigidbody> ().isKinematic;
		springJointMaster.GetComponent<Rigidbody> ().useGravity = false;
		springJointMaster.GetComponent<Rigidbody> ().isKinematic = true;
		springJointMaster.AddComponent<SpringJoint> ();
		grabberGameObjectSpringJoint = springJointMaster.GetComponent<SpringJoint> ();
		grabberGameObjectSpringJoint.spring = spring;
		grabberGameObjectSpringJoint.damper = damper;
		grabberGameObjectSpringJoint.maxDistance = 0f;
		grabberGameObjectSpringJoint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody> ();
		grabberGameObjectSpringJoint.anchor = Vector3.zero;

		grabbedObject = hit.collider.gameObject.GetComponent<AFPC_Grab> ();

		#region Grabbing Audio
		if (!hit.collider.gameObject.GetComponent<AudioSource> ()) 
		{
			hit.collider.gameObject.AddComponent<AudioSource> ();
			grabbedObjectAudioSource = hit.collider.gameObject.GetComponent<AudioSource> ();
			grabbedObjectAudioSource.volume = grabbedObject.audioVolume;
			if(grabbedObject.grabSound != null)
			{
			grabbedObjectAudioSource.clip = grabbedObject.grabSound;
			grabbedObjectAudioSource.Play();
			}
		}
		grabbedObjectAudioSource = hit.collider.gameObject.GetComponent<AudioSource> ();
		grabbedObjectAudioSource.volume = grabbedObject.audioVolume;
		if(grabbedObject.grabSound != null)
		{
		grabbedObjectAudioSource.clip = grabbedObject.grabSound;
		grabbedObjectAudioSource.Play();
		}
		#endregion

		grabbedGameObject = hit.collider.gameObject;
		grabberGameObjectSpringJoint.autoConfigureConnectedAnchor = false;
		grabberGameObjectSpringJoint.connectedAnchor = Vector3.zero;
		springJointMaster.transform.SetParent (fpsCam.transform, true);
		hit.collider.transform.position = Vector3.SmoothDamp (hit.collider.transform.position, fpsCam.transform.position + fpsCam.transform.forward, ref currentV1, grabbedObject.grabSmoothTime);
		grabbedGameObjectRgbd = hit.collider.gameObject.GetComponent<Rigidbody> ();
		grabbedGameObjectRgbd.isKinematic = false;
		grabbedGameObjectRgbd.useGravity = false;
		startingDrag = grabbedGameObjectRgbd.drag;
		startingAngularDrag = grabbedGameObjectRgbd.angularDrag;
		grabbedGameObjectRgbd.drag = drag;
		grabbedGameObjectRgbd.angularDrag = angularDrag;
	}

	void Grab()
	{
		Ray ray = fpsCam.ViewportPointToRay (new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;
		if(Physics.Raycast(ray.origin, fpsCam.transform.forward, out hit, grabRange,Physics.AllLayers , QueryTriggerInteraction.Ignore))
		{
			if (hit.collider.gameObject.GetComponent<AFPC_Grab> () && hit.collider.gameObject.GetComponent<Rigidbody>()) 
			{
				DoGrabbing (hit);
			}
		}
	}

}