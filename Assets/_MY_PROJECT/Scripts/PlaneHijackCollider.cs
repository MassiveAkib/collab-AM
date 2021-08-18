using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXnRXn.PlaneHijack
{
    public class PlaneHijackCollider : MonoBehaviour
    {

        public string _playerTag;


        private void OnTriggerStay(Collider other)
        {
            if(string.IsNullOrEmpty(_playerTag)) return;
            
            if (other.gameObject.tag == _playerTag)
            {
                HijackMissionManager.singleton.ColliderEnterButton(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == _playerTag)
            {
                HijackMissionManager.singleton.ColliderEnterButton(false);
            }
        }
    }
}

