using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FXnRXn.Helicopter
{
    public class Heli_MissionManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _missionText;
        [SerializeField] private Image _missionImage;
        [SerializeField] private GameObject _planeInput;
        [SerializeField] private string _missinInfo;
        [Space(15)]
        [SerializeField] private GameObject _missionMsg;
        [SerializeField] private GameObject _gameOverPanel;

        [Header("Mission Point:")] 
        [SerializeField] private List<GameObject> _missionPoint;


        private void Start()
        {
            _gameOverPanel.SetActive(false);

            DeactiveAllMissionPoint();
            SelectRandomHanger();
        }


        private void OnEnable()
        {
            ColliderDetection.OnCollide += ChangeMissionText;
        }

        private void OnDisable()
        {
            ColliderDetection.OnCollide -= ChangeMissionText;
        }

        public void ChangeMissionText()
        {
            this.Wait(1f, () =>
            {
                _missionImage.gameObject.SetActive(false);
                _planeInput.SetActive(false);
                _missionText.text = _missinInfo;
                
                this.Wait(2f, () =>
                {
                    _missionMsg.SetActive(false);
                    _gameOverPanel.SetActive(true);
                });
            });
            
        }

        public void LoadLevel(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        private void SelectRandomHanger()
        {
            int p = Random.Range(0, _missionPoint.Count);
            _missionPoint[p].SetActive(true);
            
        }
        private void DeactiveAllMissionPoint()
        {
            foreach (var p in _missionPoint)
            {
                p.SetActive(false);
            }
        }
        
        

    }
}


