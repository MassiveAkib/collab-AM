/* The Main Script of This Asset, handles all the player movement!
 * Created By Harsh Pandey, gameDev Mode
*/

using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class AFPC_PlayerMovement : MonoBehaviour {

	public enum PlayerStates	
	{
		Standing_0, Crouch_1, Swim_2, Climb_3	// These are the player states that player can have
	}
	public enum PlayerType
	{
		rigidBodyPlayer, spectator	// The Types of player we have - Use Rigidbody Player type for normal first person controller 
	}
	public enum FootstepsDetectionMode
	{
		basedOnTags, basedOnTextures	/* These are the footstep detection mode
										 * Use Based On Tags if you simply want to change foostep sound according to tag of Ground
										 * Use Based On Textures if you want to change footstep sound according to the texture of Ground
										 * REMEMBER -- BASED ON TAGS FOOTSTEPS DETECTION MODE DOES NOT SUPPORT MULTIPLE FOOTSTEP 
										 * SOUND ON A TERRAIN BECAUSE IT DEALS WITH TAG OF THE GROUND
										*/
	}

	public FootstepsDetectionMode footstepsDetectionMode;
	public PlayerType playerType;
	public LayerMask layerMask;
	[Tooltip("Refers to the player current state")]
	public PlayerStates playerState;

	//Mobile Input
	public AFPC_VirtualJoystick movementJoystick;	// The PLayer Movement virtual joystick
	public AFPC_VirtualButton jumpButton, crouchButton, runButton, goDownButton, spectatorForwardButton, spectatorDownButton;// Self Explanatory


	public bool CANMOVE = true, CANJUMP = true, CANCLIMB = true, CANSWIM = true, CANCROUCH = true; // If they are true, then player can perform these specific actions
	public bool lockCursor = true; // locks the cursor when in game
	[Tooltip("Set to Locked if you want invisible cursor which is always in the center of the screen")]
	public CursorLockMode wantedCursorLock;
	public KeyCode disableCursorLock = KeyCode.Escape;

	public float maximumSpectatorSpeed = 40f;
	public bool useGravityInSpectatorMode = false;
	public KeyCode spectatorForwardKey = KeyCode.W;
	public float spectatorModeAcceleration = 15f;
	public float jumpForceInSpectatorMode = 50f;
	public float spectatorModeDrag = 0f;
	public float spectatorRetardation = 0.4f;
	public KeyCode spectatorUpKey = KeyCode.Space, spectatorDownKey = KeyCode.LeftControl;
	public float playerDrag = 5f;	// Drag of the player's rigidbody 

	public bool stopImmediately = false; // If True then player stops instantly when their is no movement, if it is false then player stops based on the value of retardationTime
	public float maximumWalkSpeed = 75f;
	public float walkAccelerationForward = 60f, walkAccelerationBackward = 40f;
	public float walkStrafeAcceleration = 50f;
	public float slideAcceleration = 5f;
	public float runStrafeAcceleration = 60f;
	public float runAccelerationForward = 100f, runAccelerationBackward = 50f;
	public float retardationTime = 0.25f;	//Deacceleration time
	public float maxSlope = 60f;
	public float slideSlope = 45f;
	public float airControlAcceleration = 5f; //The amount of control we have when we are in air
	public KeyCode runKey = KeyCode.LeftShift;
	public bool allowSliding = true;	// If True, then player will slide on steep surfaces
	public bool AirControl = false;		// If Enabled, then player can be controlled when in air 
	public bool infiniteRunning = false;	//If Enabled then the player could run as long as the run key is held down
	public bool runWithoutAxisInput = false; // If true, then Axis Input is not Required To Run, Only RunKeyHold Can run the player
	public bool visualize = false; // If Set To True and the gizmos are set true for game window, then the ground check 1 sphere and Sticktoground helper sphere will be drawn on the screen
	public AnimationCurve slopeCurve = new AnimationCurve(new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));	
	/* this is the animation curve to whose values are multiplied by the desired move to change
     * the amount of movement depending on the angle of ground!
     * If angle between player and ground is 90f, then no movement will take place because the value of curve will be zero(if you didn't made any changes to the curve)
	*/

	//FOV - Field Of View (Of fpsCamera)
	public bool useFOVKick = true;	// If true then FOV Kick is done when running
	[Range(1, 179)]
	public float runFOV = 75f;	// The Amount of FOV of Camera while Running
	public float timeToIncreaseFOV = 0.25f, timeToDecreaseFOV = 0.25f;

	private bool canUseFOVKick = true; /* If true then FOVKick will be used, Its value is assinged in game, if their is enough space in 
										* front of the player(no Collider in front of player), then FOV Kick will not HAPPEN 
										* even when useFOVKick is set to true!
									   */

	[Tooltip("Prevents bouncing of player while moving down slopes")]
	[Header("Stick To Ground")]
	public float shellOffset = 0.1f;	/* It is the amount by which the radius of groundNormal, groundcheck1 and  stickToGroundHelper 
										 * Spheres is less than the radius of PlayerCapsule
										*/ 
	public float stickToGroundHelperDistance = 0.6f; /* distance to check below the player to stay grounded when their is sudden 
													  * change in the angle between the player and the ground to remain grounded!
													 */ 
	public float groundCheckDistance = 0.025f; // The Distance For GroundCheck1 to be true!
	public float audioVolume = 1f; // volume for audio source attached to this gameobject 

	[Header("Jump")]
	public float jumpForce;
	public bool jumpForceDependsOnTheTimeJumpButtonPressed = true;
	public bool useFallMultiplier = true;	/* If set to true, then player will fall depending on the fall multiplier amount,
											 * use 1f for normal falling or simply set this boolean to false
											*/ 
	public float dragWhileJumping = 7f;
	public AudioClip jumpUpSound;
	private AudioClip landingSound;
	public AudioClip LandingSound{get{ return landingSound;} set{ landingSound = value;}}

	[Tooltip("Affects the time it takes to get down")]
	public float fallMultiplier = 2.5f; 
	public float lowJumpMultiplier = 2f; /* lowJumpMultiplier is only used when jumpForceDependsOnTheTimeJumpButtonPressed is set to true
										  * It is the amount by which the height of jump depends on the time the jump button was pressed down!	
										 */
	[HideInInspector]
	public bool isGrounded = true;


	#region Tilt Upcoming In next Update
	[Header("Player Tilt")]
	public bool allowPlayerTilting = true;
	public KeyCode leftTiltKey = KeyCode.Q, rightTiltKey = KeyCode.E;
	public AFPC_VirtualButton leftTiltVButton, rightTiltVButton;
	public float timeToTilt = 0.2f;
	[Range(0f, 90f)]
	public float angleToTilt = 5f;

	private bool isTilting = false; // Boolean variable used to identify if the player is tilting or not
	public bool IsTilting{get{ return isTilting;}}
	#endregion

	[Header("Footsteps")]
	[Range(0f, 1f)]
	public float runningFootstepDelayFactor = 0.6f;
	public float walkingFootstepSoundInterval = 0.8f;	// The time in seconds after which next footstep sound will play while walking
	public float runningFootstepSoundInterval = 0.5f;   // The time in seconds after which next footstep sound will play while running
	[HideInInspector]
	public  float stepCycle, nextStep;
	public float stepInterval = 6f; 

	[Header("Climbing")]
	public PhysicMaterial playerMaterial;	// The Physic Material Of The Capsule Collider Or Player!
	public float climbAcceleration = 25f;
	public float climbDrag = 0f;
	public bool useGravityWhileClimbing = false;
	public KeyCode climbKey = KeyCode.W;
	public bool useMouseScrollForClimbing = false;	// If true, then you can climb by scrolling the mouse
	public bool useClimbingSound = true;
	public AudioClip[] climbingSounds;


	public bool useJoystickInSpectatorMode = false; // If true then in Mobile platform in spectator mode movementJoystick will be used for movement else spectatorForwardButton

	[Header("Crouching")]
	private GameObject pivot;	/* The Gameobject according to which the player localscale should be 
								 * changed when crouching in order to remain grounded
								 * In case I have changed the localScale of transform normally, then 
								 * scale would change from the center of capsule which will eventually lead to altering 
								 * of value of isGrounded 
								*/
	public float crouchAccelerationForward = 10f, crouchAccelerationBackward = 6f, crouchStrafeAcceleration = 8f;
	public float playerDynamicFriction = 1f, playerStaticFriction = 1f;
	public float currentCrouchStandingRatio = 0f; // If it is 1, then player is standing, if it is 0 then player is crouching
	public float floatOffset = 0.05f; /* Float Offest Is how much above the player, the
									   * crouch Collision Detection capsule will Sweep for Collision Detection
									  */ 
	public KeyCode crouchKey = KeyCode.C; 
	public float originalLocalScaleY; // The length or height of the transform along y axis
	[Range(0, 1)]
	public float crouchHeightRatio = 0.6f; //The ratio of crouch height to original height, if it is 0, player will be on the bottom most and if it is 0.5, the crouch height will be in the middle 	 
	[Tooltip("In Seconds")]
	public float transitionTimeOfCrouch = 0.2f;

	[Header("Swimming")]
	public float swimAcceleration = 15f;
	public float swimDrag = 8f;
	public bool useGravityWhileSwimming = false;
	public KeyCode swimForwardKey = KeyCode.W;
	public KeyCode goDownKey = KeyCode.LeftControl;
	public float jumpUpThrustForceUnderWater = 15f;
	public float jumpUpThrustForceOnWaterSurface = 5f;
	public float goDownForceWhileSwimming = 10f;

	[HideInInspector]
	public bool playerOnWaterSurface = false; // determines whether the player is underwater or on water surface, useful in oxygen counter

	public Camera fpsCamera;
	public CapsuleCollider playerCapsule;


	private Vector3 crouchV; // used in smoothdamp
	private bool canClimb = false; // If Enabled, then player can climb, if not then player will move normaly
	public bool CanClimb{ set { canClimb = value; } get { return canClimb; } }
	private float crouchLocalScaleY; // Local Scale of player in y axis when crouching 
	private bool isJumping = false; // If true, then player is jumping
	public bool IsJumping{	get{return isJumping;}}
	private bool isSwimming = false; // If true, then player is swimming, if not then player is not swimming
	public bool IsSwimming{ set{isSwimming = value;} get{ return isSwimming;}}
	private bool isCrouching = false;
	public bool IsCrouching{ set{ isCrouching = value;} get{ return isCrouching;}}
	private bool isSliding = false;
	public bool IsSliding{	get{ return isSliding;}}
	private bool isRunning = false; // Boolean variable used to identify if the player is running or not
	public bool IsRunning{get{return isRunning;} set{isRunning = value;}}

	private bool canJumpOnWaterSurface = true;
	public bool CanJumpOnWaterSurface{get{ return canJumpOnWaterSurface;}set{ canJumpOnWaterSurface = value;}}
	private bool decreaseOxygen = false;
	public bool DecreaseOxygen{get{ return decreaseOxygen;}set{ decreaseOxygen = value;}}
	private float retardationxV, retardationyV, retardationzV; //Used as a ref argument in SmoothDamp for creating friction effect
	private Rigidbody rgbd;
	private float fpsCamInitialFOV;
	private float fov; // passed as a ref argument when smooth dampening the FOV change in fpsCam while running
	private Vector2 horizontalMovement;
	private float rgbdx, rgbdy, rgbdz;	// The x, y and z movement of the rigidbody of the player 
	private AFPC_StaminaManager staminaManager;
	private AFPC_SpawnManager spawnManager;
	private AFPC_OxygenManager oxygenManager;
	private float zRotationV; // used in smooth damp
	private Vector3 rgbdV; // used in smooth damp
	[HideInInspector]
	public AFPC_FootstepsManager footstepManagerTexture;
	[HideInInspector]
	public AFPC_FootstepsSystem footstepManagerTag;
	private RigidbodyConstraints initialRigidbodyConstraints;
	private AudioSource _audioSrc;
	private float currentAcceleration;
	private int n = 0; // stores the index of current climbingSound
	private bool groundCheck1 = false, groundCheck2 = false;
	private Collider[] colls; 

	/*Upcoming
	 *[HideInInspector]
	 *public float zRotation; // Rotation in the z axis of player when tilting
	*/
	private Vector3 _groundNormal; // the normal to the surface in contact
	#region CoroutineFlags
	private bool decreaseStaminaInvokeFlag = false; // to make sure that the decrease stamina InvokeRepeating doesn't start on every frame or Update()
	private bool increaseStaminaInvokeFlag = false; // to make sure that the increase stamina InvokeRepeating doesn't start on every frame or Update()
	private bool decreaseOxygenInvokeFlag = false;  // to make sure that the decrease oxygen InvokeRepeating doesn't start on every frame or Update()
	private bool increaseOxygenInvokeFlag = false;  // to make sure that the increase oxygen InvokeRepeating doesn't start on every frame or Update()
	private bool climbingSoundCoroutineFlag = false;
	#endregion

	#region InputVariables
	[HideInInspector]
	public float inputX, inputY; //The Horizontal And Veritcal Axis Input Resceptively
	private float inputY1; // Vertical input for climbing
	#if UNITY_STANDALONE || UNITY_WEBGL
	private float mouseScrollDeltaY; // the mouse scroll delta y for climbing if climb using scroll is enabled
	#endif
	private float inputY2 = 0f; //Vertical Input For Swimming
	#if UNITY_STANDALONE || UNITY_WEBGL
	private bool swimKeyHold;
	#endif
	private bool jumpPressed, climbKeyHold, runKeyHold, crouchButtonDown, spectatorKeyHold;
	[HideInInspector]
	public bool downHold; 
