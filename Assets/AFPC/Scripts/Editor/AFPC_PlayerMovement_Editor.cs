using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor.AnimatedValues;

#if UNITY_EDITOR
[CustomEditor(typeof(AFPC_PlayerMovement))]
[CanEditMultipleObjects]
public class AFPC_PlayerMovement_Editor : Editor {

	private AnimBool showExtraFields, showExtraFields1, showExtraFields2, showExtraFields3, showExtraFields4, showExtraFields5, showExtraFields6, showExtraFields7, showExtraFields8, showExtraFields9, showExtraFields10, showExtraFields11, showExtraFields12;

	private AFPC_PlayerMovement afpcPlayer;
	private float groundDist = 0f;
	private bool putOnGround = false; 
	private void OnEnable()
	{
		showExtraFields = new AnimBool (true);
		showExtraFields.valueChanged.AddListener (Repaint);
		showExtraFields1 = new AnimBool (true);
		showExtraFields1.valueChanged.AddListener (Repaint);
		showExtraFields2 = new AnimBool (true);
		showExtraFields2.valueChanged.AddListener (Repaint);
		showExtraFields3 = new AnimBool (true);
		showExtraFields3.valueChanged.AddListener (Repaint);
		showExtraFields4 = new AnimBool (true);
		showExtraFields4.valueChanged.AddListener (Repaint);
		showExtraFields5 = new AnimBool (true);
		showExtraFields5.valueChanged.AddListener (Repaint);
		showExtraFields6 = new AnimBool (true);
		showExtraFields6.valueChanged.AddListener (Repaint);
		showExtraFields7 = new AnimBool (true);
		showExtraFields7.valueChanged.AddListener (Repaint);
		showExtraFields8 = new AnimBool (true);
		showExtraFields8.valueChanged.AddListener (Repaint);
		showExtraFields9 = new AnimBool (true);
		showExtraFields9.valueChanged.AddListener (Repaint);
		showExtraFields10 = new AnimBool (true);
		showExtraFields10.valueChanged.AddListener (Repaint);
		showExtraFields11 = new AnimBool (true);
		showExtraFields11.valueChanged.AddListener (Repaint);
		showExtraFields12 = new AnimBool (true);
		showExtraFields12.valueChanged.AddListener (Repaint);
	}
	public override void OnInspectorGUI()
	{

		afpcPlayer = (AFPC_PlayerMovement)target;

		EditorGUI.BeginChangeCheck ();
	
		EditorGUILayout.BeginVertical ("NotificationBackground");
		showExtraFields.target = EditorGUILayout.Foldout (showExtraFields.target,"Player Type And States", showExtraFields.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields.faded)) 
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			afpcPlayer.playerType = (AFPC_PlayerMovement.PlayerType)EditorGUILayout.EnumPopup ("Player Type: ", afpcPlayer.playerType, "ExposablePopupMenu");
			afpcPlayer.playerState = (AFPC_PlayerMovement.PlayerStates)EditorGUILayout.EnumPopup ("Player State: ", afpcPlayer.playerState, "ExposablePopupMenu");
			EditorGUILayout.HelpBox ("Player Type is the type of player!\nPlayerStates are automatically controlled ingame.\nPlayerState refers to the player's current state!", MessageType.Info, true);
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();

