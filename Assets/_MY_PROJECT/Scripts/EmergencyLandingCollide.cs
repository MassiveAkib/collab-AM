using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FXnRXn.EmergencyLanding
{
    public class EmergencyLandingCollide : MonoBehaviour
    {
        
        public string playerTag;


        private void OnTriggerEnter(Collider other)
        {
            if(string.IsNullOrEmpty(playerTag)) return;
            
            if (other.gameObject.CompareTag(playerTag))
            {
                EmergencyLandingMission.Singleton.EmergencyLandingPointReached();
            }
        }
        
        
    
    }
}

