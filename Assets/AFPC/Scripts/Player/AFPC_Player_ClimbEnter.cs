using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AFPC_Player_ClimbEnter : MonoBehaviour {

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.GetComponent<AFPC_PlayerMovement> () ) 
		{
			AFPC_PlayerMovement afpcPlayer = coll.gameObject.GetComponent<AFPC_PlayerMovement> ();
			if(afpcPlayer.playerType != AFPC_PlayerMovement.PlayerType.spectator)
				afpcPlayer.CanClimb = true;
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if (coll.gameObject.GetComponent<AFPC_PlayerMovement> ()) 
		{
			AFPC_PlayerMovement afpcPlayer = coll.gameObject.GetComponent<AFPC_PlayerMovement> ();
			if(afpcPlayer.playerType != AFPC_PlayerMovement.PlayerType.spectator)
				afpcPlayer.CanClimb = true;
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if (coll.gameObject.GetComponent<AFPC_PlayerMovement> ()) 
		{
			AFPC_PlayerMovement afpcPlayer = coll.gameObject.GetComponent<AFPC_PlayerMovement> ();
			if(afpcPlayer.playerType != AFPC_PlayerMovement.PlayerType.spectator)
			{
				afpcPlayer.CanClimb = false;
				coll.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero; // To Make Sure that the player does not fly after climbing
			}
		}
	}
}
