using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_ItemPickup : MonoBehaviour {


	public bool useOnce = true; // If true then gameobject is destroyed after usage!
	public int increaseAmount = 10;	// The Amount to increase
	protected AFPC_PlayerMovement afpcPlayer;

	[HideInInspector]
	public bool pickedUp = false; // True if object is picked
	private GameObject pickupGameObject;

	protected virtual void Pickup()
	{
		if (useOnce) {
			pickupGameObject.SetActive (false);
		}
		pickedUp = false;
	}

	// Use this for initialization
	protected virtual void Start () {
		gameObject.SetActive (true);
		pickedUp = false;
		pickupGameObject = gameObject;	
		if (afpcPlayer == null)
			afpcPlayer = GameObject.FindObjectOfType<AFPC_PlayerMovement> ();
		if (increaseAmount < 0)
			increaseAmount *= -1;
	}

}
