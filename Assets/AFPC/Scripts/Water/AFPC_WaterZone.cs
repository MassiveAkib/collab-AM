using UnityEngine;

public class AFPC_WaterZone : MonoBehaviour {

	public Camera playerCamera;
	public Color fogColor = new Color(0f, 192f, 255f, 106f);
	public FogMode fogMode = FogMode.ExponentialSquared;
	public float fogDensity = 0.2f;
	public float magnitudeOfVelocityToInstantiateWaterEnterSplash = 10f;
	public bool useWaterSplashesAndRipples = true;
	public bool flipPlane = true; // when player is underwater, water plane faces the opposite direction
	public GameObject waterEnterSplash;	//The water splash when a rigidbody enters the water zone with the magnitude of velocity greater than magnitudeOfVelocityToInstantiateWaterEnterSplash
	[Tooltip("Set these values same as the duration of the respective particle effects or just leave them")]
	public float waterEnterSplashEffectDestroyTime;
	public float waterMovementSplashEffectDestroyTime;
	public float waterStaticRippleEffectDestroyTime;
	public GameObject waterStaticRipple; // The water ripples when the player is in water but not moving
	public GameObject waterMovementSplash; // the water splashes when the player is in water and is moving
	public bool useWaterSoundEffects = true;
	public float audioVolume = 0.7f;
	public AudioClip waterEnterSound, waterExitSound;
	public AudioClip playerUnderWaterSoundEffect;
	public AudioClip[] playerWaterMovementSoundOnWaterSurface;
	public float timeToReapeatPlayerMovementSound = 0.4f;
	public Transform waterPlane;
	public Camera imageEffectsCamera;

	[HideInInspector]
	public bool isUnderWater = false;	// true if player is under water, else false

	private AFPC_PlayerMovement afpcPlayer;
	private bool playerInWaterZone = false;

	private Vector3 waterStaticRippleV; // used in smoothdamp
	private float originalLightIntensity;
	private float intensityV; // Used in SmoothDamp
	private bool initialFogEnabled;
	private Color initialFogColor;
	private FogMode initialFogMode;
	private float initialFogDensity;
	private bool invokeStaticRippleFlag = false;
	private bool invokeMovementSplashFlag = false;
	private bool invokePlayerMovementSoundFlag = false;
	private AudioSource _audioSrc; // the audiosource attached to this gameobject

	private AFPC_Cam _cam;
	private Rigidbody playerRigidbody;
	private Vector3  currV;
	private int n = 0; // index for playerwatermovement sounds
	private bool playingUnderWaterSound = false;
	private bool previousUseUnderwaterImageEffects;
	void OnTriggerEnter(Collider coll)
	{
		if (useWaterSplashesAndRipples && coll.gameObject.GetComponent<Rigidbody> ()) 
		{
			if(coll.gameObject.GetComponent<Rigidbody> ().velocity.sqrMagnitude > magnitudeOfVelocityToInstantiateWaterEnterSplash)
				InstantiateWaterEnterSplash (coll.transform.position);
		}

		afpcPlayer = coll.gameObject.GetComponent<AFPC_PlayerMovement> ();
		if (coll.gameObject.GetComponent<AFPC_PlayerMovement> ())
		{
			afpcPlayer = coll.gameObject.GetComponent<AFPC_PlayerMovement> ();
			playerInWaterZone = true;
			afpcPlayer.IsSwimming = true;
		}
		if (useWaterSoundEffects && waterEnterSound != null) 
		{
			if (afpcPlayer != null) 
			{
				if(afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer)
					AudioSource.PlayClipAtPoint (waterEnterSound, coll.transform.position, audioVolume);
			}
		}

	}

	void OnTriggerStay(Collider coll)
	{
		
		if (coll.gameObject.GetComponent<AFPC_PlayerMovement> () 	/*If our player is in the trigger area*/)
		{
			afpcPlayer = coll.gameObject.GetComponent<AFPC_PlayerMovement> ();
			playerInWaterZone = true;
			afpcPlayer.IsSwimming = true;
		}
	}

