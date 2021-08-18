using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FXnRXn.EmergencyLanding
{
    public class EmergencyLandingCollide : MonoBehaviour
    {
        
        public string _playerTag;


        private void OnTriggerEnter(Collider other)
        {
            if(string.IsNullOrEmpty(_playerTag)) return;
            
            if (other.gameObject.tag == _playerTag)
            {
                EmergencyLandingMission.singleton.EmergencyLandingPointReached();
            }
        }
        
        
    
    }
}

