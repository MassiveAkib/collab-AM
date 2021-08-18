using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    public class IP_HeliTail_Rotor : MonoBehaviour, IP_IHeliRotor
    {
        #region Variables
        [Header("Tail Rotor Properties")]
        public Transform lRotor;
        public Transform rRotor;
    
        public float maxPitch = 45f;
        public float rotationSpeedModifier = 1.5f;
        #endregion

        #region Builtin Methods
        // Use this for initialization
        void Start()
        {

        }
        #endregion

        #region Interface Methods
        public void UpdateRotor(float dps, IP_Input_Controller input)
        {
            //Debug.Log("Updating Tail Rotor");
            transform.Rotate(Vector3.right, dps * rotationSpeedModifier * Time.deltaTime);

            //Pitch the blased up and down
            if (lRotor && rRotor)
            {
                lRotor.localRotation = Quaternion.Euler(0f, input.PedalInput * maxPitch, 0f);
                rRotor.localRotation = Quaternion.Euler(0f, -input.PedalInput * maxPitch, 0f);
            }
        }
        #endregion
    }
}