//	private bool leftTiltPressed, rightTiltPressed;	// Variables which are true when their respective inputs are true
	#endregion
	private AFPC_Cam cam;
	private Vector3 desiredMove = Vector3.zero;
	private float initFallMultiplier;
	private bool initUseFallMultiplier;

	private float initPMDF, initPMSF; // initPMDF - Initial Physic Material Dynamic Friction
									  // initPMSF - Initial Physic Material Static Friction
	private bool previouslyGrounded; // The player was grounded on the last update or not

	private Vector3 spectatorInput;
	private PlayerType previousPlayerType;
	// Use this for initialization
	void Start () 
	{
		if (GetComponent<Rigidbody> ()) {
			rgbd = GetComponent<Rigidbody> ();
		} else {
			gameObject.AddComponent<Rigidbody> ();
			rgbd = GetComponent<Rigidbody> ();
		}

		if (playerType == PlayerType.rigidBodyPlayer) {

			stepCycle = 0f;
			nextStep = stepCycle / 2f;
			pivot = new GameObject ("CrouchPivot");	
			pivot.transform.SetSiblingIndex (transform.GetSiblingIndex () + 1); // sets the index in hierarchy to be one before the afpcPlayer

			if (footstepsDetectionMode == FootstepsDetectionMode.basedOnTags) {
				if (GetComponent<AFPC_FootstepsSystem> ())
					footstepManagerTag = GetComponent<AFPC_FootstepsSystem> ();
				else
					Debug.LogError ("No Footsteps Manager Based On Tag is found!");
			} else if (footstepsDetectionMode == FootstepsDetectionMode.basedOnTextures) {
				if (GetComponent<AFPC_FootstepsManager> ())
					footstepManagerTexture = GetComponent<AFPC_FootstepsManager> ();
				else
					Debug.LogError("No Footsteps Manager Based On Texture is found!");
			}

			if (playerMaterial != null)
			{
				// Initializing the player material friction values
				playerMaterial.dynamicFriction = playerDynamicFriction;
				playerMaterial.staticFriction = playerStaticFriction;
				initPMDF = playerMaterial.dynamicFriction;
				initPMSF = playerMaterial.staticFriction;
			}
				
			initFallMultiplier = fallMultiplier;
			initUseFallMultiplier = useFallMultiplier;
	
			if (GetComponent<AFPC_SpawnManager> ())
				spawnManager = GetComponent<AFPC_SpawnManager> ();
			if (fpsCamera != null)
				cam = fpsCamera.GetComponent<AFPC_Cam> ();
			staminaManager = GetComponent<AFPC_StaminaManager> ();
			oxygenManager = GetComponent<AFPC_OxygenManager> ();
			if (playerCapsule == null)
				playerCapsule = GetComponent<CapsuleCollider> ();
			if (GetComponent<AudioSource> ())
				_audioSrc = GetComponent<AudioSource> ();
			else
				gameObject.AddComponent<AudioSource> ();

			_audioSrc = GetComponent<AudioSource> ();
			_audioSrc.volume = audioVolume;
			_audioSrc.playOnAwake = false;

			if (currentCrouchStandingRatio == 0)
				isCrouching = true; // If the Player Set the crouch standing ratio to 0, then crouching should be done

			originalLocalScaleY = transform.localScale.y;
			crouchLocalScaleY = transform.localScale.y * crouchHeightRatio;	// Set the crouch local scale in Y axis to crouchHeight Ratio Times Original Local scale in Y axis
			#if UNITY_STANDALONE || UNITY_WEBGL
			// Cursor Lock Should Only Be there when playing in standalone platform 
			if (lockCursor) {
				Cursor.lockState = wantedCursorLock;
				Cursor.visible = false;
			}
			#endif
			fpsCamInitialFOV = fpsCamera.fieldOfView;
			#region Initialize Rigidbody
			rgbd.freezeRotation = true;
			rgbd.useGravity = true;
			rgbd.isKinematic = false;
			initialRigidbodyConstraints = rgbd.constraints;
			#endregion
		} else if (playerType == PlayerType.spectator) 
		{
			#if UNITY_STANDALONE || UNITY_WEBGL
			// Cursor Lock Should Only Be there when playing in standalone platform 
			if (lockCursor) {
				Cursor.lockState = wantedCursorLock;
				Cursor.visible = false;
			}
			#endif
			rgbd.freezeRotation = true;
			rgbd.useGravity = false;
			rgbd.isKinematic = false;
			initialRigidbodyConstraints = rgbd.constraints;
		}
	}

	void IncreaseFOV()
	{
		fpsCamera.fieldOfView = Mathf.SmoothDamp (fpsCamera.fieldOfView, runFOV, ref fov, timeToIncreaseFOV);
	}

	void DecreaseFOV()
	{
		fpsCamera.fieldOfView = Mathf.SmoothDamp (fpsCamera.fieldOfView, fpsCamInitialFOV, ref fov, timeToDecreaseFOV);
	}

	public bool AboveCollisionDetection()
	{
		Vector3 endOfCapsule = new Vector3 (transform.position.x, 0f, transform.position.z);
		endOfCapsule.y = transform.position.y + (playerCapsule.height * transform.localScale.y / 2) + 0.25f + floatOffset;
		return Physics.CheckCapsule (transform.position, endOfCapsule, playerCapsule.radius, layerMask, QueryTriggerInteraction.Ignore);
		//Returns TRUE if there are any colliders overlapping the capsule defined by position and radius in world coordinates.
	}

	public void BelowCollisionDetection()
	{
		float distanceFromGround =	transform.position.y - (playerCapsule.height * transform.localScale.y / 2f) + (playerCapsule.radius * (1.0f - shellOffset)) - groundCheckDistance;
		Vector3 origin = new Vector3 (transform.position.x, distanceFromGround, transform.position.z);
		colls = Physics.OverlapSphere (origin, playerCapsule.radius * (1.0f - shellOffset), layerMask, QueryTriggerInteraction.Ignore);

		if (colls.Length >= 1)
			groundCheck1 = true;
		else if (colls.Length == 0)
			groundCheck1 = false;
		else
			groundCheck1 = false;
	}

	#region Crouching

	void CrouchSystem()
	{
		float transformPosY = Mathf.Lerp (crouchLocalScaleY, originalLocalScaleY, currentCrouchStandingRatio);	//Linearly Interpolate the transformPosY depending on the crouchStanding Ratio
		Vector3 DesiredlocalScale = Vector3.SmoothDamp(transform.localScale, new Vector3 (transform.localScale.x, transformPosY, transform.localScale.z), ref crouchV, transitionTimeOfCrouch);

		ScaleAround (transform, pivot.transform, DesiredlocalScale);

		if (isRunning && isCrouching /*If Run key is pressed while crouching*/) 
		{
			DoUnCrouch ();
		}

		if (jumpPressed && isCrouching /*If Jump Key Is pressed during crouching*/) 
		{
			DoUnCrouch ();
			jumpPressed = false;
		}
		#if UNITY_ANDROID || UNITY_IOS
		if(crouchButton != null)
			crouchButton.buttonType = AFPC_VirtualButton.ButtonType.LateTriggerButton;
		#endif
		if (crouchButtonDown) 
		{
			if (!isCrouching) {
				// If not already crouching, then Crouch 
				DoCrouch ();
			} else 
			{
				//If crouching, then UnCrouch
				DoUnCrouch ();
			}
			crouchButtonDown = false;
		}
	}

	void DoCrouch()
	{
		isCrouching = true;
		currentCrouchStandingRatio = 0f;
	}

	void DoUnCrouch()
	{
		if (AboveCollisionDetection () == false) 
		{
			isCrouching = false;
			currentCrouchStandingRatio = 1f;
		}
	}

	private void ScaleAround(Transform target, Transform pivot, Vector3 scale) {
		Transform pivotParent = pivot.parent;
		Vector3 pivotPos = pivot.position;
		pivot.parent = target;      
		target.localScale = scale;
		target.position += pivotPos - pivot.position;
		pivot.parent = pivotParent;
	}

	#endregion

	#region PlayerTilt Upcoming
