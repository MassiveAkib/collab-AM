using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FXnRXn.EmergencyLanding
{
    public class EmergencyLandingMission : MonoBehaviour
    {
    
        public static EmergencyLandingMission singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }

        public GameObject _gameOverPanel;

        private void Start()
        {
            _gameOverPanel.SetActive(false);
        }

        public void EmergencyLandingPointReached()
        {
            _gameOverPanel.SetActive(true);
        }

        public void SceneToLoad(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    
    
    
    
    }
}

