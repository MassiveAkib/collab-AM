using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFPC_Door : MonoBehaviour {

	public Animator doorAnimator;	// Animator attached to door gameobject
	public AudioClip doorOpenSound, doorCloseSound, keyPickupSound;
	public float audioVolume = 0.73f;
	public bool doorOpenedInitialy = false; // If True then door is opened on the start of game else door is closed!
	public bool requireKey = true; // If True then, door will only be opened when player has Key
	public string animatorOpenDoorParamName = "OpenDoor";

	[HideInInspector]
	public bool doorOpen = false; // When True door is open, when false door is closed!
	[HideInInspector]
	public bool hasDoorKey = false; // if true then player has door key else not
	[HideInInspector]
	public bool previouslyOpen;
	private AudioSource _audioSrc;

	private int openDoorBoolHashID;


	private void OpenDoor()
	{
		doorOpen = true;
		if (doorOpenSound != null)
			_audioSrc.PlayOneShot (doorOpenSound, audioVolume);
		doorAnimator.SetBool (openDoorBoolHashID, true);
	}

	private void CloseDoor()
	{
		doorOpen = false;
		if (doorCloseSound != null)
			_audioSrc.PlayOneShot (doorCloseSound, audioVolume);
		doorAnimator.SetBool (openDoorBoolHashID, false);
	}

	public void PickupKey(GameObject key1)
	{
		if (keyPickupSound != null) {
			_audioSrc.PlayOneShot (keyPickupSound, audioVolume);
			Destroy (key1, keyPickupSound.length);
		} else {
			Destroy (key1);
		}
	}

	private void DoorManagement()
	{
		if (!requireKey) {
			if (doorOpen)
				OpenDoor ();
			else
				CloseDoor ();
		} else {
			if (hasDoorKey) {
				if (doorOpen)
					OpenDoor ();
				else
					CloseDoor ();
			} else {
				CloseDoor (); // If We Dont have door key, then close the door!
			}
		}
	}

	private void Update()
	{
		if(previouslyOpen != doorOpen)
			DoorManagement ();	
	}

	// Use this for initialization
	void Start () 
	{
		if(!GetComponent<AudioSource> ())
			gameObject.AddComponent<AudioSource> ();
		_audioSrc = GetComponent<AudioSource> ();

		doorOpen = doorOpenedInitialy;

		openDoorBoolHashID = Animator.StringToHash (animatorOpenDoorParamName);
	}
}
