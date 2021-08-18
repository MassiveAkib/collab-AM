using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXnRXn.PlaneHijack
{
    public class HelicopterDetecCollider : MonoBehaviour
    {
        public string _playerTag;


        private void OnTriggerStay(Collider other)
        {
            if(string.IsNullOrEmpty(_playerTag)) return;
            
            if (other.gameObject.tag == _playerTag)
            {
                HijackMissionManager.singleton.HeliEnterButton(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == _playerTag)
            {
                HijackMissionManager.singleton.HeliEnterButton(false);
            }
        }
    }
}




