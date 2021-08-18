using UnityEngine;

[System.Serializable]
public class Surface
{
	public string surfaceName;
	public AudioClip[] surfaceFootstepSounds;
	public AudioClip landingSound;
}

[RequireComponent(typeof(AFPC_PlayerMovement))]
[RequireComponent(typeof(AudioSource))]
public class AFPC_FootstepsSystem : MonoBehaviour {

	public Surface[] surfaces;
	public bool useDefaultFootstepSounds = true;	// If enabled, then plays default footstepsound for undefined surface
	public AudioClip[] defaultFootstepSounds;	
	public AudioClip defaultLandingSound;
	[Range(0f, 1f)]
	public float footstepVolume = 0.87f;
	private AudioClip[]	_temp, _temp1; // temporary variables used to store surfaceFootstepSounds
	private int _n, _n1; // used to store the index of current surfacecFootstepSounds
	private AudioSource _audioSrc;
	private AFPC_PlayerMovement _afpcPlayer;
	private Surface _currentSurface; // Used to store the current surface
	// Use this for initialization
	void Start () 
	{
		_afpcPlayer = GetComponent<AFPC_PlayerMovement> ();
		_audioSrc = GetComponent<AudioSource> ();
		_audioSrc.playOnAwake = false;
	}

	public void SetLandingSound()
	{
		if ((!_afpcPlayer.isGrounded || _afpcPlayer.playerState == AFPC_PlayerMovement.PlayerStates.Swim_2) && !_afpcPlayer.CanClimb && _afpcPlayer.fpsCamera.GetComponent<AFPC_Cam>().CanBob)
			return;	 // if the player is not grounded or swimming or climbing, then do nothing
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(transform.position, Vector3.down,out hit))
		{
			switch (hit.collider.gameObject.tag) 
			{
			case "Grass":
				if (surfaces [0].landingSound != null) {
					_afpcPlayer.LandingSound = surfaces [0].landingSound;
				} else {
					Debug.LogError ("No Landing Sound Defined For " + surfaces [0].surfaceName + ", Can't Play Sound!");
				}
					break;
			case "Gravel":
				if (surfaces [1].landingSound != null) {
					_afpcPlayer.LandingSound = surfaces [1].landingSound;
				} else {
					Debug.LogError ("No Landing Sound Defined For" + surfaces [1].surfaceName + ", Can't Play Sound!");
				}
					break;
				/*	Template for creating new Footsteps surface based on Tags(Just Copy and replace the values and new surface is ready!) - 
				 * 	case "surfaceName":
				if (surfaces [indexOfSurface].landingSound != null) {
					_afpcPlayer.LandingSound = surfaces [indexOfSurface].landingSound;
				} else {
					Debug.LogError ("No Landing Sound Defined For" + surfaces [indexOfSurface].surfaceName + ", Can't Play Sound!");
				}
					break;
				 * 
				*/
			default:
				if (defaultLandingSound != null) {
					_afpcPlayer.LandingSound = defaultLandingSound;
				} else {
					Debug.LogError ("No Default Landing Sound Defined For Undefined Surface, Can't Play Sound!");
				}
					break;
			}
		}
	}

	public void PlayFootStepSound()
	{
		if ((!_afpcPlayer.isGrounded || _afpcPlayer.playerState == AFPC_PlayerMovement.PlayerStates.Swim_2) && !_afpcPlayer.CanClimb && _afpcPlayer.fpsCamera.GetComponent<AFPC_Cam>().CanBob)
			return;	 // if the player is not grounded or swimming or climbing, then do nothing

		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(transform.position, Vector3.down,out hit))
		{
			switch (hit.collider.gameObject.tag) 
			{
			case "Grass":
				if (surfaces [0].surfaceFootstepSounds != null) {
					_temp = surfaces [0].surfaceFootstepSounds;
					_n = Random.Range (0, _temp.Length - 1);
					_audioSrc.clip = _temp [_n];
				} else {
					Debug.LogError("Cannot Play Sound, no footsteps sounds defined for " + surfaces[0].surfaceName);
				}
			    break;
			case "Gravel":
				if (surfaces [1].surfaceFootstepSounds != null) {
					_temp = surfaces [1].surfaceFootstepSounds;
					_n = Random.Range (0, _temp.Length - 1);
					_audioSrc.clip = _temp [_n];
				} else {
					Debug.LogError("Cannot Play Sound, no footsteps sounds defined for " + surfaces[1].surfaceName);
				}
                break;
				/*	Template for creating new Footsteps surface based on Tags(Just Copy and replace the values and new surface is ready!) - 
				case "surfaceName":
				if (surfaces [indexOfSurface].surfaceFootstepSounds != null) {
					_temp = surfaces [indexOfSurface].surfaceFootstepSounds;
					_n = Random.Range (0, _temp.Length - 1);
					_audioSrc.clip = _temp [_n];
				}else {
					Debug.LogError ("Cannot Play Sound, no footsteps sounds defined for " + surfaces [indexOfSurface].surfaceName);
				}
					break;
				*/
			default:
				if (useDefaultFootstepSounds && defaultFootstepSounds != null) {
					// If the tag doesnot match any, then play the normal footstep sound or universal footstep sound
					// pick & play a random footstep sound from the array,
					_temp1 = defaultFootstepSounds;
					_n1 = Random.Range (0, _temp1.Length - 1);
					_audioSrc.clip = _temp1 [_n1];
				} else {
					Debug.LogError("Cannot Play Sound, no default footsteps sounds defined for undefined surface");
				}
				break;
			}
		}
		_audioSrc.PlayOneShot (_audioSrc.clip, footstepVolume);
	}
}