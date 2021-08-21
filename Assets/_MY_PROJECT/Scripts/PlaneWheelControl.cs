using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneWheelControl : MonoBehaviour
{
        public GameObject wheel;
        
        private void OnTriggerEnter(Collider other)
        {
                Debug.Log(other);
                if (other.gameObject.tag == "Player")
                {
                        if (wheel.gameObject.activeSelf)
                        { 
                                wheel.gameObject.SetActive(false);
                        }
                        else
                        {
                                wheel.gameObject.SetActive(true);
                        }
                        
                }
        }
    
}
