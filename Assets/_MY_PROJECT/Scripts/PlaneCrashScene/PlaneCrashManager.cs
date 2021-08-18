using System;
using System.Collections;
using System.Collections.Generic;
using MassiveStar;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FXnRXn.PlaneCrash
{

    
    public class PlaneCrashManager : MonoBehaviour
    {

        #region Singleton

        public static PlaneCrashManager singleton;

        private void Awake()
        {
            singleton = this;
        }

        #endregion
        
        
        [Header("Refference :")] 
        [SerializeField] private GameObject _planeObj;
        [SerializeField] private GameObject _planeInputUI;
        [SerializeField] private GameObject _playerObj;
        [SerializeField] private GameObject _playerInputUI;
        [SerializeField] private GameObject _waterZone;
        [Space(10)] 
        [SerializeField] private GameObject _indicator1;

        [Header("UI Refference :")] 
        [SerializeField] private GameObject _ejectFromPlaneButton;


        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            Engine.OnPlaneHitWater += PlayerInstantiate;
        }

        private void OnDisable()
        {
            Engine.OnPlaneHitWater -= PlayerInstantiate;
        }


        private void Init()
        {
            _ejectFromPlaneButton.SetActive(false);
            _playerInputUI.SetActive(false);
            _waterZone.SetActive(false);
        }


        public void OnPlaneCrash()
        {
            Destroy(_indicator1);
        }


        public void PlayerInstantiate()
        {
            _planeInputUI.SetActive(false);
            _ejectFromPlaneButton.SetActive(true);
        }

        public void PlayerInstantiateCoroutine()
        {
            _planeObj.SetActive(false);
            _playerObj.transform.parent = null;
            //PlayerController.singleton._isUnderwater = true;
            _playerObj.SetActive(true);
            _ejectFromPlaneButton.SetActive(false);
            _playerInputUI.SetActive(true);
            _waterZone.SetActive(true);
        }
    }
}


