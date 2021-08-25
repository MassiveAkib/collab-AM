//
// Copyright (c) Brian Hernandez. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using System;
using FXnRXn.PlaneCrash;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Engine : MonoBehaviour
{
	public delegate void PlaneCrashState();
	public static event PlaneCrashState OnPlaneHitWater;
	public static event PlaneCrashState OnPlaneCrash;
	
	[Range(0, 1)]
	public float throttle = 1.0f;

	[Tooltip("How much power the engine puts out.")]
	public float thrust;

	private Rigidbody rigid;
	[HideInInspector]
	public bool _isFlying;

	private bool _isPlaneCrashed;
	[Header("Trail :")] 
	[SerializeField] private TrailRenderer _trailL;
	[SerializeField] private TrailRenderer _trailR;

	private void Awake()
	{
		rigid = GetComponentInParent<Rigidbody>();
		_isFlying = true;
		_isPlaneCrashed = false;
	}

	private void Start()
	{
		_trailL.emitting = false;
		_trailR.emitting = false;
	}

	private void FixedUpdate()
	{
		if (rigid != null && _isFlying)
		{
			rigid.AddRelativeForce(Vector3.forward * thrust * throttle, ForceMode.Force);
		}
		else
		{
			rigid.AddRelativeForce(Vector3.down * 10f, ForceMode.Force);
		}
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Collider")
		{
			
			Destroy(other.gameObject);
			thrust = 0;
			_isFlying = false;
			throttle = 0;
			rigid.velocity = Vector3.zero;
			//rigid.angularVelocity = Vector3.zero;
			_isPlaneCrashed = true;
			this.Wait(2f, () =>
			{
				_trailL.emitting = true;
				_trailR.emitting = true;
			});
		}

		// if (other.gameObject.tag == "Water" && _isPlaneCrashed)
		// {
		// 	OnPlaneHitWater?.Invoke();
		// 	rigid.mass = 1000f;
		// 	rigid.drag = 1f;
		// }
		if(other.gameObject.tag == "Water" && !_isPlaneCrashed)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
