using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_StaminaPickup : AFPC_ItemPickup {

	public AFPC_StaminaManager staminaManager;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		if (staminaManager == null)
			staminaManager = afpcPlayer.GetComponent<AFPC_StaminaManager> ();
	}

	protected override void Pickup()
	{
		staminaManager.IncreaseStaminaByAmount (increaseAmount);
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
