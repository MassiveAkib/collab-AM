using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

[CustomEditor(typeof(AFPC_Cam))]
[CanEditMultipleObjects]
public class AFPC_CamEditor : Editor
{
	private AnimBool showExtraFields, showExtraFields1, showExtraFields2, showExtraFields3;

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
	}

	public override void OnInspectorGUI()
	{
		AFPC_Cam cam = (AFPC_Cam)target;
		EditorGUILayout.BeginVertical ("NotificationBackground");
		EditorGUI.BeginChangeCheck ();
		showExtraFields.target = EditorGUILayout.Foldout (showExtraFields.target,"Camera Settings", showExtraFields.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields.faded)) 
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			cam.CANROTATE = EditorGUILayout.Toggle ("Can Rotate: ", cam.CANROTATE);
			if (cam.CANROTATE) {
				cam.xlookSenstivity = EditorGUILayout.FloatField ("Cam X Senstivity: ", cam.xlookSenstivity);
				cam.ylookSenstivity = EditorGUILayout.FloatField ("Cam Y Senstivity: ", cam.ylookSenstivity);
				cam.clampXRotation = EditorGUILayout.Toggle ("Clamp X Rotation: ", cam.clampXRotation);
				if (cam.clampXRotation) {
					cam.minimumXRotation = EditorGUILayout.FloatField ("Minimum X Rotation: ", cam.minimumXRotation);
					cam.maximumXRotation = EditorGUILayout.FloatField ("Maximum X Rotation: ", cam.maximumXRotation);
				}
				cam.invertMouse = EditorGUILayout.Toggle ("Invert Mouse Axis: ", cam.invertMouse);
				cam.smoothRotation = EditorGUILayout.Toggle ("Smooth Rotation: ", cam.smoothRotation);
				cam.useSlerpCam = EditorGUILayout.Toggle ("Use Slerp Cam: ", cam.useSlerpCam);
				cam.useUnderwaterImageEffects = EditorGUILayout.Toggle ("Use Underwater Image Effects: ", cam.useUnderwaterImageEffects);
				if (cam.smoothRotation)
					cam.smoothTime = EditorGUILayout.FloatField ("Cam Smooth Time: ", cam.smoothTime);
				cam.tiltCamSlightly = EditorGUILayout.Toggle ("Tilt Cam Slightly when player is strafing: ", cam.tiltCamSlightly);
				if (cam.tiltCamSlightly) {
					cam.angleToTilt = EditorGUILayout.FloatField ("Angle To Tilt: ", cam.angleToTilt);
					cam.timeToTiltCamSlightly = EditorGUILayout.FloatField ("Time Taken To Tilt Cam Slightly: ", cam.timeToTiltCamSlightly);
				}
				#if UNITY_ANDROID || UNITY_IOS
			cam.cameraTouchPad = (AFPC_TouchPad)EditorGUILayout.ObjectField("Camera TouchPad: ", cam.cameraTouchPad, typeof(AFPC_TouchPad), true);
				#endif
				EditorGUILayout.HelpBox ("UseSlerpCam Makes Camera Rotation More Smooth.\nIf TiltCamSlightly is enabled, then on strafing, the camera will tilt slighty according to the given tiltAmount!", MessageType.Info, true);
			
			}
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();

		GUILayout.Box (GUIContent.none, "horizontalSlider");

		showExtraFields1.target = EditorGUILayout.Foldout (showExtraFields1.target,"Head Bob", showExtraFields1.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields1.faded)) 
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			cam.bobSpeed = EditorGUILayout.FloatField("Bob Speed: ", cam.bobSpeed);
			cam.maxHorizontalBob = EditorGUILayout.FloatField("Maximum Horizontal Bob: ", cam.maxHorizontalBob);
			cam.maxHorizontalBobWhileRunning = EditorGUILayout.FloatField("Maximum Horizontal Bob While Running: ", cam.maxHorizontalBobWhileRunning);
			cam.maxVerticalBob = EditorGUILayout.FloatField("Maximum Vertical Bob: ", cam.maxVerticalBob);
			cam.maxVerticalBobWhileRunning = EditorGUILayout.FloatField("Maximum Vertical Bob While Running: ", cam.maxVerticalBobWhileRunning);
			cam.eyeHeightRatio = EditorGUILayout.Slider("Eye Height Ratio: ", cam.eyeHeightRatio, 0f, 1f);
			EditorGUILayout.HelpBox ("Eye Height Ratio is the ratio of eyeHeight to that of player height, if it is 1, then eye Will be at the top most of the player, if 0 then at the bottom most of the player.", MessageType.Info, true);
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();

		GUILayout.Box (GUIContent.none, "horizontalSlider");

		showExtraFields2.target = EditorGUILayout.Foldout (showExtraFields2.target,"Climb Bob", showExtraFields2.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields2.faded)) 
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			cam.allowClimbBobing = EditorGUILayout.Toggle ("Do Climb Bob: ", cam.allowClimbBobing);
			cam.climbBobCurve = EditorGUILayout.CurveField ("Climb Bob Curve: ", cam.climbBobCurve);
			cam.climbBobSpeed = EditorGUILayout.FloatField("Climb Bob Speed: ", cam.climbBobSpeed);
			cam.verticalToHorizontalClimbBobRatio = EditorGUILayout.FloatField ("Vertical To Horizontal Climb Bob Ratio: ", cam.verticalToHorizontalClimbBobRatio);
			cam.maxHorizontalClimbBob = EditorGUILayout.FloatField("Maximum Horizontal Climb Bob: ", cam.maxHorizontalClimbBob);
			cam.maxVerticalClimbBob = EditorGUILayout.FloatField("Maximum Vertical Climb Bob: ", cam.maxVerticalClimbBob);
			cam.climbBobCycleInterval = EditorGUILayout.FloatField ("Climb Bob Cycle Interval: ", cam.climbBobCycleInterval);
			cam.timeToSmoothClimbBob = EditorGUILayout.FloatField ("Time Taken To Smooth Climb Bob: ", cam.timeToSmoothClimbBob);
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();

		GUILayout.Box (GUIContent.none, "horizontalSlider");


		showExtraFields3.target = EditorGUILayout.Foldout (showExtraFields3.target,"Jump Bob", showExtraFields3.target);
		if (EditorGUILayout.BeginFadeGroup (showExtraFields3.faded)) 
		{
			EditorGUI.indentLevel++; 
			EditorGUILayout.BeginVertical ("WindowBackground");
			cam.useJumpKickBackAnimation = EditorGUILayout.Toggle ("Use Animation For Jump Bob: ", cam.useJumpKickBackAnimation);
			if (cam.useJumpKickBackAnimation) {
				cam.jumpkickBack = (GameObject)EditorGUILayout.ObjectField ("Jump KickBack Gameobject: ", cam.jumpkickBack, typeof(GameObject), true);
			} else {
				cam.jumpBobAmount = EditorGUILayout.FloatField ("Jump Bob Amount: ", cam.jumpBobAmount);
				cam.jumpBobDuration = EditorGUILayout.FloatField ("Jump Bob Duration: ", cam.jumpBobDuration);
			}
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup ();

		GUILayout.Box (GUIContent.none, "horizontalSlider");

		cam.afpcPlayer = (AFPC_PlayerMovement)EditorGUILayout.ObjectField ("Afps Player: ", cam.afpcPlayer, typeof(AFPC_PlayerMovement), true);
		GUILayout.Box (GUIContent.none, "horizontalSlider");
		EditorGUILayout.EndVertical ();
		if (EditorGUI.EndChangeCheck ()) 
		{
			Undo.RegisterCompleteObjectUndo (target, "Changed Settings");
		}
		if (GUI.changed)
			EditorUtility.SetDirty (cam);
	}

}