//	private void TiltSystem()
//	{
//		if (!(leftTiltPressed && rightTiltPressed) && (rightTiltPressed || leftTiltPressed)) {
//			if (leftTiltPressed) {
//				TiltLeft ();
//			} else if (rightTiltPressed) {
//				TiltRight ();
//			}
//		} else {
//			RemoveTilt ();
//		}
//
//	}
//
//	private void RemoveTilt()
//	{
//		zRotation = Mathf.SmoothDamp (zRotation, 0f, ref zRotationV, timeToTilt);
//		rgbd.MoveRotation (Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, zRotation));
//
//		isTilting = false;
//	}
//
//	private void TiltRight()
//	{
//		isTilting = true;
//		zRotation = Mathf.SmoothDamp (zRotation, zRotation - angleToTilt, ref zRotationV, timeToTilt);
//		zRotation = Mathf.Clamp (zRotation, -angleToTilt, 0f);
//		rgbd.MoveRotation (Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, zRotation));
//	}
//
//	private void TiltLeft()
//	{
//		isTilting = true;
//		zRotation = Mathf.SmoothDamp (zRotation, zRotation + angleToTilt, ref zRotationV, timeToTilt);
//		zRotation = Mathf.Clamp (zRotation, 0f, angleToTilt);
//		rgbd.MoveRotation (Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, zRotation));
//	}

	#endregion
	void LateUpdate()
	{
		if (Mathf.Abs (Time.timeScale) < float.Epsilon)
			return;
			
		if(playerType == PlayerType.rigidBodyPlayer && !spawnManager.HasDied && CANCROUCH)
			CrouchSystem ();

		#region Upcoming In Next Update
//		if (playerType == PlayerType.rigidBodyPlayer && allowPlayerTilting)
//			TiltSystem ();
		#endregion
	}

	private void GroundCheck()
	{
		previouslyGrounded = isGrounded;	
		BelowCollisionDetection ();	// do BelowCollisionDetection to set appropriate values of groundcheck1
		if (isJumping) 
		{
			isGrounded = false;
			if (groundCheck1 && groundCheck2)
				isJumping = false;
		}else
		{
			if (groundCheck1 && groundCheck2)
				isGrounded = true;	// Is Grounded Is True only when both ground checks are true
			else if (!groundCheck2 && !groundCheck1)
				isGrounded = false;	// player is not grounded when both ground checks are false
		}
	}

	void SwitchPlayerType ()
	{
		if (playerType == PlayerType.rigidBodyPlayer) {
			stepCycle = 0f;
			nextStep = stepCycle / 2f;
			pivot = new GameObject ("CrouchPivot");	
			pivot.transform.SetSiblingIndex (transform.GetSiblingIndex () + 1); // sets the index in hierarchy to be one before the afpcPlayer

			#if UNITY_ANDROID || UNITY_IOS
			if(movementJoystick != null)
				movementJoystick.gameObject.SetActive(true);
			if(spectatorForwardButton != null)
				spectatorForwardButton.gameObject.SetActive(false);
			if(runButton != null)
				runButton.gameObject.SetActive(true);
			if(crouchButton != null)
				crouchButton.gameObject.SetActive(true);
			if(goDownButton != null)
				goDownButton.gameObject.SetActive(true);
			if(spectatorDownButton != null)
				spectatorDownButton.gameObject.SetActive(false);
			#endif
			if (footstepsDetectionMode == FootstepsDetectionMode.basedOnTags) {
				if (GetComponent<AFPC_FootstepsSystem> ())
					footstepManagerTag = GetComponent<AFPC_FootstepsSystem> ();
				else
					Debug.LogError ("No Footsteps Manager Based On Tag is found!");
			} else if (footstepsDetectionMode == FootstepsDetectionMode.basedOnTextures) {
				if (GetComponent<AFPC_FootstepsManager> ())
					footstepManagerTexture = GetComponent<AFPC_FootstepsManager> ();
				else
					Debug.LogError ("No Footsteps Manager Based On Texture is found!");
			}

			if (playerMaterial != null) {
				// Initializing the player material friction values
				playerMaterial.dynamicFriction = playerDynamicFriction;
				playerMaterial.staticFriction = playerStaticFriction;
				initPMDF = playerMaterial.dynamicFriction;
				initPMSF = playerMaterial.staticFriction;
			}

			initFallMultiplier = fallMultiplier;
			initUseFallMultiplier = useFallMultiplier;

			if (GetComponent<AFPC_SpawnManager> ())
				spawnManager = GetComponent<AFPC_SpawnManager> ();
			if (fpsCamera != null)
				cam = fpsCamera.GetComponent<AFPC_Cam> ();
			staminaManager = GetComponent<AFPC_StaminaManager> ();
			oxygenManager = GetComponent<AFPC_OxygenManager> ();
			if (playerCapsule == null)
				playerCapsule = GetComponent<CapsuleCollider> ();
			if (GetComponent<AudioSource> ())
				_audioSrc = GetComponent<AudioSource> ();
			else
				gameObject.AddComponent<AudioSource> ();

			_audioSrc = GetComponent<AudioSource> ();
			_audioSrc.volume = audioVolume;
			_audioSrc.playOnAwake = false;

			if (currentCrouchStandingRatio == 0)
				isCrouching = true; // If the Player Set the crouch standing ratio to 0, then crouching should be done

			originalLocalScaleY = transform.localScale.y;
			crouchLocalScaleY = transform.localScale.y * crouchHeightRatio;	// Set the crouch local scale in Y axis to crouchHeight Ratio Times Original Local scale in Y axis
			#if UNITY_STANDALONE || UNITY_WEBGL
			// Cursor Lock Should Only Be there when playing in standalone platform 
			if (lockCursor) {
				Cursor.lockState = wantedCursorLock;
				Cursor.visible = false;
			}
			#endif
			fpsCamInitialFOV = fpsCamera.fieldOfView;
			#region Initialize Rigidbody
			rgbd.freezeRotation = true;
			rgbd.useGravity = true;
			rgbd.isKinematic = false;
			initialRigidbodyConstraints = rgbd.constraints;
			#endregion
		} else if (playerType == PlayerType.spectator)
		{
			Destroy (pivot);
			#if UNITY_STANDALONE || UNITY_WEBGL
			// Cursor Lock Should Only Be there when playing in standalone platform 
			if (lockCursor) {
				Cursor.lockState = wantedCursorLock;
				Cursor.visible = false;
			}
			#endif

			#if UNITY_ANDROID || UNITY_IOS
			if(!useJoystickInSpectatorMode)
			{
				if(movementJoystick != null)
					movementJoystick.gameObject.SetActive(false);
			}else
			{
				if(movementJoystick != null)
					movementJoystick.gameObject.SetActive(true);
			}
			if(spectatorDownButton != null)
				spectatorDownButton.gameObject.SetActive(true);
			if(spectatorForwardButton != null)
				spectatorForwardButton.gameObject.SetActive(true);
			if(runButton != null)
				runButton.gameObject.SetActive(false);
			if(crouchButton != null)
				crouchButton.gameObject.SetActive(false);
			if(goDownButton != null)
				goDownButton.gameObject.SetActive(false);
			#endif
			rgbd.freezeRotation = true;
			rgbd.useGravity = false;
			rgbd.isKinematic = false;
			initialRigidbodyConstraints = rgbd.constraints;
		}
	}
	// Update is called once per frame
	void Update ()
	{
		#if UNITY_STANDALONE || UNITY_WEBGL
		#region CursorLock
		if(lockCursor)
		{
			if (Input.GetKeyDown (disableCursorLock)) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			} else if (Input.GetMouseButtonDown (0) && Time.timeScale > 0f /*To Make Sure Cursor Does not become invisible when Game is Paused*/) {
				Cursor.lockState = wantedCursorLock;
				Cursor.visible = false;
			}
		}
		#endregion
		#endif

		InputManagement ();
		if (previousPlayerType != playerType) 
		{
			SwitchPlayerType ();
		}
		previousPlayerType = playerType;
		if (playerType == PlayerType.rigidBodyPlayer && !spawnManager.HasDied) {
			
			if (pivot != null)
			{
				// Set the crouchPivot gameobject's position to be on the bottom of the player capsule
				float dist = transform.position.y - (transform.localScale.y * playerCapsule.height / 2f);
				pivot.transform.position = new Vector3 (transform.position.x, dist, transform.position.z);
			}

			#if UNITY_STANDALONE || UNITY_WEBGL
			inputX = Input.GetAxis ("Horizontal");
			inputY = Input.GetAxis ("Vertical");
			#endif

			#if UNITY_ANDROID || UNITY_IOS
			if(movementJoystick != null)
			{	inputX = movementJoystick.InputVector.x;
			inputY = movementJoystick.InputVector.y;
			}else
			{
			Debug.LogError("No Movement Joystick set in PlayerMovementScript!");	
			}
			#endif

			if (playerState != PlayerStates.Swim_2)
				cam.CanBob = (rgbd.velocity.sqrMagnitude > 0.1); // camera can always bob while swimming so check if we can bob only in other states
			canUseFOVKick = (rgbd.velocity.sqrMagnitude > 0);

			horizontalMovement = new Vector2 (rgbd.velocity.x, rgbd.velocity.z);
			if (horizontalMovement.sqrMagnitude > maximumWalkSpeed * maximumWalkSpeed) {
				/* If The velocity of rigidbody is greater than the maximum permissible speed, then normalize player rigidbody
				 * and then multiply it by maximum walking speed to
				 * limit velocity of player to maximum walk speed
				*/
				horizontalMovement.Normalize ();
				horizontalMovement *= maximumWalkSpeed;
			}
			rgbdx = horizontalMovement.x;
			rgbdz = horizontalMovement.y;
			rgbdy = rgbd.velocity.y;

			#region FOVKick
			if (useFOVKick && isRunning && canUseFOVKick) {
				IncreaseFOV ();	// If We are running, Increase FOV of fpsCamera
			} else if (useFOVKick && (isRunning == false || canUseFOVKick == false)) {
				DecreaseFOV (); // If We are not Running, Decrease FOV of fpsCamera
			}
			#endregion

			if (playerState != PlayerStates.Swim_2) {
				if (!canClimb && !isGrounded)
					rgbd.drag = dragWhileJumping;

				if (!canClimb) {
					rgbd.useGravity = true;
				} else {
					rgbd.useGravity = useGravityWhileClimbing;
					rgbd.drag = climbDrag;
				}

				if (isGrounded)
					rgbd.drag = playerDrag;
			}

			if (isGrounded && (allowSliding && !isSliding)) {
				//Adding Friction Effect to x and z velocity
				if (!stopImmediately) {
					rgbdx = Mathf.SmoothDamp (rgbdx, 0f, ref retardationxV, retardationTime);
					rgbdz = Mathf.SmoothDamp (rgbdz, 0f, ref retardationzV, retardationTime);
				}
				rgbd.velocity = new Vector3 (rgbdx, rgbdy, rgbdz);
			} else if (isGrounded && !allowSliding) {
				//Adding Friction Effect to x and z velocity
				if (!stopImmediately) {
					rgbdx = Mathf.SmoothDamp (rgbdx, 0f, ref retardationxV, retardationTime);
					rgbdz = Mathf.SmoothDamp (rgbdz, 0f, ref retardationzV, retardationTime);
				}
				rgbd.velocity = new Vector3 (rgbdx, rgbdy, rgbdz);
			}

			// Rotate the player in Y axis by the currentYRotation of fps Camera
			transform.rotation = Quaternion.Euler (0f, fpsCamera.GetComponent<AFPC_Cam> ().CurrentyRotation, 0f);	
			PlayerStateChecker ();
			currentAcceleration = DesiredAcceleration(new Vector2(inputX, inputY));

		} else if (playerType == PlayerType.spectator) 
		{
			#if UNITY_STANDALONE || UNITY_WEBGL
			inputX = Input.GetAxis ("Horizontal");
			inputY = Input.GetAxis ("Vertical");
			#endif
			#if UNITY_ANDROID || UNITY_IOS
			if(useJoystickInSpectatorMode)
			{
				if(movementJoystick != null)
				{	inputX = movementJoystick.InputVector.x;
					inputY = movementJoystick.InputVector.y;
				}else
				{
				Debug.LogError("No Movement Joystick set in PlayerMovementScript!");	
				}
			}
			#endif
			horizontalMovement = new Vector2 (rgbd.velocity.x, rgbd.velocity.z);
			if (horizontalMovement.sqrMagnitude > maximumSpectatorSpeed * maximumSpectatorSpeed) {
				/* If The velocity of rigidbody is greater than the maximum permissible speed, then normalize player rigidbody 
				 * and then multiply it by maximum walking speed to
				 * limit velocity of player to maximum walk speed
				*/
				horizontalMovement.Normalize ();
				horizontalMovement *= maximumSpectatorSpeed;
			}
			rgbdx = horizontalMovement.x;
			rgbdy = rgbd.velocity.y;
			rgbdz = horizontalMovement.y;
			//Adding Friction Effect to x and z velocity
			rgbdx = Mathf.SmoothDamp (rgbdx, 0f, ref retardationxV, spectatorRetardation);
			rgbdy = Mathf.SmoothDamp (rgbdy, 0f, ref retardationyV, spectatorRetardation);
			rgbdz = Mathf.SmoothDamp (rgbdz, 0f, ref retardationzV, spectatorRetardation);
			rgbd.velocity = new Vector3 (rgbdx, rgbdy, rgbdz);

			// Rotate the player in Y axis by the currentYRotation of fps Camera
			transform.rotation = Quaternion.Euler (0f, fpsCamera.GetComponent<AFPC_Cam> ().CurrentyRotation, 0f);
		}
	}

	void PlayerStateChecker()
	{
		// Assigns the playerState according to the boolean Variables
		if (isSwimming)
			playerState = PlayerStates.Swim_2;
		if (isCrouching && !isSwimming)
			playerState = PlayerStates.Crouch_1;
		if (canClimb)
			playerState = PlayerStates.Climb_3;
		if (!canClimb && !isCrouching && !isSwimming)
			playerState = PlayerStates.Standing_0;
	}

	void PlayClimbingSound()
	{
		if (canClimb) 
		{
			n = Random.Range (0, climbingSounds.Length - 1);
			_audioSrc.PlayOneShot (climbingSounds [n]);
		}
	}

	float DesiredAcceleration(Vector2 input)
	{
		float desiredAcceleration = 0f;

		if (isRunning && isGrounded) {
			if (!runWithoutAxisInput) {
				if (input.y > 0)
					desiredAcceleration = runAccelerationForward;
				else
					desiredAcceleration = runAccelerationBackward;
				if (input.x > 0 || input.x < 0)
					desiredAcceleration = runStrafeAcceleration;
			} else {
				/* If Running Is Enabled Without Axis Input, Then we will only move in the camera forward direction, 
				 * so set the desired acceleration to runAccelerationForward if run Button Is Pressed
				*/
				if (runKeyHold)
					desiredAcceleration = runAccelerationForward;
				else
					desiredAcceleration = 0f;
			}
		} else if (!isRunning && isGrounded && !isCrouching) {
			if (input.y > 0)
				desiredAcceleration = walkAccelerationForward;
			else
				desiredAcceleration = walkAccelerationBackward;

			if (input.x > 0 || input.x < 0)
				desiredAcceleration = walkStrafeAcceleration;
		} else if (isGrounded && isCrouching) {
			if (input.y > 0)
				desiredAcceleration = crouchAccelerationForward;
			else
				desiredAcceleration = crouchAccelerationBackward;

			if (input.x > 0 || input.x < 0)
				desiredAcceleration = crouchStrafeAcceleration;
		} else if (!isGrounded && AirControl ) {
			desiredAcceleration = airControlAcceleration;
		}

		return desiredAcceleration;
	}

	void InputManagement()
	{
		#if UNITY_STANDALONE || UNITY_WEBGL
		if (playerType == PlayerType.rigidBodyPlayer) {

			//Tilt Upcoming
//			if(allowPlayerTilting)
//			{
//				leftTiltPressed = Input.GetKey(leftTiltKey);
//				rightTiltPressed = Input.GetKey(rightTiltKey);
//			}

			if(Input.GetKeyDown(crouchKey) && !crouchButtonDown)
				crouchButtonDown = true;

			if (playerState != PlayerStates.Swim_2) {
				if (canClimb) {
					climbKeyHold = Input.GetKey (climbKey);
					if (useMouseScrollForClimbing)
						mouseScrollDeltaY = Input.mouseScrollDelta.y;
				}

				if (Input.GetButtonDown ("Jump") && !jumpPressed && isGrounded)
					jumpPressed = true;
				runKeyHold = Input.GetKey (runKey);
			} else if (playerState == PlayerStates.Swim_2) {
				swimKeyHold = Input.GetKey (swimForwardKey);
				if(Input.GetKey(goDownKey) && !downHold)
					downHold = true;
				if (decreaseOxygen) {
					if (Input.GetButton ("Jump") && !jumpPressed)
						jumpPressed = true;
				} else {
					if (Input.GetButtonDown ("Jump") && !jumpPressed)
						jumpPressed = true;
				}
			}

		} else if (playerType == PlayerType.spectator) {
			spectatorKeyHold = Input.GetKey (spectatorForwardKey);
			if (Input.GetKey(spectatorUpKey) && !jumpPressed)
				jumpPressed = true;
			if(Input.GetKey(spectatorDownKey) && !downHold)
				downHold = true;
		}
		#endif
			
		#if UNITY_ANDROID || UNITY_IOS
		if (playerType == PlayerType.rigidBodyPlayer) {

//			if(allowPlayerTilting)
//			{
//				if(leftTiltVButton != null)
//				{
//					leftTiltVButton.buttonType = AFPC_VirtualButton.ButtonType.HoldButton;
//					leftTiltPressed = leftTiltVButton.value;
//				}
//				if(rightTiltVButton != null)
//				{
//					rightTiltVButton.buttonType = AFPC_VirtualButton.ButtonType.HoldButton;
//					rightTiltPressed = rightTiltVButton.value;
//				}
//			}
			if(crouchButton != null)
			{
				crouchButton.buttonType = AFPC_VirtualButton.ButtonType.LateTriggerButton;
				if(!crouchButtonDown && crouchButton.value)
					crouchButtonDown = true;
			}

			if (canClimb) {
				if(movementJoystick != null)
				{
					if(movementJoystick.InputVector.y > 0)
						climbKeyHold = true;
					else 
						climbKeyHold = false;
				}
			}

			if (playerState != PlayerStates.Swim_2) {
			if(goDownButton != null)
			goDownButton.gameObject.SetActive(false);

			if(jumpButton !=null)
			{
					jumpButton.buttonType = AFPC_VirtualButton.ButtonType.LateTriggerButton;

			if (jumpButton.value && !jumpPressed && isGrounded)
			{
			jumpPressed = true;
			}
			}

			if(runButton != null)
				{
					runButton.buttonType = AFPC_VirtualButton.ButtonType.HoldButton;
					runKeyHold = runButton.value;
				}
			} else if (playerState == PlayerStates.Swim_2) {
			if(goDownButton != null)
			{
			goDownButton.gameObject.SetActive(true);
			goDownButton.buttonType = AFPC_VirtualButton.ButtonType.HoldButton;
			if(goDownButton.value && !downHold)
			downHold = true;
			}
			if (decreaseOxygen) {
			if(jumpButton != null)
			{
			jumpButton.buttonType = AFPC_VirtualButton.ButtonType.HoldButton;
			if (jumpButton.value && !jumpPressed)
			jumpPressed = true;
			}
			} else {
			if(jumpButton != null)
			{
			jumpButton.buttonType = AFPC_VirtualButton.ButtonType.LateTriggerButton;
			if (jumpButton.value && !jumpPressed)
			jumpPressed = true;
			}
			}
			}

			} else if (playerType == PlayerType.spectator) {

				if (spectatorForwardButton != null) 
				{
						spectatorForwardButton.buttonType = AFPC_VirtualButton.ButtonType.HoldButton;
						spectatorKeyHold = spectatorForwardButton.value;
				}
				if(jumpButton != null)
				{
					jumpButton.buttonType = AFPC_VirtualButton.ButtonType.HoldButton;
					if (jumpButton.value && !jumpPressed)
						jumpPressed = true;
				}

			if(spectatorDownButton != null)
			{
				spectatorDownButton.buttonType = AFPC_VirtualButton.ButtonType.HoldButton;
				if(spectatorDownButton.value && !downHold)
					downHold = true;
			}
			}
		#endif
	}

	private void ProgressStepCycle(float speed)
	{
		if (rgbd.velocity.sqrMagnitude > 0 && ((inputX != 0 || inputY != 0) || (isRunning && runWithoutAxisInput)))
		{
			stepCycle += (rgbd.velocity.magnitude + (speed*(!isRunning ? 1f : runningFootstepDelayFactor)))*Time.fixedDeltaTime;
		}

		if (!(stepCycle > nextStep))
		{
			return;
		}
			
		if (!isCrouching)
			nextStep = stepCycle + stepInterval;
		else
			nextStep = stepCycle + (stepInterval - 3f);
		
		if(!(_audioSrc.clip == landingSound && _audioSrc.isPlaying))
			PlayFootStepSound();
	}
		
	private void PlayFootStepSound()
	{
		if (!isGrounded && !(rgbd.velocity.sqrMagnitude > 0.1))
			return;
		//Play footstep audio file
		if (footstepsDetectionMode == FootstepsDetectionMode.basedOnTags && footstepManagerTag != null)
			footstepManagerTag.PlayFootStepSound ();
		else if (footstepsDetectionMode == FootstepsDetectionMode.basedOnTextures && footstepManagerTexture != null)
			footstepManagerTexture.PlayFootStepSound ();
	}

	float SlopeMultiplier()
	{
		float angle = Vector3.Angle(Vector3.up, _groundNormal);
		return slopeCurve.Evaluate (angle);
	}

	void FixedUpdate()
	{
		if (playerType == PlayerType.rigidBodyPlayer && !spawnManager.HasDied) {

			GroundCheck();

			if (isGrounded && isJumping) {
				isJumping = false; // If We are grounded and jumpiung, set jumping to false because we landed on grounded
			}
			if (isGrounded && !isJumping && !previouslyGrounded) {
				// Set the Landing Sound
				if (footstepsDetectionMode == FootstepsDetectionMode.basedOnTextures)
					footstepManagerTexture.SetLandingSound ();
				else if(footstepsDetectionMode == FootstepsDetectionMode.basedOnTags)
					footstepManagerTag.SetLandingSound ();
			}

			if(rgbd.velocity.sqrMagnitude < 1 && (inputY != 0 || inputX != 0) && playerDynamicFriction > 0f && !isGrounded && !isSwimming && !canClimb)
				rgbd.AddForce(0, -2f, 0f, ForceMode.VelocityChange);	// To Make sure player does not stick to a wall when having input

			CalcGroundNormal ();
			if (!canClimb) {
				rgbd.constraints = initialRigidbodyConstraints;	//Reseting the constraints to initial constraints
				useFallMultiplier = initUseFallMultiplier;
				fallMultiplier = initFallMultiplier;
				if (playerMaterial != null) {
					playerMaterial.dynamicFriction = initPMDF;
					playerMaterial.staticFriction = initPMSF;
				} else {
					Debug.LogError ("Player Physics Material not assinged in Player Movement Script!");
				}
			}

			if (canClimb && CANCLIMB) {
				if (playerMaterial != null) {
					playerMaterial.dynamicFriction = 0f;
					playerMaterial.staticFriction = 0f;
				} else {
					Debug.LogError ("Player Physics Material not assinged in Player Movement Script!");
				}
				if (useMouseScrollForClimbing && climbKeyHold) {

					useFallMultiplier = true;
					fallMultiplier = 0f;

					#if UNITY_STANDALONE || UNITY_WEBGL
					inputY1 = mouseScrollDeltaY * climbAcceleration * 10f;
					#endif
					cam.doClimbBob = true;
					rgbd.constraints = initialRigidbodyConstraints;	//Reseting the constraints to initial constraints
					if (useClimbingSound && climbingSounds != null && !climbingSoundCoroutineFlag && rgbd.velocity.sqrMagnitude > 2f) {
						climbingSoundCoroutineFlag = true;
						InvokeRepeating ("PlayClimbingSound", 0.01f, climbingSounds [n].length);
					}
				} else if (!useMouseScrollForClimbing && climbKeyHold) {

					useFallMultiplier = true;
					fallMultiplier = 0f;

					float Yfactor = 0f;
					#if UNITY_STANDALONE || UNITY_WEBGL
					Yfactor = fpsCamera.transform.forward.y; // The acceleration depends on the camera forward direction
					if(Yfactor < 0.4f && Yfactor > -0.3f)
						Yfactor = 0.8f;
					if(Yfactor < 0)
						Yfactor += 0.25f;
					#endif
					#if UNITY_ANDROID || UNITY_IOS
					Yfactor = fpsCamera.transform.forward.y * movementJoystick.InputVector.y; // The acceleration depends on the camera forward direction & the joystick y input
					if(Yfactor < 0.4f && Yfactor > -0.3f)
						Yfactor = 0.8f;
					if(Yfactor < 0)
						Yfactor += 0.25f;
					#endif
					inputY1 = Yfactor * climbAcceleration;
					cam.doClimbBob = true;
					rgbd.constraints = initialRigidbodyConstraints;	//Reseting the constraints to initial constraints
					if (useClimbingSound && climbingSounds != null && !climbingSoundCoroutineFlag && rgbd.velocity.sqrMagnitude > 2f) {
						climbingSoundCoroutineFlag = true;
						if (climbingSounds [n] != null) 
						{
							if (!IsInvoking ("PlayClimbingSound"))
								InvokeRepeating ("PlayClimbingSound", 0.01f, climbingSounds [n].length);
							else
								CancelInvoke ("PlayClimbingSound");
						}
					}
				} else {
					useFallMultiplier = false;
					climbingSoundCoroutineFlag = false;
					CancelInvoke ("PlayClimbingSound");
					inputY1 = 0f;
					cam.doClimbBob = false;
					rgbd.constraints = RigidbodyConstraints.FreezePositionY | initialRigidbodyConstraints;	// When there is no input, then freeze the y position so player doesn't go down
				}
				rgbd.AddRelativeForce (inputX * climbAcceleration, inputY1, inputY * climbAcceleration, ForceMode.Acceleration);
			}

			if (playerState != PlayerStates.Swim_2) {
				if (!isSwimming) {
					//Cancel all the oxygenManager invokes here also
					oxygenManager.CancelInvoke ("IncreaseOxygen");
					increaseOxygenInvokeFlag = false;
					decreaseOxygenInvokeFlag = false;
					oxygenManager.CancelInvoke ("DecreaseOxygen");
					oxygenManager.currentOxygen = oxygenManager.maxOxygen;
					rgbd.useGravity = true;
				}

				if (!canClimb && (allowSliding && isSliding) && !isJumping && isGrounded && (inputX < float.Epsilon || inputY < float.Epsilon)) {
					rgbd.AddForce (0f, -slideAcceleration * SlopeMultiplier (), 0f, ForceMode.VelocityChange);	//Add Slide Force To The Player Rigidbody
				}

				if (((inputY != 0 || inputX != 0) || (isRunning && runWithoutAxisInput)) && !canClimb && isGrounded && CANMOVE) {
					desiredMove = fpsCamera.transform.forward * inputY + fpsCamera.transform.right * inputX;

					if (!isJumping)
						desiredMove = Vector3.ProjectOnPlane (desiredMove, _groundNormal).normalized;
					
					if (!isRunning) {
						desiredMove.x = desiredMove.x * currentAcceleration;
						desiredMove.z = desiredMove.z * currentAcceleration;
						desiredMove.y = desiredMove.y * currentAcceleration;
					} else if (isRunning && runWithoutAxisInput) {
						desiredMove = fpsCamera.transform.forward;
						desiredMove.z = runAccelerationForward;
						desiredMove.x = 0f;
						desiredMove.y = desiredMove.y * runAccelerationForward;
					} else if (isRunning && !runWithoutAxisInput) {
						desiredMove.x = desiredMove.x * currentAcceleration;
						desiredMove.z = desiredMove.z * currentAcceleration;
						desiredMove.y = desiredMove.y * currentAcceleration;
					}

					if (desiredMove.y < 0 && !isJumping)
						rgbd.velocity = Vector3.ProjectOnPlane (rgbd.velocity, _groundNormal);

					if (!isJumping) {
						if (rgbd.velocity.sqrMagnitude < currentAcceleration * currentAcceleration && !runWithoutAxisInput)
							rgbd.AddForce (desiredMove * SlopeMultiplier (), ForceMode.Impulse);
						else if (rgbd.velocity.sqrMagnitude < currentAcceleration * currentAcceleration && !isRunning && runWithoutAxisInput)
							rgbd.AddForce (desiredMove * SlopeMultiplier (), ForceMode.Impulse);
						else if (rgbd.velocity.sqrMagnitude < currentAcceleration * currentAcceleration && isRunning && runWithoutAxisInput)
							rgbd.AddRelativeForce (desiredMove * SlopeMultiplier (), ForceMode.Impulse);
					} 
					if (isGrounded && rgbd.velocity.sqrMagnitude > 0.1)
						ProgressStepCycle (currentAcceleration);
				}


				if (isJumping && CANJUMP) {
					if (AirControl)
						rgbd.AddRelativeForce (inputX * airControlAcceleration, 0f, inputY * airControlAcceleration, ForceMode.Acceleration);
				}

				if (isGrounded && !canClimb)
					rgbd.velocity = Vector3.ProjectOnPlane (rgbd.velocity, _groundNormal);

				if (isGrounded) {
					if (stopImmediately) {
						if (!canClimb) {
							if (allowSliding) {
								if (!isSliding) {
									if (isRunning) {
										if (runWithoutAxisInput) {
											if (!isJumping && !runKeyHold && rgbd.velocity.sqrMagnitude < 1) {
												rgbd.Sleep ();
											}
										} else {
											if (!isJumping && (inputX == 0 && inputY == 0) && rgbd.velocity.sqrMagnitude < 1) {
												rgbd.Sleep ();
											}
										}
									} else {
										if (!isJumping && (inputX == 0 && inputY == 0) && rgbd.velocity.sqrMagnitude < 1) {
											rgbd.Sleep ();
										}
									}
								}
							} else {
								if (isRunning) {
									if (runWithoutAxisInput) {
										if (!isJumping && !runKeyHold && rgbd.velocity.sqrMagnitude < 1) {
											rgbd.Sleep ();
										}
									} else {
										if (!isJumping && (inputX == 0 && inputY == 0) && rgbd.velocity.sqrMagnitude < 1) {
											rgbd.Sleep ();
										}
									}
								} else {
									if (!isJumping && (inputX == 0 && inputY == 0) && rgbd.velocity.sqrMagnitude < 1) {
										rgbd.Sleep ();
									}
								}
							}
						}
					}
				} else {
					
					if (!canClimb && !isJumping && previouslyGrounded)
						StickToGroundHelper ();
				}

				if (infiniteRunning) {
					if (runKeyHold && isGrounded && ((inputX != 0 || inputY != 0) || runWithoutAxisInput) && !canClimb) {
						//Running
						isRunning = true;
					} else {
						isRunning = false;
					}
				} else if (staminaManager.currentStamina > staminaManager.minStamina && !infiniteRunning) {
					if (runKeyHold && isGrounded && ((inputX != 0 || inputY != 0) || runWithoutAxisInput) && !canClimb && (rgbd.velocity.sqrMagnitude > 1 || runWithoutAxisInput)) {
						//Running
						isRunning = true;

						if (!decreaseStaminaInvokeFlag) {
							decreaseStaminaInvokeFlag = true;
							staminaManager.InvokeRepeating ("DecreaseStamina", staminaManager.staminaDecreaseTime, staminaManager.staminaDecreaseTime);
						}
						staminaManager.CancelInvoke ("IncreaseStamina");
						increaseStaminaInvokeFlag = false;
					} else {
						staminaManager.CancelInvoke ("DecreaseStamina");
						decreaseStaminaInvokeFlag = false;
						isRunning = false;
						if (!increaseStaminaInvokeFlag) {
							increaseStaminaInvokeFlag = true;
							staminaManager.InvokeRepeating ("IncreaseStamina", staminaManager.staminaIncreaseTime, staminaManager.staminaIncreaseTime);
						}
					}
				} else if (staminaManager.currentStamina <= staminaManager.minStamina && !infiniteRunning) {
					if (runKeyHold && isGrounded && ((inputX != 0 || inputY != 0) || runWithoutAxisInput) && !canClimb && (rgbd.velocity.sqrMagnitude > 1 || runWithoutAxisInput)) {
						isRunning = false;
						staminaManager.CancelInvoke ("IncreaseStamina");
						increaseStaminaInvokeFlag = false;
					} else {
						isRunning = false;
						if (!increaseStaminaInvokeFlag) {
							increaseStaminaInvokeFlag = true;
							staminaManager.InvokeRepeating ("IncreaseStamina", staminaManager.staminaIncreaseTime, staminaManager.staminaIncreaseTime);
						}
					}
				}

				if (jumpPressed && isGrounded && !isCrouching && !canClimb && CANJUMP) {
					//Jumping
					if (jumpUpSound != null) {
						_audioSrc.PlayOneShot (jumpUpSound);
					}
					isJumping = true;
					isGrounded = false;
					rgbd.drag = dragWhileJumping;
					rgbd.velocity = new Vector3 (rgbd.velocity.x, 0f, rgbd.velocity.z);
					rgbd.AddForce (new Vector3 (0f, jumpForce, 0f), ForceMode.VelocityChange);
					jumpPressed = false;
				}

				if (rgbd.velocity.y < 0 && useFallMultiplier && !isGrounded && !isCrouching) {
					rgbd.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;		
				} else if (rgbd.velocity.y > 0 && !Input.GetButton ("Jump") && jumpForceDependsOnTheTimeJumpButtonPressed && !canClimb && !isCrouching) {
					rgbd.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
				}

			} else if (playerState == PlayerStates.Swim_2) {

				if (CANSWIM) 
				{
					if (isRunning)
						isRunning = false;
					playerMaterial.dynamicFriction = 0f;
					playerMaterial.staticFriction = 0f;
					cam.CanBob = true;
					if (decreaseOxygen) {
						// Decrease Oxygen Here
						oxygenManager.CancelInvoke ("IncreaseOxygen");
						increaseOxygenInvokeFlag = false;
						if (!decreaseOxygenInvokeFlag) {
							oxygenManager.InvokeRepeating ("DecreaseOxygen", oxygenManager.oxygenDecreaseTime, oxygenManager.oxygenDecreaseTime);
							decreaseOxygenInvokeFlag = true;
						}
						rgbd.drag = swimDrag;
						rgbd.useGravity = useGravityWhileSwimming;
						#if UNITY_STANDALONE || UNITY_WEBGL
						if (swimKeyHold) {
							float Yfactor = fpsCamera.transform.forward.y; // The acceleration depends on the camera forward direction
							inputY2 = Yfactor * swimAcceleration;
						} else {
							inputY2 = 0f;
						}
						#endif
						rgbd.AddRelativeForce (inputX * swimAcceleration, inputY2, inputY * swimAcceleration, ForceMode.Acceleration);
						if (jumpPressed) {
							rgbd.AddForce (new Vector3 (0f, jumpUpThrustForceUnderWater, 0f), ForceMode.Acceleration);
							jumpPressed = false;
						}
					} else {
						// Restore oxygen here
						decreaseOxygenInvokeFlag = false;
						oxygenManager.CancelInvoke ("DecreaseOxygen");
						if (!increaseOxygenInvokeFlag) {
							oxygenManager.InvokeRepeating ("IncreaseOxygen", oxygenManager.oxygenIncreaseTime, oxygenManager.oxygenIncreaseTime);
							increaseOxygenInvokeFlag = true;
						}
						if (jumpPressed)
							rgbd.velocity = Vector3.zero;
						float Yfactor = 0f;
						rgbd.drag = 5f;
						rgbd.useGravity = false;
						#if UNITY_STANDALONE || UNITY_WEBGL
						if (swimKeyHold) {
							Yfactor = fpsCamera.transform.forward.y; // The acceleration depends on the camera forward direction
							inputY2 = Yfactor * swimAcceleration;
						} else {
							inputY2 = 0f;
						}
						#endif

						if (Yfactor < -0.8f/*We Can't go upwards here so only apply y force when we are looking downwards while holding swimForwardKey*/) {
							rgbd.AddRelativeForce (inputX * swimAcceleration, inputY2, inputY * swimAcceleration, ForceMode.Acceleration);
						} else {
							rgbd.AddRelativeForce (inputX * swimAcceleration, 0f, inputY * swimAcceleration, ForceMode.Acceleration);
						}
						if (jumpPressed/* && canJumpOnWaterSurface*/) {
							rgbd.AddForce (new Vector3 (0f, jumpUpThrustForceOnWaterSurface, 0f), ForceMode.VelocityChange);
							jumpPressed = false;
						}
					}
					if (downHold) {
						rgbd.AddForce (new Vector3 (0f, -goDownForceWhileSwimming, 0f), ForceMode.Acceleration);
						downHold = false;
					}
				}
			}

		} else if (playerType == PlayerType.spectator)
		{
			rgbd.useGravity = useGravityInSpectatorMode;
			rgbd.drag = spectatorModeDrag;

			#if UNITY_ANDROID || UNITY_IOS
			if (!useJoystickInSpectatorMode) 
			{
				if (spectatorKeyHold) {
				Vector3 Yfactor = fpsCamera.transform.forward; // The acceleration depends on the camera forward direction
					spectatorInput = Yfactor * spectatorModeAcceleration;
				} else {
					spectatorInput = Vector3.zero;
				}
			}

			if(!useJoystickInSpectatorMode)
			rgbd.AddForce (spectatorInput, ForceMode.Acceleration);
			else
			rgbd.AddRelativeForce(inputX * spectatorModeAcceleration, 0f, inputY * spectatorModeAcceleration, ForceMode.Acceleration); 
			#endif

			#if UNITY_STANDALONE || UNITY_WEBGL
			if (spectatorKeyHold) {
				Vector3 Yfactor = fpsCamera.transform.forward; // The acceleration depends on the camera forward direction
				spectatorInput = Yfactor * spectatorModeAcceleration;
			} else {
				spectatorInput = Vector3.zero;
			}

			rgbd.AddForce (spectatorInput, ForceMode.Acceleration);
			#endif

			if (jumpPressed) {
				rgbd.AddForce (new Vector3 (0f, jumpForceInSpectatorMode, 0f), ForceMode.Acceleration);
				jumpPressed = false;
			}
			if (downHold) {
				rgbd.AddForce (new Vector3 (0f, -jumpForceInSpectatorMode, 0f), ForceMode.Acceleration);
				downHold = false;
			}
		}
	}

	void StickToGroundHelper()
	{
		float distanceFromGround =	transform.position.y - (playerCapsule.height * transform.localScale.y / 2f) + (playerCapsule.radius * (1.0f - shellOffset));
		Vector3 origin = new Vector3 (transform.position.x, distanceFromGround, transform.position.z);
		RaycastHit hit = new RaycastHit ();
		if (Physics.SphereCast (origin, playerCapsule.radius * (1.0f - shellOffset), Vector3.down, out hit, stickToGroundHelperDistance, layerMask, QueryTriggerInteraction.Ignore)) 
		{
			if(Mathf.Abs(Vector3.Angle(hit.normal, Vector3.up)) < 85f)
			{
				rgbd.velocity = Vector3.ProjectOnPlane (rgbd.velocity, hit.normal);
			}
		}
	}

	void CalcGroundNormal()
	{
		float distanceFromGround =	transform.position.y - (playerCapsule.height * transform.localScale.y / 2f) + (playerCapsule.radius * (1.0f - shellOffset));
		Vector3 origin = new Vector3 (transform.position.x, distanceFromGround, transform.position.z);
		RaycastHit hit = new RaycastHit ();
		if (Physics.SphereCast (origin, playerCapsule.radius * (1.0f - shellOffset), Vector3.down, out hit, groundCheckDistance, layerMask, QueryTriggerInteraction.Ignore)) {
			if (Mathf.Abs (Vector3.Angle (hit.normal, Vector3.up)) < maxSlope)
				_groundNormal = hit.normal;
			else
				_groundNormal = Vector3.up;
		} else {
			_groundNormal = Vector3.up;
		}
	}

	void OnDrawGizmos()
	{
		if (visualize)
		{
			//For GroundCheck1 Sphere
			float distanceFromGround =	transform.position.y - (playerCapsule.height * transform.localScale.y / 2f) + (playerCapsule.radius * (1.0f - shellOffset));
			Vector3 origin = new Vector3 (transform.position.x, distanceFromGround, transform.position.z);
			Vector3 desiredOrigin = origin + new Vector3 (0f, -groundCheckDistance, 0f);
			float Radius = playerCapsule.radius * (1.0f - shellOffset);
			Gizmos.DrawWireSphere (desiredOrigin, Radius);

			//For StickToGroundHelperSphere
			Vector3 desiredOrigin1 = origin + new Vector3 (0f, -stickToGroundHelperDistance, 0f);
			float Radius1 = playerCapsule.radius * (1.0f - shellOffset);
			Gizmos.DrawWireSphere (desiredOrigin1, Radius1);
		}
	}


	void OnCollisionStay(Collision coll)
	{
		
		foreach (ContactPoint contact in coll.contacts)
		{
			if (Mathf.Abs (Vector3.Angle (contact.normal, Vector3.up)) < maxSlope)
				groundCheck2 = true;
			
			if (groundCheck2 && (Mathf.Abs (Vector3.Angle (contact.normal, Vector3.up)) > slideSlope)
			    && allowSliding
			    && (inputY < float.Epsilon || inputX < float.Epsilon))
					isSliding = true;
		}
		
	}



	void OnCollisionExit()
	{
		groundCheck2 = false;
		if(allowSliding)
			isSliding = false;
	}
}