using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FXnRXn.PlaneHijack
{
    public class EjectFromCar : MonoBehaviour
    {
        public GameObject WinScreen;
        public GameObject block;
        public string _carTag;
        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(_carTag))
            {
                if (other.gameObject.tag == _carTag)
                {
                    HijackMissionManager.singleton.PlayerExitButton(true);
                    WinScreen.SetActive(true);
                    block.SetActive(true);
                    Time.timeScale = 0;
                }
            }
            
        }

        public void NextScene()
        {
            Time.timeScale = 1;
            WinScreen.SetActive(false);
        }
        
    }

}

