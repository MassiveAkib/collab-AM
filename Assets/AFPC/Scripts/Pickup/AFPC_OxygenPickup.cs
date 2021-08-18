using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_OxygenPickup : AFPC_ItemPickup {

	public AFPC_OxygenManager oxygenManager;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		if(oxygenManager == null)
			oxygenManager = afpcPlayer.GetComponent<AFPC_OxygenManager>();
	}

	protected override void Pickup()
	{
		oxygenManager.IncreaseOxygenByAmount (increaseAmount);
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
