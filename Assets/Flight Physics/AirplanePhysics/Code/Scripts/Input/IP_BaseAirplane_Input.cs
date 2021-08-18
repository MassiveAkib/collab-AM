using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    public class IP_BaseAirplane_Input : MonoBehaviour 
    {
        #region Variables
        protected float pitch = 0f;
        protected float roll = 0f;
        protected float yaw = 0f;
        protected float throttle = 0f;
        public float throttleSpeed = 0.1f;


        protected float stickyThrottle;
        public float StickyThrottle
        {
            get{return stickyThrottle;}
        }


        [SerializeField]
        private KeyCode brakeKey = KeyCode.Space;
        protected float brake = 0f;

        [SerializeField]
        protected KeyCode cameraKey = KeyCode.C;
        protected bool cameraSwitch = false;

        public int maxFlapIncrements = 2;
        protected int flaps = 0;
        #endregion





        #region Properties
        public float Pitch
        {
            get{return pitch;}
        }

        public float Roll
        {
            get{return roll;}
        }

        public float Yaw
        {
            get{return yaw;}
        }

        public float Throttle
        {
            get{return throttle;}
        }

        public int Flaps
        {
            get{return flaps;}
        }
            
        public float NormalizedFlaps
        {
            get
            {
                return (float)flaps / maxFlapIncrements;
            }
        }

        public float Brake
        {
            get{return brake;}
        }

        public bool CameraSwitch
        {
            get{return cameraSwitch;}
        }
        #endregion






        #region Builtin Methods
    	// Use this for initialization
    	void Start () {}
    	
    	// Update is called once per frame
    	void Update () 
        {
            HandleInput();
            StickyThrottleControl();
            ClampInputs();
    	}
        #endregion






        #region Custom Methods
        protected virtual void HandleInput()
        {
            //Process Main Control Input
            pitch = Input.GetAxis("Vertical");
            roll = Input.GetAxis("Horizontal");
            yaw = Input.GetAxis("Yaw");
            throttle = Input.GetAxis("Throttle");


            //Process Brake inputs
            brake = Input.GetKey(brakeKey)? 1f : 0f;

            //Process Flaps Inputs
            if(Input.GetKeyDown(KeyCode.F))
            {
                flaps += 1;
            }

            if(Input.GetKeyDown(KeyCode.G))
            {
                flaps -= 1;
            }

            //flaps = Mathf.Clamp(flaps, 0, maxFlapIncrements);

            //Camera Switch Key
            cameraSwitch = Input.GetKeyDown(cameraKey);

        }


        //Create a Throttle Value that gradually grows and shrinks
        protected void StickyThrottleControl()
        {
            stickyThrottle = stickyThrottle + (-throttle * throttleSpeed * Time.deltaTime);
            stickyThrottle = Mathf.Clamp01(stickyThrottle);
        }

        protected void ClampInputs()
        {
            pitch = Mathf.Clamp(pitch, -1f, 1f);
            roll = Mathf.Clamp(roll, -1f, 1f);
            yaw = Mathf.Clamp(yaw, -1f, 1f);
            throttle = Mathf.Clamp(throttle, -1f, 1f);
            brake = Mathf.Clamp(brake, 0f, 1f);
            
            flaps = Mathf.Clamp(flaps, 0, maxFlapIncrements);
        }
        #endregion
    }
}
