using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXnRXn.PlaneHijack
{
    public class HelicopterDetecCollider : MonoBehaviour
    {
        public string playerTag;


        private void OnTriggerStay(Collider other)
        {
            if(string.IsNullOrEmpty(playerTag)) return;
            
            if (other.gameObject.CompareTag(playerTag))
            {
                HijackMissionManager.singleton.HeliEnterButton(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(playerTag))
            {
                HijackMissionManager.singleton.HeliEnterButton(false);
            }
        }
    }
}




