using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MassiveStar
{
    
    public class PlayerColliderTrigger : MonoBehaviour
    {
        public string _sceneToLoad;
        private void OnTriggerEnter(Collider other)
        {
           // IPlayerCollide playerCollide = GetComponentInParent<IPlayerCollide>();
            if (other.gameObject.tag == "Player")
            {
                MissionManager.singleton.OnPlayerCollide(_sceneToLoad);
            }
        }
    }
}

