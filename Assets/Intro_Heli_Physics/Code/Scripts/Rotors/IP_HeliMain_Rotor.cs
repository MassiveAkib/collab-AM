using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    public class IP_HeliMain_Rotor : MonoBehaviour, IP_IHeliRotor
    {
        #region Variables
        [Header("Main Rotor Properties")]
        public Transform lRotor;
        public Transform rRotor;
        public float maxPitch = 35f;

        public float radius = 2f;

        [HideInInspector]
        public Vector2 cyclicVal;
        #endregion


        #region Properties
        private float currentRPMs;
        public float CurrentRPMs
        {
            get { return currentRPMs; }
        }
        #endregion


        #region Builtin Methods
        #endregion

        #region Interface Methods
        public void UpdateRotor(float dps, IP_Input_Controller input)
        {
            //Debug.Log("Updating Main Rotor");
            currentRPMs = (dps / 360) * 60f;
            //Debug.Log(currentRPMs);
            transform.Rotate(Vector3.up, dps * Time.deltaTime * 0.5f);

            //Figure out the Swash blade effect
            Vector3 discNormal = Vector3.Normalize(transform.up + new Vector3(-cyclicVal.x, 0f, -cyclicVal.y));

            //Pitch the blades up and down
            if (lRotor && rRotor)
            {
                cyclicVal = input.CyclicInput;

                lRotor.localRotation = Quaternion.Euler(-input.StickyCollectiveInput * maxPitch, 0f, 0f);
                rRotor.localRotation = Quaternion.Euler(input.StickyCollectiveInput * maxPitch, 0f, 0f);
            }
        }
        #endregion
    }
}
