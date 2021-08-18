using UnityEngine;


[RequireComponent(typeof(AFPC_PlayerMovement))]
[RequireComponent(typeof(AudioSource))]
public class AFPC_FootstepsManager : MonoBehaviour {

	public AFPC_Surface[] surfaces; //all the surfaces type 
	public AFPC_Surface[] surfacesOnTerrain; //all the surfaces used on terrain
	public AudioClip[] defaultFootstepSounds;
	public AudioClip defaultLandingSound;
	public bool playDefaultFootstepsSoundForUndefinedSurfaces = true;
	[Range(0f, 1f)]
	public float audioVolume = 0.5f;

	private AFPC_PlayerMovement _controller; // the Fps player movement script
	private AudioSource _audioSource; //The AudioSouce Attached to the character controller gameobject or in this case the AFPC_Player
	private int n, n1, n2, n3;	//Used to store Integer value
	private AFPC_Surface _currentSurface; // the current surface the player is in contact with
	// Use this for initialization
	void Start () 
	{
		_controller = GetComponent<AFPC_PlayerMovement> ();
		_audioSource = GetComponent<AudioSource> ();
	}

	public void SetLandingSound()
	{
		if ((!_controller.isGrounded || _controller.IsSwimming) && !_controller.CanClimb && _controller.fpsCamera.GetComponent<AFPC_Cam>().CanBob)
			return;	 // if the player is not grounded or swimming or Climbing, then do nothing

		_currentSurface = null;

		RaycastHit hit = new RaycastHit(); 
		if (Physics.Raycast (transform.position, Vector3.down, out hit)) {
			if (!hit.collider.gameObject.GetComponent<Terrain> () /* If we are not on terrain */) {
				foreach (AFPC_Surface surface in surfaces /* Looping through all the surfaces */) {
					foreach (Texture text in surface.texture) {
						if (hit.collider.GetComponent<Renderer> ()) 
						{
							if (text == hit.collider.GetComponent<Renderer> ().sharedMaterial/* If The surface texture matches the texture of the collided gameobject */) {
								_currentSurface = surface;
								if (surface.landingSound != null) {
									_controller.LandingSound = surface.landingSound;
								} else {
									Debug.LogError ("No Landing Sound Set For " + surface.name);
								}
							}
						}
					}
				}
				if (playDefaultFootstepsSoundForUndefinedSurfaces && _currentSurface == null) 
				{
					if(defaultLandingSound != null)
						_controller.LandingSound = defaultLandingSound;
					else
						Debug.LogError ("No Default Landing Sound Set!");
				}

			} else {
				SetLandingSoundOnTerrain (hit);
			}
		}
	}

	public void PlayFootStepSound()
	{
		if ((!_controller.isGrounded || _controller.IsSwimming) && !_controller.CanClimb && _controller.fpsCamera.GetComponent<AFPC_Cam>().CanBob)
			return;	 // if the player is not grounded or swimming or Climbing, then do nothing

		_currentSurface = null; // Reseting the current Surface to null
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(transform.position, Vector3.down,out hit))
		{
			if (!hit.collider.gameObject.GetComponent<Terrain> () /* If we are not on terrain */) {
				foreach (AFPC_Surface surface in surfaces /* Looping through all the surfaces */) {
					foreach (Texture text in surface.texture) 
					{
						if (hit.collider.GetComponent<Renderer> ()) 
						{
							if (text == hit.collider.GetComponent<Renderer> ().sharedMaterial/* If The surface texture matches the texture of the collided gameobject */) {
								_currentSurface = surface;	//Setting the current surface to the surface which has the same name as the tag of the gameobject of hit.collider
								if (surface.surfaceFootStepSounds != null) {
									n = Random.Range (0, surface.surfaceFootStepSounds.Length - 1);
									_audioSource.clip = surface.surfaceFootStepSounds [n]; //Setting the current footstep sound to surface.surfaceFootStepSounds sound by using random indexes
								} else {
									Debug.LogError ("No Footstep Sound Set For " + surface.name);
								}
							}
						}
					}
				}
				if (playDefaultFootstepsSoundForUndefinedSurfaces && _currentSurface == null) {
					// If the tag doesnot match any, then play the normal footstep sound or universal footstep sound
					// pick & play a random footstep sound from the array,
					if (defaultFootstepSounds != null) {
						n1 = Random.Range (0, defaultFootstepSounds.Length - 1);
						_audioSource.clip = defaultFootstepSounds [n1];
					} else {
						Debug.LogError ("No Default Landing Sound Set!");
					}

				}
			} else {
				PlayFootstepSoundWhenOnTerrain(hit);
			}
		}
			
		_audioSource.PlayOneShot (_audioSource.clip, audioVolume);
	}
		
	private void SetLandingSoundOnTerrain(RaycastHit hit)
	{
		if (!_controller.isGrounded)
			return;	 // if the player is not grounded, then do nothing

		if(!hit.collider.gameObject.GetComponent<Terrain>())
			return;
		bool existance = false;	//Used to check the existance of the surface
		int i = TerrainSurface.GetMainTexture (transform.position);
		for (int j = 0; j < surfacesOnTerrain.Length; ++j) 
		{
			if (j == i) {
				existance = true;	// if index i of terrain main texture also exists in the terrain surfaces array
				break;
			}
		}
		if (existance) {
			if (surfacesOnTerrain [i].landingSound != null) {
				_controller.LandingSound = surfacesOnTerrain [i].landingSound;
			} else {
				Debug.LogError ("No Landing Sound Set For " + surfacesOnTerrain [i].name);
			}
		} else {
			if (playDefaultFootstepsSoundForUndefinedSurfaces) 
			{
				if (defaultLandingSound != null) {
					_controller.LandingSound = defaultLandingSound;
				} else {
					Debug.LogError ("No Default Landing Sound Set!");
				}
			}
		}
	}

	private void PlayFootstepSoundWhenOnTerrain(RaycastHit hit)
	{
		if (!_controller.isGrounded)
			return;	 // if the player is not grounded, then do nothing

		if(!hit.collider.gameObject.GetComponent<Terrain>())
			return;
		
		bool existance = false;	//Used to check the existance of the surface
		int i = TerrainSurface.GetMainTexture (transform.position);
		for (int j = 0; j < surfacesOnTerrain.Length; ++j) 
		{
			if (j == i) {
				existance = true;	// if index i of terrain main texture also exists in the terrain surfaces array
				break;
			}
		}
		if (existance) {
			if (surfacesOnTerrain [i].surfaceFootStepSounds != null) {
				_currentSurface = surfacesOnTerrain [i];
				n2 = Random.Range (0, surfacesOnTerrain [i].surfaceFootStepSounds.Length - 1);
				_audioSource.clip = surfacesOnTerrain [i].surfaceFootStepSounds [n2];
			} else {
				Debug.LogError ("No Footsteps Sound Set For " + surfacesOnTerrain [i].name);
			}
		} 
		else
		{
			if (playDefaultFootstepsSoundForUndefinedSurfaces) {
				if (defaultFootstepSounds != null) {
					n3 = Random.Range (0, defaultFootstepSounds.Length - 1);
					_audioSource.clip = defaultFootstepSounds [n3];
					_currentSurface = null;
				} else {
					Debug.LogError ("No Default Landing Sound Set!");
				}
			}
		}

		_audioSource.PlayOneShot (_audioSource.clip, audioVolume);
	}
}