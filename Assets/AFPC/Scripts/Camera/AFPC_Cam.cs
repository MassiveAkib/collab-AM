using System.Collections;
using UnityEngine;

public class AFPC_Cam : MonoBehaviour {

	public float xlookSenstivity = 3f;
	public bool CANROTATE = true;
	public float ylookSenstivity = 3f;
	public bool clampXRotation = true;
	public float minimumXRotation = -90f;
	public float maximumXRotation = 90f;
	public bool smoothRotation = true;
	public bool invertMouse = false;
	public bool useSlerpCam = true;	//If enabled, then camera becomes more smooth and has jerk when mouse movement is high
	public float smoothTime = 4f;
	public bool useUnderwaterImageEffects = true;
	public AFPC_TouchPad cameraTouchPad;
	[Tooltip("If it is enabled, then the camera will rotate slightly towards positive or negative x axis depending upon then values of Horizontal Input Axis.")]
	public bool tiltCamSlightly = true;
	[Tooltip("Rotates the camera if rotateCamSlightly is enabled in the desired value")]
	public float angleToTilt = 2f;
	public float timeToTiltCamSlightly = 0.4f;

	[Header("Head Bob")]
	public float bobSpeed = 1f;
	public float maxHorizontalBob = 0.08f, maxHorizontalBobWhileRunning = 0.2f;
	public float maxVerticalBob = 0.05f, maxVerticalBobWhileRunning = 0.1f;
	public float eyeHeightRatio = 0.9f; // the ratio of the height of the cam to the height of the player
	private Vector3 parentLastPostion; //the last position of the player
	private float bobStepCounter = 0f;


