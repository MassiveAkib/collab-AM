using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXnRXn.Helicopter
{
    public class ColliderDetection : MonoBehaviour
    {
        public delegate void CilliderDetect();

        public static event CilliderDetect OnCollide;
        
        
        public string _targetObjTag;
        private void OnTriggerEnter(Collider other)
        {
            if (string.IsNullOrEmpty(_targetObjTag)) return;

            if (!other.CompareTag(_targetObjTag)) return;
            Debug.Log("Collide with Player");
            OnCollide?.Invoke();
        }
    }
}


