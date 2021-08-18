using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    [RequireComponent(typeof(IP_Airplane_Characteristics))]
    public class IP_Airplane_Controller : IP_BaseRigidbody_Controller 
    {
        
        #region Variables
        [Header("Base Airplane Properties")]
        public IP_Airplane_Preset airplanePreset;
        public IP_BaseAirplane_Input input;
        public IP_Airplane_Characteristics charactistics;
        public Transform centerOfGravity;

        [Tooltip("Weight is in LBS")]
        public float airplaneWeight = 800f;

        [Header("Engines")]
        public List<IP_Airplane_Engine> engines = new List<IP_Airplane_Engine>();

        [Header("Wheels")]
        public List<IP_Airplane_Wheel> wheels = new List<IP_Airplane_Wheel>();

        [Header("Control Surfaces")]
        public List<IP_Airplane_ControlSurface> controlSurfaces = new List<IP_Airplane_ControlSurface>();
        #endregion


        #region Properties
        private float currentMSL;
        public float CurrentMSL
        {
            get{return currentMSL;}
        }

        private float currentAGL;
        public float CurrentAGL
        {
            get{return currentAGL;}
        }
        #endregion




        #region Constants
        const float poundsToKilos = 0.453592f;
        const float metersToFeet = 3.28084f;
        #endregion





        #region Builtin Methods
        public override void Start()
        {
            GetPresetInfo();

            base.Start();

            float finalMass = airplaneWeight * poundsToKilos;
            if(rb)
            {
                rb.mass = finalMass;
                if(centerOfGravity)
                {
                    rb.centerOfMass = centerOfGravity.localPosition;
                }

                charactistics = GetComponent<IP_Airplane_Characteristics>();
                if(charactistics)
                {
                    charactistics.InitCharacteristics(rb, input);
                }
            }

            if(wheels != null)
            {
                if(wheels.Count > 0)
                {
                    foreach(IP_Airplane_Wheel wheel in wheels)
                    {
                        wheel.InitWheel();
                    }
                }
            }
        }
        #endregion





        #region Custom Methods
        protected override void HandlePhysics()
        {
            if(input)
            {
                HandleEngines();
                HandleCharacteristics();
                HandleControlSurfaces();
                HandleWheel();
                HandleAltitude();
            }
        }


        void HandleEngines()
        {
            if(engines != null)
            {
                if(engines.Count > 0)
                {
                    foreach(IP_Airplane_Engine engine in engines)
                    {
                        rb.AddForce(engine.CalculateForce(input.StickyThrottle));
                    }
                }
            }
        }

        void HandleCharacteristics()
        {
            if(charactistics)
            {
                charactistics.UpdateCharacteristics();
            }
        }

        void HandleControlSurfaces()
        {
            if(controlSurfaces.Count > 0)
            {
                foreach(IP_Airplane_ControlSurface controlSurface in controlSurfaces)
                {
                    controlSurface.HandleControlSurface(input);
                }
            }
        }

        void HandleWheel()
        {
            if(wheels.Count > 0)
            {
                foreach(IP_Airplane_Wheel wheel in wheels)
                {
                    wheel.HandleWheel(input);
                }
            }
        }

        void HandleAltitude()
        {
            currentMSL = transform.position.y * metersToFeet;

            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                if(hit.transform.tag == "ground")
                {
                    currentAGL = (transform.position.y - hit.point.y) * metersToFeet;
                }
            }
        }

        void GetPresetInfo()
        {
            if(airplanePreset)
            {
                airplaneWeight = airplanePreset.airplaneWeight;
                if(centerOfGravity)
                {
                    centerOfGravity.localPosition = airplanePreset.cogPosition;
                }

                if(charactistics)
                {
                    charactistics.dragFactor = airplanePreset.dragFactor;
                    charactistics.flapDragFactor = airplanePreset.flapDragFactor;
                    charactistics.liftCurve = airplanePreset.liftCurve;
                    charactistics.maxLiftPower = airplanePreset.maxLiftPower;
                    charactistics.maxMPH = airplanePreset.maxMPH;
                    charactistics.rollSpeed = airplanePreset.rollSpeed;
                    charactistics.yawSpeed = airplanePreset.yawSpeed;
                    charactistics.pitchSpeed = airplanePreset.pitchSpeed;
                    charactistics.rbLerpSpeed = airplanePreset.rbLerpSpeed;
                }
            }
        }
        #endregion
    }
}
