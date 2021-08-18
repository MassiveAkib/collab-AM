using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    [RequireComponent(typeof(IP_Input_Controller))]
    public class IP_Heli_Controller : IP_Base_RBController
    {
        #region Variables
        [Header("Helicopter Properties")]
        public List<IP_Heli_Engine> engines = new List<IP_Heli_Engine>();

        [Header("Helicopter Rotors")]
        public IP_Heli_Rotor_Controller rotorCtrl;

        private IP_Input_Controller input;
        private IP_Heli_Characteristics characteristics;
        //private IP_HeliWeapon_Controller weapons;
        #endregion


        #region Built in Methods
        public override void Start()
        {
            base.Start();
            characteristics = GetComponent<IP_Heli_Characteristics>();
            //weapons = GetComponentInChildren<IP_HeliWeapon_Controller>();
        }
        #endregion


        #region Custom Methods
        protected override void HandlePhysics()
        {
            input = GetComponent<IP_Input_Controller>();
            if (input)
            {
                HandleEngines();
                HandleRotors();
                HandleCharacteristics();
                //HandleWeapons();
            }
        }
        #endregion



        #region Helicopter Control Methods
        protected virtual void HandleEngines()
        {
            for(int i = 0; i < engines.Count; i++)
            {
                engines[i].UpdateEngine(input.StickyThrottle);
            }
        }

        protected virtual void HandleRotors()
        {
            if(rotorCtrl && engines.Count > 0)
            {
                rotorCtrl.UpdateRotors(input, engines[0].CurrentRPM);
            }
        }

        protected virtual void HandleCharacteristics()
        {
            if(characteristics)
            {
                characteristics.UpdateCharacteristics(rb, input);
            }
        }

        protected virtual void HandleWeapons()
        {
            // if(weapons)
            // {
            //     weapons.UpdateWeapons(input);
            // }
        }
        #endregion
    }
}