	void OnTriggerExit(Collider coll)
	{
		CancelInvoke ("InstantiateWaterStaticRipple");
		invokeStaticRippleFlag = false;
		CancelInvoke ("InstantiateWaterMovementSplash");
		invokeMovementSplashFlag = false;

		if (coll.gameObject.GetComponent<AFPC_PlayerMovement> ())
		{
			afpcPlayer = coll.gameObject.GetComponent<AFPC_PlayerMovement> ();
			playerInWaterZone = false;
			afpcPlayer.IsSwimming = false;
		}
		if (useWaterSoundEffects && waterExitSound !=null) 
		{
			if (afpcPlayer != null) {
				if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer)
					AudioSource.PlayClipAtPoint (waterExitSound, coll.transform.position, audioVolume);
			}
		}

	}

	void InstantiateWaterEnterSplash (Vector3 pos)
	{
		if (waterPlane != null)
			pos.y = waterPlane.transform.position.y;
		GameObject waterSplash = Instantiate (waterEnterSplash, pos, waterEnterSplash.transform.rotation);
		if (waterEnterSplashEffectDestroyTime != 0 && waterEnterSplashEffectDestroyTime > 0)
			Destroy (waterSplash, waterEnterSplashEffectDestroyTime);
		else
			Destroy (waterSplash, waterEnterSplash.GetComponent<ParticleSystem> ().main.duration + 1f);	// Wait for the duration of the particle effect + 1 seconds and then destroy waterEnterSplash
	}

	void InstantiateWaterMovementSplash ()
	{
		if (afpcPlayer != null) 
		{
			if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
				GameObject waterSplash = Instantiate (waterMovementSplash, afpcPlayer.transform.position, waterMovementSplash.transform.rotation);
				if (afpcPlayer.playerOnWaterSurface) {
					if (waterMovementSplashEffectDestroyTime != 0 && waterMovementSplashEffectDestroyTime > 0)
						Destroy (waterSplash, waterMovementSplashEffectDestroyTime);
					else
						Destroy (waterSplash, waterMovementSplash.GetComponent<ParticleSystem> ().main.duration + 1f);	// Wait for the duration of the particle effect + 1 seconds and then destroy waterMovementSplash
				} else {
					Destroy (waterSplash);
				}
			}
		}
	}


	void InstantiateWaterStaticRipple()
	{
		if (afpcPlayer != null) 
		{
			if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
			GameObject waterSplash = Instantiate (waterStaticRipple, afpcPlayer.transform.position, waterStaticRipple.transform.rotation);
				if (afpcPlayer.playerOnWaterSurface) {
			if (waterStaticRippleEffectDestroyTime != 0 && waterStaticRippleEffectDestroyTime > 0)
				Destroy (waterSplash, waterStaticRippleEffectDestroyTime);
			else
				Destroy (waterSplash, waterStaticRipple.GetComponent<ParticleSystem> ().main.duration + 1f);	// Wait for the duration of the particle effect + 1 seconds and then destroy waterStaticRipple
				}else{
					Destroy (waterSplash);
				} 
			}
		}
	}

	// Use this for initialization
	void Start () 
	{

		_cam = playerCamera.GetComponent<AFPC_Cam> ();
		if (_cam.useUnderwaterImageEffects) 
		{
			imageEffectsCamera.GetComponent<AFPC_UnderWaterDisplaceEffect> ().enabled = false;
			imageEffectsCamera.GetComponent<AFPC_UnderWaterBlurEffect> ().enabled = false;
		}

		if (GetComponent<AudioSource> ())
			_audioSrc = GetComponent<AudioSource> ();
		else
			gameObject.AddComponent<AudioSource> ();
		_audioSrc = GetComponent<AudioSource> ();
		_audioSrc.volume = audioVolume;
		_audioSrc.playOnAwake = true;
		initialFogDensity = RenderSettings.fogDensity;
		initialFogEnabled = RenderSettings.fog;
		initialFogMode = RenderSettings.fogMode;
		initialFogColor = RenderSettings.fogColor;
	}

	void PlayerMovementSound()
	{
		if (afpcPlayer != null) 
		{
			if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
				if (afpcPlayer.playerOnWaterSurface) {
					n = Random.Range (0, playerWaterMovementSoundOnWaterSurface.Length);
					AudioSource.PlayClipAtPoint (playerWaterMovementSoundOnWaterSurface [n], afpcPlayer.transform.position, audioVolume);
				}
			}
		}
	}

	void SwitchImageEffects ()
	{
		if (!_cam.useUnderwaterImageEffects && previousUseUnderwaterImageEffects) 
		{
			imageEffectsCamera.GetComponent<AFPC_UnderWaterDisplaceEffect> ().enabled = false;
			imageEffectsCamera.GetComponent<AFPC_UnderWaterBlurEffect> ().enabled = false;
		}	
	}

	// Update is called once per frame
	void Update () 
	{
		if (previousUseUnderwaterImageEffects != _cam.useUnderwaterImageEffects)
			SwitchImageEffects ();
		previousUseUnderwaterImageEffects = _cam.useUnderwaterImageEffects;
		if (!isUnderWater)
			playingUnderWaterSound = false;

		if (_audioSrc.clip == playerUnderWaterSoundEffect)
			_audioSrc.loop = true;
		else
			_audioSrc.loop = false;
		
		if (afpcPlayer != null)
		{
			if (afpcPlayer.playerState != AFPC_PlayerMovement.PlayerStates.Swim_2)
			{
				// Make sure to stop making ripples and water splashes when we switch back to our previous State
				CancelInvoke ("InstantiateWaterStaticRipple");
				invokeStaticRippleFlag = false;
				CancelInvoke ("InstantiateWaterMovementSplash");
				invokeMovementSplashFlag = false;
			}
		}

		if (afpcPlayer != null && playerInWaterZone && afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer && !afpcPlayer.downHold)
		{
			if (!isUnderWater)
			{
				playerRigidbody = afpcPlayer.GetComponent<Rigidbody> ();
				if (playerRigidbody.velocity.y < float.Epsilon && (playerRigidbody.velocity.x < float.Epsilon || playerRigidbody.velocity.z < float.Epsilon)) {
					float dist = 0f;
					if (waterPlane != null)
						dist = waterPlane.transform.position.y - 0.1f;
					else
						dist = afpcPlayer.transform.position.y;
					playerRigidbody.MovePosition(Vector3.SmoothDamp (afpcPlayer.transform.position, new Vector3 (afpcPlayer.transform.position.x, dist, afpcPlayer.transform.position.z), ref currV, 0.5f));
				}
			}
		}
		// It ensures that the underwater effect only take place when player eyes or Camera are inside water
		if (playerInWaterZone && playerCamera.transform.position.y < waterPlane.transform.position.y /* It means player is underwater*/) 
		{
			if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
				invokePlayerMovementSoundFlag = false;
				CancelInvoke ("PlayerMovementSound");
				CancelInvoke ("InstantiateWaterStaticRipple");
				invokeStaticRippleFlag = false;
				CancelInvoke ("InstantiateWaterMovementSplash");
				invokeMovementSplashFlag = false;
			}
			if (_cam.useUnderwaterImageEffects) {
				imageEffectsCamera.GetComponent<AFPC_UnderWaterDisplaceEffect> ().enabled = true;
				imageEffectsCamera.GetComponent<AFPC_UnderWaterBlurEffect> ().enabled = true;
			}
			if (useWaterSoundEffects && playerUnderWaterSoundEffect !=null) 
			{
				if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer && !playingUnderWaterSound) {
					_audioSrc.clip = playerUnderWaterSoundEffect;
					_audioSrc.Play ();
					playingUnderWaterSound = true;
				}
			}
			isUnderWater = true;
			if (flipPlane)
			{
				if (waterPlane.transform.localRotation.x >= 0f) 
				{
					Quaternion requiredRotation = new Quaternion (-waterPlane.localRotation.x, waterPlane.localRotation.y, waterPlane.localRotation.z, waterPlane.localRotation.w);
					waterPlane.localRotation = requiredRotation;
				}
			}
			if (afpcPlayer != null) 
			{
				if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
					afpcPlayer.DecreaseOxygen = true;
					afpcPlayer.playerOnWaterSurface = false;
					afpcPlayer.GetComponent<Rigidbody> ().useGravity = afpcPlayer.useGravityWhileSwimming;
				}
			}
			RenderSettings.fog = true;
			RenderSettings.fogColor = fogColor;
			RenderSettings.fogMode = fogMode;
			RenderSettings.fogDensity = fogDensity;
		} else if (playerInWaterZone && Mathf.Abs(playerCamera.transform.position.y - waterPlane.transform.position.y) < 0.4f && Mathf.Abs(playerCamera.transform.position.y - waterPlane.transform.position.y) > 0.05f) 
		{
			
			_audioSrc.Stop ();
			if (afpcPlayer != null) 
				afpcPlayer.playerOnWaterSurface = true;
			
			if (_cam.useUnderwaterImageEffects) 
			{
				imageEffectsCamera.GetComponent<AFPC_UnderWaterDisplaceEffect> ().enabled = false;
				imageEffectsCamera.GetComponent<AFPC_UnderWaterBlurEffect> ().enabled = false;
			}
			if (flipPlane)
			{
				if (waterPlane.transform.localRotation.x <= 0f) 
				{
					Quaternion requiredRotation = new Quaternion (-waterPlane.localRotation.x, waterPlane.localRotation.y, waterPlane.localRotation.z, waterPlane.localRotation.w);
					waterPlane.localRotation = requiredRotation;
				}
			}
			if (afpcPlayer != null) 
			{
				if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
					afpcPlayer.DecreaseOxygen = false;
				    afpcPlayer.CanJumpOnWaterSurface = true;
					if (afpcPlayer.GetComponent<Rigidbody> ().velocity.sqrMagnitude > 2f) {
						CancelInvoke ("InstantiateWaterStaticRipple");
						invokeStaticRippleFlag = false;
						if (!invokeMovementSplashFlag) {
							invokeMovementSplashFlag = true;
							InvokeRepeating ("InstantiateWaterMovementSplash", 0.01f, 0.2f);
						}
						if (useWaterSoundEffects && playerWaterMovementSoundOnWaterSurface != null && !invokePlayerMovementSoundFlag) {
							invokePlayerMovementSoundFlag = true;
							InvokeRepeating ("PlayerMovementSound", 0.01f, timeToReapeatPlayerMovementSound);
						}
					} else if (afpcPlayer.GetComponent<Rigidbody> ().velocity.sqrMagnitude < 2f) {
						invokeMovementSplashFlag = false;
						invokePlayerMovementSoundFlag = false;
						CancelInvoke ("PlayerMovementSound");
						CancelInvoke ("InstantiateWaterMovementSplash");
						if (!invokeStaticRippleFlag) {
							invokeStaticRippleFlag = true;
							InvokeRepeating ("InstantiateWaterStaticRipple", 0.01f, 0.25f);
						}
					}
				}
			}

			isUnderWater = false;
			RenderSettings.fog = initialFogEnabled;
			RenderSettings.fogColor = initialFogColor;
			RenderSettings.fogMode = initialFogMode;
			RenderSettings.fogDensity = initialFogDensity;
		} else 
		{
			if (afpcPlayer != null) {
				if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
					invokePlayerMovementSoundFlag = false;
					CancelInvoke ("PlayerMovementSound");
					_audioSrc.Stop ();
				}
			}
			if (flipPlane)
			{
				if (waterPlane.transform.localRotation.x <= 0f) 
				{
					Quaternion requiredRotation = new Quaternion (-waterPlane.localRotation.x, waterPlane.localRotation.y, waterPlane.localRotation.z, waterPlane.localRotation.w);
					waterPlane.localRotation = requiredRotation;
				}
			}
			if (_cam.useUnderwaterImageEffects)
			{
				imageEffectsCamera.GetComponent<AFPC_UnderWaterDisplaceEffect> ().enabled = false;
				imageEffectsCamera.GetComponent<AFPC_UnderWaterBlurEffect> ().enabled = false;
			}
			if (afpcPlayer != null)
			{
				if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) {
					afpcPlayer.DecreaseOxygen = false;
					afpcPlayer.CanJumpOnWaterSurface = false;
					afpcPlayer.playerOnWaterSurface = false;
				}
			}

			isUnderWater = false;
			RenderSettings.fog = initialFogEnabled;
			RenderSettings.fogColor = initialFogColor;
			RenderSettings.fogMode = initialFogMode;
			RenderSettings.fogDensity = initialFogDensity;
		}
	}
}