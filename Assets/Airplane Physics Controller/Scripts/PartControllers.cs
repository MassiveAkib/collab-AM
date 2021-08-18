using System;
using System.Collections.Generic;
using UnityEngine;

public class PartControllers : MonoBehaviour
{
	[Header("For Propellers")]
	[SerializeField]
	[Tooltip("Use another set of Array for opposite spin or different speed")]
	public List<Propellers> propellers;

	[Header("Other Parts")]
	[SerializeField]
	[Tooltip("For Example - Rudders")]
	public List<SingleController> singles;
	[SerializeField]
	[Tooltip("For Example - Flaps")]
	public List<DualController> duals;
	[SerializeField]
	[Tooltip("For Example - Engine of Vertical Take Off Plane")]
	public List<MovingParts> movingParts;

	[NonSerialized]
	public float yawKeys;//ADKeys
	[NonSerialized]
	public float pitchKeys;//UP Down Keys
	[NonSerialized]
	public float rollKeys;//Left Right Keys

	// Update is called once per frame
	void FixedUpdate()
	{
		if (singles.Count > 0)
		{
			if (Mathf.Abs(yawKeys) > Mathf.Epsilon || singles[0].part.transform.localEulerAngles.magnitude > 5)
			{
				RotateSingleParts();
			}
		}
		if (duals.Count > 0)
			RotateDualParts();
	}

	//Make sure all Rudder and Flaps rotation initially is 0 on all axis
	//using local space for rotation
	public void RotateSingleParts()//Single Parts like Rudder
	{
		foreach (var single in singles)
		{
			float rotationAngle = -yawKeys * single.maxRotateAngle;
			Quaternion rotation = Quaternion.Lerp(single.part.transform.localRotation, Quaternion.Euler((single.rotationAxis) * rotationAngle), single.rotationSpeed * Time.deltaTime);
			single.part.transform.localRotation = rotation;
		}
	}

	//Dual Parts like Flaps
	public void RotateDualParts()
	{
		//Rotation of Plane Parts - When Roll
		if (Mathf.Abs(rollKeys) > Mathf.Epsilon)
		{
			foreach (var dual in duals)
			{
				float rotationAngle = rollKeys * dual.maxRotateAngle;
				//Calculating for Left and Right Flaps Seperately as when rolling both flaps rotates differently
				Quaternion leftRotation = Quaternion.Lerp(dual.leftPart.transform.localRotation, Quaternion.Euler((dual.rotationAxis) * -rotationAngle), dual.rotationSpeed * Time.deltaTime);
				Quaternion rightRotation = Quaternion.Lerp(dual.rightPart.transform.localRotation, Quaternion.Euler((dual.rotationAxis) * rotationAngle), dual.rotationSpeed * Time.deltaTime);
				dual.leftPart.transform.localRotation = leftRotation;
				dual.rightPart.transform.localRotation = rightRotation;
			}
		}

		//Rotation of Plane Parts - When Pitch
		else if (Mathf.Abs(pitchKeys) > Mathf.Epsilon)
		{
			foreach (var dual in duals)
			{
				float rotationAngle = pitchKeys * dual.maxRotateAngle;
				//Here Left and right part, both rotates in same direction so, 1 time calculation only
				Quaternion rotation = Quaternion.Lerp(dual.leftPart.transform.localRotation, Quaternion.Euler((dual.rotationAxis) * -rotationAngle), dual.rotationSpeed * Time.deltaTime);
				dual.leftPart.transform.localRotation = rotation;
				dual.rightPart.transform.localRotation = rotation;
			}
		}

		else if (duals[0].leftPart.transform.localRotation.eulerAngles.magnitude > 5)//Back to zero - when user releases rotation input keys
		{
			foreach (var dual in duals)
			{
				//Vector3 eulerRotation = Vector3.Lerp(dual.leftPart.transform.localEulerAngles, new Vector3(0, 0, 0), dual.rotationSpeed * Time.deltaTime);
				Quaternion leftRotation = Quaternion.Lerp(dual.leftPart.transform.localRotation, Quaternion.Euler(0, 0, 0), dual.rotationSpeed * Time.deltaTime);
				Quaternion rightRotation = Quaternion.Lerp(dual.rightPart.transform.localRotation, Quaternion.Euler(0, 0, 0), dual.rotationSpeed * Time.deltaTime);
				dual.leftPart.transform.localRotation = leftRotation;
				dual.rightPart.transform.localRotation = rightRotation;
			}
		}
	}

	//Propeller's Rotation
	public void RotatePropellers(float proppelerRotationSpeedMultiplier)
	{
		foreach (var propeller in propellers)
		{
			propeller.propellerObject.transform.Rotate(propeller.rotationAxis, proppelerRotationSpeedMultiplier * propeller.rotationSpeed * Time.deltaTime);
		}
	}

	//Engine Rotation for Vertical Take off Aircraft or Can also be used For Gears OFF and ON
	public void RotateParts(float currentAngle)
	{
		foreach (var parts in movingParts)
		{
			parts.movingPart.localRotation = Quaternion.AngleAxis(currentAngle * parts.maxAngle, parts.rotationAxis);
		}
	}
}

[Serializable]
public struct DualController
{
	public GameObject leftPart;
	public GameObject rightPart;
	public float maxRotateAngle;
	public float rotationSpeed;
	public Vector3 rotationAxis;
}

[Serializable]
public struct SingleController
{
	public GameObject part;
	public float maxRotateAngle;
	public float rotationSpeed;
	public Vector3 rotationAxis;
}

[Serializable]
public struct Propellers
{
	public GameObject propellerObject;
	public Vector3 rotationAxis;
	public float rotationSpeed;
}

[Serializable]
public struct MovingParts
{
	public Transform movingPart;
	public Vector3 rotationAxis;
	public float maxAngle;
}