using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MassiveStar
{
    
    public class PlayerColliderTrigger : MonoBehaviour
    {
        public string sceneToLoad;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                MissionManager.singleton.OnPlayerCollide(sceneToLoad);
            }
        }
    }
}

