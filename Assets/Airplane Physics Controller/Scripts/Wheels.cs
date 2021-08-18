using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneControl))]
public class Wheels : MonoBehaviour
{
	[Header("Wheels Settings")]
	public List<AllWheels> wheels;
	public float maxMoterTorque = 10;
	public float maxSteerAngle = 30;

	//References
	PlaneControl userInputs;
	AirplanePhysics plane;

	public void Start()
	{
		userInputs = GetComponent<PlaneControl>();
		plane = GetComponent<AirplanePhysics>();
	}

	public void FixedUpdate()
	{
		//Tires physics and animation
		if (plane.onSurface)
				WheelsMotion();
	}

	void WheelsMotion()
	{
		foreach (var wheel in wheels)
		{
			//Apply Force to wheels
			if (wheel.motor)
			{
				if (userInputs.thrustInput > 0.01f)
					wheel.wheelCollider.motorTorque = maxMoterTorque * Time.deltaTime * userInputs.thrustInput;
				else if (userInputs.thrustInput < 0.01f )
					wheel.wheelCollider.brakeTorque = maxMoterTorque * Time.deltaTime * userInputs.thrustInput * 10;
				//no force applied when no user input, it goes on it's own
			}
			if (wheel.steer && !Mathf.Approximately(userInputs.yawInput, 0))
				wheel.wheelCollider.steerAngle = userInputs.yawInput * Time.deltaTime * maxSteerAngle * 30;

			//Apply Visual Rotation to Wheels
			Vector3 position;
			Quaternion rotaion;

			wheel.wheelCollider.GetWorldPose(out position, out rotaion);
			wheel.wheelModel.transform.position = position;
			wheel.wheelModel.transform.rotation = rotaion;
		}
	}
}

[Serializable]
public struct AllWheels
{
	public GameObject wheelModel;
	public WheelCollider wheelCollider;
	public bool steer;
	public bool motor;
}
