using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_HealthPickup : AFPC_ItemPickup {

	public AFPC_HealthManager healthManager;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		if (healthManager == null)
			healthManager = afpcPlayer.GetComponent<AFPC_HealthManager> ();
	}

	protected override void Pickup()
	{
		healthManager.IncreaseHealth (increaseAmount);
		pickedUp = false;
		if (useOnce)
			Destroy (gameObject, 1f);
	}

	// Update is called once per frame
	private void Update () {
		if (pickedUp) 
		{
			base.Pickup ();
			Pickup ();
		}
	}
}
