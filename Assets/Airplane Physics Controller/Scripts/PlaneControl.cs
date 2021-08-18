using System;
using FXnRXn.PlaneParking;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AirplanePhysics))]
public class PlaneControl : MonoBehaviour
{
	public FixedJoystick _LThumbstick;
	public FixedJoystick _RThumbstick;

	AirplanePhysics airplanePhysics;
	[Header("Keyboard Input")]
	[NonSerialized]
	[Tooltip("WS Keys Input")]
	public float thrustInput;
	[NonSerialized]
	[Tooltip("AD Keys Input")]
	public float yawInput;
	[NonSerialized]
	[Tooltip("Up Down Keys Inputs")]
	public float pitchInput;
	[NonSerialized]
	[Tooltip("Left Right Keys Input")]
	public float rollInput;
	public float hoverInput;

	// Start is called before the first frame update
	void Start()
	{
		airplanePhysics = GetComponent<AirplanePhysics>();
        
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
	}
	void FixedUpdate()
	{
		ImplementInput();
	}

	void GetInput()
	{
		//User Inputs///////////////////////////////////////////////////////////
		
		if (_LThumbstick != null)
		{
			//Thrust keys
			thrustInput = _LThumbstick.Vertical;//Input.GetAxisRaw("Vertical")
			//Yaw keys
            if (SceneManager.GetActiveScene().name == "PlaneParkingScene")
            {
                yawInput = InputPlaneParking.singleton.horizontalValue;
            }
            else
            {
                yawInput = _LThumbstick.Horizontal;//Input.GetAxis("Horizontal")
            }
			
		}

		if (_RThumbstick != null)
		{
			//Pitch keys
			pitchInput = _RThumbstick.Vertical;//Input.GetAxis("Vertical1")
			//Roll keys
			rollInput = _RThumbstick.Horizontal;//Input.GetAxis("Horizontal1")
		}
		
		
		//Hovering Input
		hoverInput = Input.GetAxis("Hover");
	}

	void ImplementInput()
	{
		//Call Functions to apply physics
		airplanePhysics.Move(thrustInput, rollInput, pitchInput, yawInput, hoverInput);
	}
}