using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWheelControl : MonoBehaviour
{
        public GameObject wheel;
        
        private void OnTriggerEnter(Collider other)
        {
                if (!other.gameObject.CompareTag("Player")) return;
                wheel.gameObject.SetActive(!wheel.gameObject.activeSelf);
        }
    
}
