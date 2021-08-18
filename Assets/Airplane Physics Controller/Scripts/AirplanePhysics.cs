using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirplanePhysics : MonoBehaviour
{
	// public AudioSource _audioS;
	// public AudioClip _audioC;
	
	[Header("Lift Calculation Settings")]
	public float flyMediumDensity = 1.1f;
	public Transform[] wings;

	float wingArea;
	float dynamicPressureConstantPart;
	float dynamicPressure;
	float maxCoefficeintLift;
	float angleOfAttack;
	float currentLift;
	float maxLift;

	[Header("Thrust and Drag Settings")]
	[Tooltip("Time it takes to reach its Max Velocity in Seconds")]
	public float accelerationTime = 10;
	[Tooltip("Greater this Value = Greater Max Velocity")]
	public float accelerationMultiplier = 35;
	float accelerationFinal;
	[Tooltip("In Air, When user pressing Thrust Key, Maintaining Full Speed")]
	public float throttleMax = 1;
	[Tooltip("In Air, When no user input, Maintaing a Neutral Speed")]
	public float throttleNeutral = 0.75f;
	[Tooltip("In Air, When user input is negetive, Maintaining Minimum Possible Speed in Air to Fly")]
	public float throttleMin = 0.5f;
	[Tooltip("Higher Value represents Higher Drag (This Drag is due to Lift and in Rigidbody settings, we have general drag which works in all directions)." +
	         "This value should be between ~0 (for very small planes e.g., paper plane)  to 0.1(for bigger passanger jet planes)")]
	public float dragRatio = 0.08f;

	float throttleFinal;
	float netThrust;

	[Header("Other Settings")]
	public Transform onSurfaceCheckPoint;
	[Tooltip("Distance from OnSurfaceCheckPoint to Ground")]
	public float groundDistance = 1;
	[NonSerialized]
	public bool onSurface;
	public bool canPlaneHover;

	float lastTimeBoost;
	float hoverMultiplier;
	float SmoothDampVelocity, reference1, reference2;

	[Header("Rotation Settings")]
	public float roll = 175;
	public float pitch = 200;
	public float yaw = 150;
	public float rotationMultiplier = 500;

	[Header("Auto Stabilization and Auto Rotation Settings")]
	[Tooltip("Greater the value, greater will be automatic yaw when rolling")]
	public float yawWhenRoll;
	[Tooltip("Greater Value means faster stabilization, should be small than Rotation Multiplier")]
	public float AutoStablizationMultiplier;

	float rotorSpeed;
	Rigidbody plane;
	PartControllers controller;

	// Start is called before the first frame update
	void Start()
	{
		//References to other Scripts
		plane = GetComponent<Rigidbody>();
		TryGetComponent<PartControllers>(out controller);

		maxLift = plane.mass * 9.81f;//Weight of Plane
		wingArea = CalculateSurfaceArea();
		//Lift = (1/2)*Density*Velocity*Velocity*Wing Area*Coefficient of Lift(CL)
		//CL = Lift / (Dynamic Pressure * Area)
		//Dynamic Pressure = (1/2)*Density*Velocity*Velocity 
		//0.86 * accelerationMultiplier - Approximation for Would be Max Velocity of Plane when Rigidbody Drag = 1 (Before drag calculated here)
		maxCoefficeintLift = (maxLift * 2) / (flyMediumDensity * wingArea * (0.86f * accelerationMultiplier) * (0.86f * accelerationMultiplier));
		dynamicPressureConstantPart = wingArea * 0.5f * flyMediumDensity;
	}

	//Call this function in Fixed update 
	public void Move(float thrustInput, float rollInput, float pitchInput, float yawInput, float movingPartInput)
	{
		isOnSurace();//Check if Plane is on Surface
		CalculateThrust(thrustInput);//Calculate Thrust According to User Input
		CalculateLift();//Calculate Lift According to given variables and Thrust
		ApplyForce(thrustInput, movingPartInput);//Apply Forces to Plane
		ApplyRotatoion(rollInput, pitchInput, yawInput);//Apply Torques for Rotation of Plane


		// if (thrustInput > 0.1f)
		// {
		// 	_audioS.clip = _audioC;
		// 	_audioS.Play();
		// 	Debug.Log("Audio");
		// }
		// else
		// {
		// 	_audioS.Stop();
		// }
		
		
		//Procedural animation of Plane Parts - Propellers, Rudders, Flaps and other Moving or Rotating Parts
		if (controller != null)
		{
			if (onSurface)
			//Propeller Rotation
			{
				if (thrustInput <= 0)//Plane is on Ground, Rotate according to User Input
				{
					rotorSpeed = Mathf.SmoothDamp(rotorSpeed, 0, ref reference2, accelerationTime + accelerationTime / 2);
					controller.RotatePropellers(rotorSpeed);
				}
				//Propeller Rotation
				else //thrustInput > 0 - Plane is on Ground, Rotate according to User Input
				{
					rotorSpeed = Mathf.SmoothDamp(rotorSpeed, 1, ref reference2, accelerationTime - accelerationTime / 2);
					controller.RotatePropellers(rotorSpeed);
				}
			}
			//Propeller Rotation
			else if (!onSurface)//Rotate at Full Speed when plane is in Air
				controller.RotatePropellers(rotorSpeed = 1);

			//For Procedural animatin of other rotating and moving parts of plane
			controller.yawKeys = yawInput;
			controller.rollKeys = rollInput;
			controller.pitchKeys = pitchInput;
		}
	}

	//Lift Calculation
	void CalculateLift()
	{
		//Angle of Attack and Coefficent of Lift calculation////////////////////////////////////////////////////////////
		//Not Using All Real World equations for Lift Calculation for Sake of Simplicity

		// How much tilted in Forward or Backward Direction
		 angleOfAttack = transform.rotation.eulerAngles.x;
		angleOfAttack = Mathf.Deg2Rad * angleOfAttack;
		angleOfAttack = Mathf.Cos(angleOfAttack);

		//dynamic Pressure = velocity * velocity * flyMediumDensity * 0.5f * wingArea
		dynamicPressure = plane.velocity.sqrMagnitude * dynamicPressureConstantPart;

		//Final Lift Force to be applied on Plane in Relative Space
		currentLift = dynamicPressure * angleOfAttack * maxCoefficeintLift;
	}

	//Calculate Thrust for Plane
	void CalculateThrust(float thrustInput)
	{
		//On Surface////////////////////////////////////////////////////////////////////////////////
		if (onSurface)
		{
			if (thrustInput > 0.01)//Positive user input
				throttleFinal = Mathf.SmoothDamp(throttleFinal, throttleMax, ref reference1, accelerationTime);
			else if (thrustInput < 0.01)//negetive user input //plane deaccelerates at twice fast rate
				throttleFinal = Mathf.SmoothDamp(throttleFinal, 0, ref reference1, accelerationTime);
			else //nill user input i.e., thrustInput <= 0 //plane deaccelerates at normal rate
				throttleFinal = Mathf.SmoothDamp(throttleFinal, 0, ref reference1, accelerationTime);
		}
		//On Air///////////////////////////////////////////////////////////////////////////////////
		if (!onSurface)
		{
			if (Mathf.Approximately(thrustInput, 0) && Mathf.Abs(throttleFinal - throttleNeutral) > 0.025f)//no user input - Go to neutral speed
				throttleFinal = Mathf.SmoothDamp(throttleFinal, throttleNeutral, ref SmoothDampVelocity, accelerationTime);
			else if (thrustInput > 0.01f && throttleMax - throttleFinal > 0.025f)//positive input (i.e., > 0) - increase speed, Go to Max Speed
				throttleFinal = Mathf.SmoothDamp(throttleFinal, throttleMax, ref SmoothDampVelocity, accelerationTime);
			else if (thrustInput < -0.01f && throttleFinal - throttleMin > 0.025f)//negetive input (i.e., < 0) - decrease speed, Go to Min Speed
				throttleFinal = Mathf.SmoothDamp(throttleFinal, throttleMin, ref SmoothDampVelocity, accelerationTime);
		}

		accelerationFinal = throttleFinal * accelerationMultiplier;//Final Acceleration to be applied on plane
		netThrust = (plane.mass * accelerationFinal) - currentLift * dragRatio;//Net Thrust
	}

	//Apply Forward Thrust and Upward Lift Relative to Plane
	void ApplyForce(float ThrustInput, float angle)
	{
		float upThrust = 0;//If plane hover then, amount of Forward Thrust Force changed to Lift
		float clampedLift = maxLift;

		//If Plane can Hover
		if (canPlaneHover)
		{
			//Rotate Moving Parts of Plane i.e., change plane Engine Direction so Forward Thrust changes to Upward Lift
			float previous = hoverMultiplier;
			hoverMultiplier = Mathf.Clamp(hoverMultiplier +  angle / 2 * Time.deltaTime, 0, 1);
			if (!Mathf.Approximately(previous, hoverMultiplier))
				controller.RotateParts(hoverMultiplier);
			
			//And change forward thrust to upward Lift according to user input
			upThrust = hoverMultiplier * netThrust;
			netThrust = netThrust - upThrust;
			if (Mathf.Abs(ThrustInput) > 0)
				clampedLift += maxLift / 4 * Mathf.Sign(ThrustInput);//When Hovering and Wants to go up or down, then Clamped Lift is increases or Decreases by 0.25 times
		}

		//Apply final Forces 
		//Clamping Max Lift, so that plane flies in forward direction only No Asscends or Descends until user Wants to in No Hover Case
		plane.AddRelativeForce(0, Mathf.Clamp(currentLift + upThrust, 0, clampedLift), netThrust, ForceMode.Force);
	}

	//Rotation for Plane
	private void ApplyRotatoion(float RollInput, float PitchInput, float YawInput)
	{
		float Pitch1 = 0;
		float Yaw1 = 0;
		float Roll1 = 0;
		float additionalYaw = 0;

		//Rotation according to user input and rotation sensitivity settings
		Pitch1 = pitch * rotationMultiplier * PitchInput;
		Roll1 = roll * rotationMultiplier * RollInput;
		Yaw1 = yaw * rotationMultiplier * YawInput;
		additionalYaw = Roll1 * yawWhenRoll;

		//Auto Stabilization Rotation - If plane is rotated in forward/backward or rightward/leftward direction then apply it
		//If don't need this feature just remove Complete If Statement and Autostabilization variable
		if (AutoStablizationMultiplier > 0)
		{
			if (Mathf.Approximately(PitchInput, 0))
			{
				
				float bankFactorForward = transform.forward.y;//how much forward/backward tilted
				Pitch1 = Pitch1 + pitch * AutoStablizationMultiplier * bankFactorForward;
			}
			if (Mathf.Approximately(RollInput, 0))
			{
				float bankFactorRight = -transform.right.y;//how much right/left tilted
				Roll1 = Roll1 + roll * AutoStablizationMultiplier * bankFactorRight;
			}
		}
		//Apply Final Rotation
		plane.AddRelativeTorque(Pitch1, Yaw1 - additionalYaw, Roll1, ForceMode.Force);
	}

	//Check Whether Plane is Close to surface i.e., ground
	void isOnSurace()
	{
		RaycastHit hit;
		//Hint - May add here layerMask for optimization purpose as without that Raycast will be checked on each types of surface
		Physics.Raycast(onSurfaceCheckPoint.position, Vector3.down, out hit, groundDistance);

		if (hit.collider == null)
		{
			onSurface = false;
			return;
		}
		onSurface = true;
	}

	float CalculateSurfaceArea()
	{
		float area = 0;
		for (int i = 0; i < wings.Length; i++)
		{
			Mesh mesh = wings[i].GetComponent<MeshFilter>().sharedMesh;
			int[] triangles =  mesh.triangles;
			Vector3[] vertices = mesh.vertices;

			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j].x *= wings[i].transform.localScale.x;
				vertices[j].y *= wings[i].transform.localScale.y;
				vertices[j].z *= wings[i].transform.localScale.z;
			}
			for (int j = 0; j < triangles.Length; j += 3)
			{
				Vector3 corner = vertices[triangles[j]];
				Vector3 pointA = vertices[triangles[j + 1]] - corner;
				Vector3 pointB = vertices[triangles[j + 2]] - corner;
				area += Vector3.Cross(pointA, pointB).magnitude / 2;
			}
		}
		return (area / 2);
	}
}