		GUILayout.Box (GUIContent.none, "horizontalSlider");

#if UNITY_STANDALONE || UNITY_WEBGL
showExtraFields1.target = EditorGUILayout.Foldout (showExtraFields1.target,"Cursor Lock", showExtraFields1.target);
if (EditorGUILayout.BeginFadeGroup (showExtraFields1.faded)) 
{
EditorGUI.indentLevel++; 
EditorGUILayout.BeginVertical ("WindowBackground");
afpcPlayer.lockCursor = (bool)EditorGUILayout.Toggle ("Lock Cursor: ", afpcPlayer.lockCursor);
afpcPlayer.wantedCursorLock = (CursorLockMode)EditorGUILayout.EnumPopup ("Cursor Lock Mode: ", afpcPlayer.wantedCursorLock, "ExposablePopupMenu");
afpcPlayer.disableCursorLock = (KeyCode)EditorGUILayout.EnumPopup ("Disable Cursor LockKey: ", afpcPlayer.disableCursorLock, "ExposablePopupMenu");
EditorGUILayout.HelpBox ("Lock Cursor controls wether the cursor should be locked or not in game!\nWantedCursorLock is the CursorLockMode Wanted on pressing left mouse button.\n DisableCursorLockKey is the key to be pressed to unlock cursor!", MessageType.Info, true);
EditorGUILayout.EndVertical ();
EditorGUI.indentLevel--;
}
EditorGUILayout.EndFadeGroup ();

GUILayout.Box (GUIContent.none, "horizontalSlider");
#endif

		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator)
		{
			showExtraFields2.target = EditorGUILayout.Foldout (showExtraFields2.target,"Spectator Player Movement Settings", showExtraFields2.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields2.faded)) 
			{
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.maximumSpectatorSpeed = (float)EditorGUILayout.FloatField ("Maximum Spectator Speed: ", afpcPlayer.maximumSpectatorSpeed);
				afpcPlayer.spectatorModeDrag = (float)EditorGUILayout.FloatField ("Spectator Mode Drag: ", afpcPlayer.spectatorModeDrag);
				afpcPlayer.spectatorModeAcceleration = EditorGUILayout.FloatField ("Spectator Mode Acceleration: ", afpcPlayer.spectatorModeAcceleration);
				afpcPlayer.jumpForceInSpectatorMode = EditorGUILayout.FloatField ("Jump Force In Spectator Mode: ", afpcPlayer.jumpForceInSpectatorMode);
				#if UNITY_STANDALONE || UNITY_WEBGL
				afpcPlayer.spectatorForwardKey = (KeyCode)EditorGUILayout.EnumPopup ("Spectator Forward Key: ", afpcPlayer.spectatorForwardKey, "ExposablePopupMenu");
				afpcPlayer.spectatorUpKey = (KeyCode)EditorGUILayout.EnumPopup ("Spectator Up Key: ", afpcPlayer.spectatorUpKey, "ExposablePopupMenu");
				afpcPlayer.spectatorDownKey = (KeyCode)EditorGUILayout.EnumPopup ("Spectator Down Key: ", afpcPlayer.spectatorDownKey, "ExposablePopupMenu");
				#endif
				afpcPlayer.spectatorRetardation = (float)EditorGUILayout.FloatField ("Spectator Retardation Amount ", afpcPlayer.spectatorRetardation);
				afpcPlayer.useGravityInSpectatorMode = (bool)EditorGUILayout.Toggle ("Use Gravity In Spectator Mode: ", afpcPlayer.useGravityInSpectatorMode);
				EditorGUILayout.HelpBox ("Maximum Spectator speed is the maximum speed spectator can have!\nSpectator Mode Drag is the amount of drag of the player rigidbody when in spectator mode.\n Spectator Retardation amount is the time after which player will be stopped, Greater is the value, more is the time taken by the player to stop!\n Use Gravity in spectator mode decides whether to use gravity or not in spectator mode.", MessageType.Info, true);
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");

		} else if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer) 
		{
			showExtraFields3.target = EditorGUILayout.Foldout (showExtraFields3.target,"Rigidbody Player Movement Settings", showExtraFields3.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields3.faded)) 
			{
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.CANMOVE = EditorGUILayout.Toggle ("Can Move: ", afpcPlayer.CANMOVE);
				if (afpcPlayer.CANMOVE) {
					afpcPlayer.maximumWalkSpeed = (float)EditorGUILayout.FloatField ("Maximum Player Speed: ", afpcPlayer.maximumSpectatorSpeed);
					afpcPlayer.walkAccelerationForward = (float)EditorGUILayout.FloatField ("Forward Walk Acceleration: ", afpcPlayer.walkAccelerationForward);
					afpcPlayer.walkAccelerationBackward = (float)EditorGUILayout.FloatField ("Backward Walk Acceleration: ", afpcPlayer.walkAccelerationBackward);
					afpcPlayer.walkStrafeAcceleration = (float)EditorGUILayout.FloatField ("Strafe Walk Acceleration: ", afpcPlayer.walkStrafeAcceleration);
					afpcPlayer.playerDrag = EditorGUILayout.FloatField ("Player Rigidbody Drag when Grounded: ", afpcPlayer.playerDrag);
					LayerMask temp = EditorGUILayout.MaskField ("Culling Mask: ", InternalEditorUtility.LayerMaskToConcatenatedLayersMask (afpcPlayer.layerMask), InternalEditorUtility.layers, "ExposablePopupMenu");
					afpcPlayer.layerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask (temp);
					afpcPlayer.runAccelerationForward = (float)EditorGUILayout.FloatField ("Forward Run Acceleration: ", afpcPlayer.runAccelerationForward);
					afpcPlayer.runAccelerationBackward = (float)EditorGUILayout.FloatField ("Backward Run Acceleration: ", afpcPlayer.runAccelerationBackward);
					afpcPlayer.runStrafeAcceleration = (float)EditorGUILayout.FloatField ("Strafe Run Acceleration: ", afpcPlayer.runStrafeAcceleration);
					afpcPlayer.slopeCurve = EditorGUILayout.CurveField ("Movement Slope Curve: ", afpcPlayer.slopeCurve);
					afpcPlayer.stopImmediately = EditorGUILayout.Toggle ("Stop Player Immediately When No Input: ", afpcPlayer.stopImmediately);

					if (!afpcPlayer.stopImmediately)
						afpcPlayer.retardationTime = (float)EditorGUILayout.FloatField ("Player Stop Time When No Input: ", afpcPlayer.retardationTime);

					afpcPlayer.maxSlope = EditorGUILayout.Slider ("Maximum Grounded Angle or Slope: ", afpcPlayer.maxSlope, 0f, 90f);
					afpcPlayer.AirControl = (bool)EditorGUILayout.Toggle ("Has Air Control: ", afpcPlayer.AirControl);
					afpcPlayer.allowSliding = EditorGUILayout.Toggle ("Allow Sliding when on steep Surface: ", afpcPlayer.allowSliding);

					if (afpcPlayer.allowSliding) {
						afpcPlayer.slideSlope = EditorGUILayout.Slider ("Minimum Slide Angle: ", afpcPlayer.slideSlope, 0f, afpcPlayer.maxSlope);
						afpcPlayer.slideAcceleration = EditorGUILayout.FloatField ("Sliding Acceleration Downward: ", afpcPlayer.slideAcceleration);
					}

					if (afpcPlayer.AirControl)
						afpcPlayer.airControlAcceleration = (float)EditorGUILayout.FloatField ("Air Acceleration: ", afpcPlayer.airControlAcceleration);
					afpcPlayer.runKey = (KeyCode)EditorGUILayout.EnumPopup ("Run Key: ", afpcPlayer.runKey, "ExposablePopupMenu");
					afpcPlayer.infiniteRunning = (bool)EditorGUILayout.Toggle ("Infinite Running: ", afpcPlayer.infiniteRunning);
					afpcPlayer.runWithoutAxisInput = EditorGUILayout.Toggle ("No Axis Input Required For Running: ", afpcPlayer.runWithoutAxisInput);
					afpcPlayer.useFOVKick = (bool)EditorGUILayout.Toggle ("Use FOV Kick When Running: ", afpcPlayer.useFOVKick);
					EditorGUILayout.HelpBox ("Culling Mask is layers on which ground check should not be done.\nMovement Slope Curve Defines the Amount of Input based Movement(y value of the curve) for the angle between the player and ground(x value of the curve).", MessageType.Info, true);
				}
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");

			if (afpcPlayer.useFOVKick) 
			{
				showExtraFields4.target = EditorGUILayout.Foldout (showExtraFields4.target, "FOV Kick", showExtraFields4.target);
				if (EditorGUILayout.BeginFadeGroup (showExtraFields4.faded)) {
					EditorGUI.indentLevel++; 
					EditorGUILayout.BeginVertical ("WindowBackground");
					afpcPlayer.runFOV = (float)EditorGUILayout.Slider ("Running FOV: ", afpcPlayer.runFOV, 1f, 179f);
					afpcPlayer.timeToIncreaseFOV = (float)EditorGUILayout.FloatField ("Time Taken to Increase FOV: ", afpcPlayer.timeToIncreaseFOV);
					afpcPlayer.timeToDecreaseFOV = (float)EditorGUILayout.FloatField ("Time Taken to Decrease FOV: ", afpcPlayer.timeToDecreaseFOV);
					EditorGUILayout.HelpBox ("Running FOV : Field of view of Camera when Running.", MessageType.Info, true);
					EditorGUILayout.EndVertical ();
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.EndFadeGroup ();

				GUILayout.Box (GUIContent.none, "horizontalSlider");
			}

			showExtraFields5.target = EditorGUILayout.Foldout (showExtraFields5.target, "Ground Checking and Sticking", showExtraFields5.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields5.faded)) {
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.shellOffset = (float)EditorGUILayout.FloatField ("Shell Offest For Ground Check: ", afpcPlayer.shellOffset);
				afpcPlayer.groundCheckDistance = EditorGUILayout.FloatField ("Ground Check Distance: ", afpcPlayer.groundCheckDistance);
				afpcPlayer.stickToGroundHelperDistance = (float)EditorGUILayout.FloatField ("Stick To Ground Helper Distance: ", afpcPlayer.stickToGroundHelperDistance);
				EditorGUILayout.HelpBox ("Prevents bouncing of player while moving down slopes\nShell Offest For Ground Check is the amount by which the radius of ground check sphere will be smaller than the Player's Capsule radius.", MessageType.Info, true);
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");

			showExtraFields6.target = EditorGUILayout.Foldout (showExtraFields6.target, "Audio Clips", showExtraFields6.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields6.faded)) {
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.audioVolume = (float)EditorGUILayout.FloatField ("Audio Effects Volume: ", afpcPlayer.audioVolume);
				afpcPlayer.jumpUpSound = (AudioClip)EditorGUILayout.ObjectField ("Jump Up Sound: ", afpcPlayer.jumpUpSound, typeof(AudioClip), false);
				if (afpcPlayer.useClimbingSound)
				{
					var serializedObj = new SerializedObject (target);
					var property = serializedObj.FindProperty ("climbingSounds");
					serializedObj.Update ();
					EditorGUILayout.PropertyField (property, true);
					serializedObj.ApplyModifiedProperties ();
				}
				EditorGUILayout.HelpBox ("All The AudioClips for jumping up, climbing\n", MessageType.Info, true);
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");

			showExtraFields7.target = EditorGUILayout.Foldout (showExtraFields7.target, "Jumping", showExtraFields7.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields7.faded)) {
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.CANJUMP = EditorGUILayout.Toggle ("Can Jump: ", afpcPlayer.CANJUMP);
				if (afpcPlayer.CANJUMP) {
					afpcPlayer.jumpForce = (float)EditorGUILayout.FloatField ("Jump Force: ", afpcPlayer.jumpForce);
					afpcPlayer.dragWhileJumping = (float)EditorGUILayout.FloatField ("Drag When Jumping: ", afpcPlayer.dragWhileJumping);
					afpcPlayer.jumpForceDependsOnTheTimeJumpButtonPressed = (bool)EditorGUILayout.Toggle ("Does Jump Force Depends on the time\njump button has been pressed: ", afpcPlayer.jumpForceDependsOnTheTimeJumpButtonPressed);
					afpcPlayer.useFallMultiplier = (bool)EditorGUILayout.Toggle ("Use Fall Multiplier When Going Down\nafter Jumping Up: ", afpcPlayer.useFallMultiplier);
					if (afpcPlayer.useFallMultiplier) {
						afpcPlayer.fallMultiplier = (float)EditorGUILayout.FloatField ("Fall Multiplier: ", afpcPlayer.fallMultiplier);
						EditorGUILayout.HelpBox ("Fall Multiplier affects the time it takes to get down, when it is 1, then the player gets down at the rate defined in Physics.gravity.y", MessageType.Info, true);
					}
					if (afpcPlayer.jumpForceDependsOnTheTimeJumpButtonPressed) {
						afpcPlayer.lowJumpMultiplier = (float)EditorGUILayout.FloatField ("Low Jump Multiplier: ", afpcPlayer.lowJumpMultiplier);
						EditorGUILayout.HelpBox ("Low Jump Multiplier affects how much we go up on pressing the jump button depending on for how much time we pressed the jump button, when it is 1, then the player goes up normally!", MessageType.Info, true);
					}
				}
				EditorGUILayout.HelpBox ("Jump Force is the amount of force applied on jump.\nDrag While jumping is the drag when player is jumping.\nUse Fall Multiplier affects the time it takes to get down after Jump.", MessageType.Info, true);
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");

			showExtraFields8.target = EditorGUILayout.Foldout (showExtraFields8.target, "Footsteps", showExtraFields8.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields8.faded)) {
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.footstepsDetectionMode = (AFPC_PlayerMovement.FootstepsDetectionMode)EditorGUILayout.EnumPopup ("FootstepsSound Detection Mode: ", afpcPlayer.footstepsDetectionMode, "ExposablePopupMenu");
				afpcPlayer.runningFootstepDelayFactor = EditorGUILayout.Slider ("Running Foostep Delay Factor: ", afpcPlayer.runningFootstepDelayFactor, 0.1f, 1f);
				afpcPlayer.stepInterval = EditorGUILayout.FloatField ("Step Interval: ", afpcPlayer.stepInterval);
				EditorGUILayout.HelpBox ("Running Footstep Delay Factor is the factor by which footstep frequency will decrease when running.\nStep Interval is the time after step cycle gets repeated or after which new step cycle starts.", MessageType.Info, true);
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");

			showExtraFields9.target = EditorGUILayout.Foldout (showExtraFields9.target, "Climbing", showExtraFields9.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields9.faded)) {
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.CANCLIMB = EditorGUILayout.Toggle ("Can Climb", afpcPlayer.CANCLIMB);
				if (afpcPlayer.CANCLIMB) {
					afpcPlayer.climbAcceleration = (float)EditorGUILayout.FloatField ("Climbing Acceleration: ", afpcPlayer.climbAcceleration);
					afpcPlayer.climbDrag = (float)EditorGUILayout.FloatField ("Drag when Climbing: ", afpcPlayer.climbDrag);
					afpcPlayer.useGravityWhileClimbing = (bool)EditorGUILayout.Toggle ("Use Gravity When Climbing: ", afpcPlayer.useGravityWhileClimbing);
					afpcPlayer.useMouseScrollForClimbing = (bool)EditorGUILayout.Toggle ("Use Mouse Scroll Delta For Climbing: ", afpcPlayer.useMouseScrollForClimbing);
					afpcPlayer.useClimbingSound = (bool)EditorGUILayout.Toggle ("Play Climbing Sounds When Climbing: ", afpcPlayer.useClimbingSound);
					afpcPlayer.climbKey = (KeyCode)EditorGUILayout.EnumPopup ("Climbing Key: ", afpcPlayer.climbKey, "ExposablePopupMenu");
					EditorGUILayout.HelpBox ("Climb Drag is the drag of the player rigidbody when climbing\n If Use Mouse Scroll Delta For Climbing is set to true, then the climbing will be done by scrolling mouse.", MessageType.Info, true);
				}
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");

			showExtraFields10.target = EditorGUILayout.Foldout (showExtraFields10.target, "Crouching", showExtraFields10.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields10.faded)) {
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.CANCROUCH = EditorGUILayout.Toggle ("Can Crouch: ", afpcPlayer.CANCROUCH);
				if (afpcPlayer.CANCROUCH) {
					afpcPlayer.crouchAccelerationForward = (float)EditorGUILayout.FloatField ("Crouching Acceleration Forward: ", afpcPlayer.crouchAccelerationForward);
					afpcPlayer.crouchAccelerationBackward = (float)EditorGUILayout.FloatField ("Crouching Acceleration Backward: ", afpcPlayer.crouchAccelerationBackward);
					afpcPlayer.crouchStrafeAcceleration = (float)EditorGUILayout.FloatField ("Crouching Strafe Acceleration: ", afpcPlayer.crouchStrafeAcceleration);
					afpcPlayer.currentCrouchStandingRatio = (float)EditorGUILayout.IntSlider ("Crouch To Standing Ratio: ", (int)afpcPlayer.currentCrouchStandingRatio, 0, 1);
					afpcPlayer.floatOffset = (float)EditorGUILayout.FloatField ("Float Offset For Crouch\nCollision Detection CapsuleCheck: ", afpcPlayer.floatOffset);
					afpcPlayer.crouchKey = (KeyCode)EditorGUILayout.EnumPopup ("Crouch Key: ", afpcPlayer.crouchKey, "ExposablePopupMenu");
					afpcPlayer.originalLocalScaleY = (float)EditorGUILayout.FloatField ("Original Local Y Scale Of Player: ", afpcPlayer.originalLocalScaleY);
					afpcPlayer.transitionTimeOfCrouch = (float)EditorGUILayout.FloatField ("Transition Time For Crouch/Uncrouch: ", afpcPlayer.transitionTimeOfCrouch);
					afpcPlayer.crouchHeightRatio = (float)EditorGUILayout.Slider ("Crouch Height Ratio: ", afpcPlayer.crouchHeightRatio, 0f, 1f);
					EditorGUILayout.HelpBox ("Crouch To Standing Ratio Determines whether the player is standing or crouching, set this value to 1 if you want player to be standing when the game starts and 0 if you want player to be crouching when the game starts!.\nFloat Offest Is how much above the player, the Collision Detection sphere will Sweep for Collision Detection.\nOriginal Local Scale Y is the original length or height of the player transform along local y axis.\nTransition Time Of Crouch/Uncrouch is the time taken in seconds to crouch or uncrouch.", MessageType.Info, true);
				}
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");

			showExtraFields11.target = EditorGUILayout.Foldout (showExtraFields11.target, "Swimming", showExtraFields11.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields11.faded)) {
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.CANSWIM = EditorGUILayout.Toggle ("Can Swim", afpcPlayer.CANSWIM);
				if (afpcPlayer.CANSWIM) {
					afpcPlayer.swimAcceleration = (float)EditorGUILayout.FloatField ("Swimming Acceleration: ", afpcPlayer.swimAcceleration);
					afpcPlayer.swimDrag = (float)EditorGUILayout.FloatField ("Drag When Swimming: ", afpcPlayer.swimDrag);
					afpcPlayer.goDownKey = (KeyCode)EditorGUILayout.EnumPopup ("Go Down Key: ", afpcPlayer.goDownKey, "ExposablePopupMenu");
					afpcPlayer.swimForwardKey = (KeyCode)EditorGUILayout.EnumPopup ("Swim Forward Key: ", afpcPlayer.swimForwardKey, "ExposablePopupMenu");
					afpcPlayer.useGravityWhileSwimming = (bool)EditorGUILayout.Toggle ("Use Gravity While Swimming: ", afpcPlayer.useGravityWhileSwimming);
					afpcPlayer.jumpUpThrustForceUnderWater = (float)EditorGUILayout.FloatField ("Up Thrust Force in Water when pressing\njump button when player is underwater : ", afpcPlayer.jumpUpThrustForceUnderWater);
					afpcPlayer.jumpUpThrustForceOnWaterSurface = (float)EditorGUILayout.FloatField ("Up Thrust Force On Water when jump button is pressed : ", afpcPlayer.jumpUpThrustForceOnWaterSurface);
					afpcPlayer.goDownForceWhileSwimming = EditorGUILayout.FloatField ("Go Down Force While Swimming", afpcPlayer.goDownForceWhileSwimming);
					EditorGUILayout.HelpBox ("Up Thrust Force in Water when pressing Jump button underwater is the amount of force applied in upwards direction when holding down jump button in underwater", MessageType.Info, true);
				}
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");
		}

#if UNITY_ANDROID
			
			showExtraFields12.target = EditorGUILayout.Foldout (showExtraFields12.target, "Virtual Input", showExtraFields12.target);
			if (EditorGUILayout.BeginFadeGroup (showExtraFields12.faded)) 
			{
				EditorGUI.indentLevel++; 
				EditorGUILayout.BeginVertical ("WindowBackground");
				afpcPlayer.jumpButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Jump Virtual Button: ", afpcPlayer.jumpButton, typeof(AFPC_VirtualButton), true);
				
				if(afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer)
				{
					afpcPlayer.movementJoystick = (AFPC_VirtualJoystick)EditorGUILayout.ObjectField("Movement Joystick: ", afpcPlayer.movementJoystick, typeof(AFPC_VirtualJoystick), true);
					afpcPlayer.crouchButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Crouch Virtual Button: ", afpcPlayer.crouchButton, typeof(AFPC_VirtualButton), true);
					afpcPlayer.runButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Sprint Virtual Button: ", afpcPlayer.runButton, typeof(AFPC_VirtualButton), true);
					afpcPlayer.goDownButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Go Down Button: ", afpcPlayer.goDownButton, typeof(AFPC_VirtualButton), true);
				}else if(afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.spectator)
				{	
					afpcPlayer.useJoystickInSpectatorMode = EditorGUILayout.Toggle("Use Joystick For Movement: ", afpcPlayer.useJoystickInSpectatorMode);
					afpcPlayer.spectatorDownButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Go Down Button: ", afpcPlayer.spectatorDownButton, typeof(AFPC_VirtualButton), true);	
					if(!afpcPlayer.useJoystickInSpectatorMode)	
						afpcPlayer.spectatorForwardButton = (AFPC_VirtualButton)EditorGUILayout.ObjectField("Spectator Forward Virtual Button: ", afpcPlayer.spectatorForwardButton, typeof(AFPC_VirtualButton), true);
					else
						afpcPlayer.movementJoystick = (AFPC_VirtualJoystick)EditorGUILayout.ObjectField("Movement Joystick: ", afpcPlayer.movementJoystick, typeof(AFPC_VirtualJoystick), true);
				}	
				EditorGUILayout.EndVertical ();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup ();

			GUILayout.Box (GUIContent.none, "horizontalSlider");
#endif
		


		afpcPlayer.fpsCamera = (Camera)EditorGUILayout.ObjectField ("Player Camera: ", afpcPlayer.fpsCamera, typeof(Camera), true);
		afpcPlayer.playerCapsule = (CapsuleCollider)EditorGUILayout.ObjectField ("Player Capsule Collider: ", afpcPlayer.playerCapsule, typeof(CapsuleCollider), true);
		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer)
		{
			afpcPlayer.playerDynamicFriction = EditorGUILayout.Slider ("Player Dynamic Friction: ", afpcPlayer.playerDynamicFriction, 0f, 1f);
			afpcPlayer.playerStaticFriction = EditorGUILayout.Slider ("Player Static Friction: ", afpcPlayer.playerStaticFriction, 0f, 1f);
			afpcPlayer.playerMaterial = (PhysicMaterial)EditorGUILayout.ObjectField ("Player Physics Material: ", afpcPlayer.playerMaterial, typeof(PhysicMaterial), false);
		}
		if (afpcPlayer.playerType == AFPC_PlayerMovement.PlayerType.rigidBodyPlayer)
			afpcPlayer.visualize = EditorGUILayout.Toggle ("Visualize Ground Check Spheres: ", afpcPlayer.visualize);

		GUILayout.Box (GUIContent.none, "horizontalSlider");

		groundDist = EditorGUILayout.FloatField ("Ground Distance: ", groundDist);
		putOnGround = GUILayout.Button("Place On Ground!");
		EditorGUILayout.HelpBox ("Press Place On Ground Button And Set Adequate Ground Distance to Set Your PLayer Ground If NOt Already", MessageType.Info, true);

		if (putOnGround)
			PlaceOnGround (groundDist);

		EditorGUILayout.EndVertical ();

		if (EditorGUI.EndChangeCheck ()) 
		{
			Undo.RegisterCompleteObjectUndo (target, "Changed Settings");
		}

		if (GUI.changed)
		{
			EditorUtility.SetDirty (afpcPlayer);
		}
	}

	private void PlaceOnGround(float dist)
	{
		Vector3 origin = new Vector3 (afpcPlayer.transform.position.x, afpcPlayer.transform.position.y, afpcPlayer.transform.position.z);
		RaycastHit hit = new RaycastHit ();
		if (Physics.SphereCast (origin, afpcPlayer.playerCapsule.radius, Vector3.down, out hit, dist, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
			afpcPlayer.transform.position = hit.point + new Vector3 (0f, afpcPlayer.playerCapsule.height * afpcPlayer.transform.localScale.y / 2f, 0f);
		} else {
			Debug.Log ("Distance Not Enough To Touch The Ground!");
		}
	}
}
#endif