	[Header("Climb Bob")]
	public AnimationCurve climbBobCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
															new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
															new Keyframe(2f, 0f));	// Sin Curve for Climb Bob
	public float climbBobSpeed = 1f;
	public bool allowClimbBobing = true;
	public float verticalToHorizontalClimbBobRatio = 2f;
	public float maxHorizontalClimbBob = 0.16f;
	public float maxVerticalClimbBob = 0.2f;
	public float climbBobCycleInterval = 5f;
	public float timeToSmoothClimbBob = 0.2f;
	[HideInInspector]
	public bool doClimbBob = false; // if enabled then performs climb bob


	[Header("Jump Bob")]
	public float jumpBobDuration = 0.15f;
	public float jumpBobAmount = 0.2f;
	[Tooltip("If it is true then, the jump kick back is done based on animation, not on physics calculations")]
	public bool useJumpKickBackAnimation = true;
	public GameObject jumpkickBack;

	[Header("References")]
	public AFPC_PlayerMovement afpcPlayer; // The Player



	private AudioSource _audioSrc;
	private Animator jumpKickBackAnimationController;
	private float offset = 0f;
	[HideInInspector]
	public bool previouslyGrounded; //Checks whether if player was grounded or not 
	private int jumpKickBackTriggerHashId;

	//Private Variables for climb bob
	private float ClimbBobCurveTime; // Length of the climbbob curve
	private Vector3 cameraStartPos;	// starting local position of the fpsCamera
	private Vector3 cameraPosBeforeClimb; // The local position of the fpsCamera before entering the climbing State
	private float curveXPos = 0f, curveYPos = 0f;
	private AFPC_SpawnManager spawnManager;
	#region Variables Used For SmoothDamp Method
	private Vector3 jumpbobv, climbBobV;
	private float xRotation, xRotationV;
	private float yRotation, yRotationV;
	private float zRotation = 0f, zRotationV;
	#endregion

	private	bool doJumpBob = false;

	private bool canBob = true; // Determines if the bob takes action, useful when we a colliding a wall and bob still occurs, it prevents us from that situation
	public bool CanBob
	{
		set{ canBob = value;}
		get{ return canBob;}
	}

	private float currentxRotation;
	public float CurrentxRotation
	{
		get{ return currentxRotation;}
		set{ currentxRotation = value;}
	}
	private float currentyRotation;
	public float CurrentyRotation
	{
		get{ return currentyRotation;}
		set{ currentyRotation = value;}
	}

	private Quaternion cameraOriginalRotation, cameraTargetRotation;

	void Awake()
	{
		parentLastPostion = transform.parent.position; 
	}

	// Use this for initialization
	void Start () 
	{
		cameraOriginalRotation = transform.rotation;
		cameraStartPos = transform.localPosition;
		ClimbBobCurveTime = climbBobCurve [climbBobCurve.length - 1].time;
		if (afpcPlayer == null)
			afpcPlayer = GameObject.FindObjectOfType<AFPC_PlayerMovement> ();
		if (afpcPlayer != null)
		{
			spawnManager = afpcPlayer.gameObject.GetComponent<AFPC_SpawnManager> ();
			_audioSrc = afpcPlayer.GetComponent<AudioSource> ();
		}
		if (jumpkickBack == null)
			jumpkickBack = transform.parent.gameObject;
		jumpKickBackAnimationController = jumpkickBack.GetComponent<Animator> ();
		jumpKickBackTriggerHashId = Animator.StringToHash ("DoJumpKickBack");
	}
		
	IEnumerator JumpBob()
	{
		//Camera Move down slightly
		float elapsedTime = 0f;
		while(elapsedTime < jumpBobDuration)
		{
			offset = Mathf.Lerp (0f, jumpBobAmount, elapsedTime / jumpBobDuration);
			Vector3 targetPos = new Vector3 (0f, -offset, 0f);
			transform.localPosition += targetPos; // Decreasing the localPostion of Cam By Offset in y axis only
			elapsedTime += Time.deltaTime;
			yield return new WaitForFixedUpdate ();
		}
		//Make it move back to normal
		elapsedTime = 0f;
		while (elapsedTime < jumpBobDuration) 
		{
			offset = 0f;
			offset = Mathf.Lerp (0f, jumpBobAmount, elapsedTime / jumpBobDuration);
			Vector3 targetPos = new Vector3 (0f, offset, 0f);
			transform.localPosition += targetPos; // Increasing the localPostion of Cam By Offset in y axis only
			elapsedTime += Time.deltaTime;
			yield return new WaitForFixedUpdate ();
		}
		doJumpBob = false;
	}

	void JumpKickBack()
	{
		jumpKickBackAnimationController.SetTrigger(jumpKickBackTriggerHashId);
		doJumpBob = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (!spawnManager.HasDied && CANROTATE) 
		{
			//avoids the mouse looking if the game is effectively paused
			if (Mathf.Abs (Time.timeScale) < float.Epsilon)
				return;

			if (!invertMouse) {
				#if UNITY_STANDALONE || UNITY_WEBGL
				xRotation -= Input.GetAxis ("Mouse Y") * ylookSenstivity;
				yRotation += Input.GetAxis ("Mouse X") * xlookSenstivity;
				#endif

				#if UNITY_ANDROID || UNITY_IOS
				if(cameraTouchPad != null)
				{
					xRotation -= cameraTouchPad.InputVector.y * ylookSenstivity;
					yRotation += cameraTouchPad.InputVector.x * xlookSenstivity;
				}else
				{
					Debug.LogError("Reference to Camera TouchPad not Set in camera Script!!");
				}
				#endif
			} else {
				#if UNITY_STANDALONE || UNITY_WEBGL
				xRotation += Input.GetAxis ("Mouse Y") * ylookSenstivity;
				yRotation -= Input.GetAxis ("Mouse X") * xlookSenstivity; 
				#endif

				#if UNITY_ANDROID || UNITY_IOS
				if(cameraTouchPad != null)
				{
					xRotation += cameraTouchPad.InputVector.y * ylookSenstivity;
					yRotation -= cameraTouchPad.InputVector.x * xlookSenstivity;
				}else
				{
					Debug.LogError("Reference to Camera TouchPad not Set in camera Script!!");
				}
				#endif
			}

			if (clampXRotation) {
				xRotation = Mathf.Clamp (xRotation, minimumXRotation, maximumXRotation);
			}
			if (smoothRotation) {
				currentxRotation = Mathf.SmoothDamp (currentxRotation, xRotation, ref xRotationV, smoothTime);
				currentyRotation = Mathf.SmoothDamp (currentyRotation, yRotation, ref yRotationV, smoothTime);
			} else {
				currentxRotation = xRotation;
				currentyRotation = yRotation;
			}

			if (useSlerpCam) {
				cameraOriginalRotation = transform.rotation;

				cameraTargetRotation = Quaternion.Euler (currentxRotation, currentyRotation, zRotation);

				/* Tilt Upcoming
				if(!afpcPlayer.allowPlayerTilting)
					cameraTargetRotation = Quaternion.Euler (currentxRotation, currentyRotation, zRotation);
				else if(afpcPlayer.allowPlayerTilting && !afpcPlayer.IsTilting)
					cameraTargetRotation = Quaternion.Euler (currentxRotation, currentyRotation, zRotation);
				else if(afpcPlayer.allowPlayerTilting && afpcPlayer.IsTilting)
					cameraTargetRotation = Quaternion.Euler (currentxRotation, currentyRotation, afpcPlayer.zRotation/2f);
				*/
				transform.rotation = Quaternion.Slerp (cameraOriginalRotation, cameraTargetRotation, Time.deltaTime / smoothTime); 
			} else {

				transform.rotation = Quaternion.Euler (currentxRotation, currentyRotation, zRotation);

				/* Tilt Upcoming
				if(!afpcPlayer.allowPlayerTilting)
					transform.rotation = Quaternion.Euler (currentxRotation, currentyRotation, zRotation);
				else if(afpcPlayer.allowPlayerTilting && !afpcPlayer.IsTilting)
					transform.rotation = Quaternion.Euler (currentxRotation, currentyRotation, zRotation);
				else if(afpcPlayer.allowPlayerTilting && afpcPlayer.IsTilting)
					transform.rotation = Quaternion.Euler (currentxRotation, currentyRotation, afpcPlayer.zRotation/2f);
				*/	
			}
		}
			
	}

	void ClimbBob()
	{
		float currentXPos = cameraStartPos.x + climbBobCurve.Evaluate (curveXPos) * maxHorizontalClimbBob;
		float currentYPos = cameraStartPos.y + climbBobCurve.Evaluate (curveYPos) * maxVerticalClimbBob;

		curveXPos += climbBobSpeed * Time.deltaTime / climbBobCycleInterval;
		curveYPos += climbBobSpeed * Time.deltaTime / climbBobCycleInterval * verticalToHorizontalClimbBobRatio;

		if (curveXPos > ClimbBobCurveTime)
			curveXPos -= ClimbBobCurveTime; // To Repeat the process
		if (curveYPos > ClimbBobCurveTime)
			curveYPos -= ClimbBobCurveTime; // To Repeat the process

		Vector3 fpsCamTargetPosition = new Vector3 (currentXPos, currentYPos, transform.localPosition.z);
		transform.localPosition = Vector3.SmoothDamp (transform.localPosition, fpsCamTargetPosition, ref climbBobV, timeToSmoothClimbBob);
	}

	void Bobbing()
	{
		if (canBob && !doJumpBob) {
			#region Motion Bob
			if ((afpcPlayer.isGrounded /*If the player on grounded*/ || afpcPlayer.playerState == AFPC_PlayerMovement.PlayerStates.Swim_2 /*or the player is swimming*/) && afpcPlayer.gameObject.GetComponent<Rigidbody> ().velocity.magnitude > 0) {
				if (!afpcPlayer.IsRunning) {
					bobStepCounter += Vector3.Distance (parentLastPostion, transform.parent.position) * bobSpeed;
					float posX, posY;
					posX = Mathf.Sin (bobStepCounter) * maxHorizontalBob;
					posY = (Mathf.Cos (bobStepCounter * 2) * maxVerticalBob * -1) + (transform.localScale.y * eyeHeightRatio) - (transform.localScale.y / 2);
					transform.localPosition = new Vector3 (posX, posY, transform.localPosition.z);
					parentLastPostion = afpcPlayer.transform.position;	
				} else {
					bobStepCounter += Vector3.Distance (parentLastPostion, transform.parent.position) * bobSpeed;
					float posX, posY;
					posX = Mathf.Sin (bobStepCounter) * maxHorizontalBobWhileRunning;
					posY = (Mathf.Cos (bobStepCounter * 2) * maxVerticalBobWhileRunning * -1) + (transform.localScale.y * eyeHeightRatio) - (transform.localScale.y / 2);
					transform.localPosition = new Vector3 (posX, posY, transform.localPosition.z);
					parentLastPostion = afpcPlayer.transform.position;	
				}
			}
			#endregion
		}

		if (doClimbBob && afpcPlayer.CanClimb && allowClimbBobing) {
			#region ClimbBob
			if (afpcPlayer.gameObject.GetComponent<Rigidbody> ().velocity.magnitude > 0) {
				ClimbBob ();
			}
			#endregion
		} else if (!doClimbBob && afpcPlayer.CanClimb) {
			transform.localPosition = Vector3.SmoothDamp (transform.localPosition, cameraPosBeforeClimb, ref climbBobV, timeToSmoothClimbBob);
		} else if (!doClimbBob && !afpcPlayer.CanClimb) {
			cameraPosBeforeClimb = transform.localPosition;
		}

		#region Jump Bob
		if (afpcPlayer.isGrounded && previouslyGrounded == false && afpcPlayer.playerState != AFPC_PlayerMovement.PlayerStates.Swim_2 && afpcPlayer.playerState != AFPC_PlayerMovement.PlayerStates.Climb_3) {
			if (!afpcPlayer.IsCrouching) {
				doJumpBob = true;
				if (afpcPlayer.LandingSound != null)
				{
					_audioSrc.clip = afpcPlayer.LandingSound;
					_audioSrc.Play();
					afpcPlayer.nextStep = afpcPlayer.stepCycle + 0.5f; 
				}
				if (useJumpKickBackAnimation)
					JumpKickBack ();
				else
					StartCoroutine (JumpBob ());
			}
		}
		#endregion

		previouslyGrounded = afpcPlayer.isGrounded;
	}

	void LateUpdate()
	{
		if(!spawnManager.HasDied)
		{
			if (afpcPlayer.playerType != AFPC_PlayerMovement.PlayerType.spectator)
				Bobbing ();
			
			if (afpcPlayer.playerType != AFPC_PlayerMovement.PlayerType.spectator && afpcPlayer.isGrounded) {
				#region CameraTilt

				float horizontalInput = afpcPlayer.inputX;

				if (tiltCamSlightly) {
					if (horizontalInput > 0) {
						zRotation = Mathf.SmoothDamp (zRotation, zRotation + angleToTilt, ref zRotationV, timeToTiltCamSlightly);
						zRotation = Mathf.Clamp (zRotation, 0f, angleToTilt);
					} else if (horizontalInput < 0) {
						zRotation = Mathf.SmoothDamp (zRotation, zRotation - angleToTilt, ref zRotationV, timeToTiltCamSlightly);
						zRotation = Mathf.Clamp (zRotation, -angleToTilt, 0f);
					} else { 
						zRotation = Mathf.SmoothDamp (zRotation, 0f, ref zRotationV, timeToTiltCamSlightly);
					}
				}

				#endregion
			}
		}

	}
}