using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FXnRXn.PlaneHijack
{
    public class EjectFromCar : MonoBehaviour
    {
        public GameObject block;
        public string _carTag;
        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(_carTag))
            {
                if (other.gameObject.tag == _carTag)
                {
                    HijackMissionManager.singleton.PlayerExitButton(true);
                    
                    block.SetActive(true);
                }
            }
            
        }

        
    }
}

