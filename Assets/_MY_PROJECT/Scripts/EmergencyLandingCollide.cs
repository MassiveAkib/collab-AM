using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FXnRXn.EmergencyLanding
{
    public class EmergencyLandingCollide : MonoBehaviour
    {
        
        public string playerTag;
        public GameObject _UIControl;

        private void OnTriggerEnter(Collider other)
        {
            if(string.IsNullOrEmpty(playerTag)) return;
            
            if (other.gameObject.CompareTag(playerTag))
            {
                EmergencyLandingMission.Singleton.EmergencyLandingPointReached();
                if (SceneManager.GetActiveScene().name != "PlaneCrash") return;
                _UIControl.gameObject.SetActive(false);
            }
        }
        
        
    
    }
}

