using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace FXnRXn.EmergencyLanding
{
    public class EmergencyLandingMission : MonoBehaviour
    {
    
        public static EmergencyLandingMission Singleton;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
        }

        public GameObject gameOverPanel;

        private void Start()
        {
            gameOverPanel.SetActive(false);
        }

        public void EmergencyLandingPointReached()
        {
            gameOverPanel.SetActive(true);
        }

        public void SceneToLoad(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    
    
    
    
    }
}

