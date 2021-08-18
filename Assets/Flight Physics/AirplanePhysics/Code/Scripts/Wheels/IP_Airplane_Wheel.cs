using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IndiePixel
{
    [RequireComponent(typeof(WheelCollider))]
    public class IP_Airplane_Wheel : MonoBehaviour 
    {
        #region Variables
        [Header("Wheel Properties")]
        public Transform wheelGraphic;
        public bool isBraking = false;
        public float brakePower = 5f;
        public bool isSteering = false;
        public float steerAngle = 20f;
        public float steerSmoothSpeed = 2f;

        private WheelCollider WheelCol;
        private Vector3 worldPos;
        private Quaternion worldRot;
        private float finalBrakeForce;
        private float finalSteerAngle;
        #endregion


        #region Builin Methods
        void Start()
        {
            WheelCol = GetComponent<WheelCollider>();
        }
        #endregion


        #region Custom Methods
        public void InitWheel()
        {
            if(WheelCol)
            {
                WheelCol.motorTorque = 0.000000000001f;
            }
        }

        public void HandleWheel(IP_BaseAirplane_Input input)
        {
            if(WheelCol)
            {
                WheelCol.GetWorldPose(out worldPos, out worldRot);
                if(wheelGraphic)
                {
                    wheelGraphic.rotation = worldRot;
                    wheelGraphic.position = worldPos;
                }

                if(isBraking)
                {
                    if(input.Brake > 0.1f)
                    {
                        finalBrakeForce = Mathf.Lerp(finalBrakeForce, input.Brake * brakePower, Time.deltaTime);
                        WheelCol.brakeTorque = finalBrakeForce;
                    }
                    else
                    {
                        finalBrakeForce = 0f;
                        WheelCol.brakeTorque = 0f;
                        WheelCol.motorTorque = 0.000000000001f;
                    }
                }


                if(isSteering)
                {
                    finalSteerAngle = Mathf.Lerp(finalSteerAngle, -input.Yaw * steerAngle, Time.deltaTime * steerSmoothSpeed);
                    WheelCol.steerAngle = finalSteerAngle;
                }
            }
        }
        #endregion
    }
}
