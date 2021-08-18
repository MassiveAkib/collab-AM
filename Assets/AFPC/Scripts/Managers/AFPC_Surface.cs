//This Script is used to create a new Surface
using UnityEngine;

public class AFPC_Surface : ScriptableObject {

	public new string name; // Name Of The Surface
	public AudioClip landingSound; // The Landing Sound on the surface
	public AudioClip[] surfaceFootStepSounds; //Footsteps Sound on the surface
	public Texture[] texture; //The Texture of the Surface
}
