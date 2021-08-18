using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FXnRXn.PlaneHijack
{
    public class HijackMissionManager : MonoBehaviour
    {
        #region Singleton

        public static HijackMissionManager singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }

            OnAwake();
        }

        #endregion


        #region Variable

        [Header("Mission Indicator :")]
       // public List<GameObject> _indicator;
        
        [Header("Player Shooting :")] 
        public GameObject shooting_PlayerObj;
        public GameObject shooting_PlayerUI;
        public GameObject shooting_UIHp;
        public List<GameObject> enemyList;
        
        
        [Header("Player Controller :")] 
        public GameObject controller_PlayerObj;
        public GameObject controller_PlayerUI;


        [Header("Car Controller :")] 
        public Transform c_playerParent;
        public GameObject c_car;
        public GameObject c_enterButton;
        public GameObject c_exitButton;
        public GameObject c_inputUI;

        [Header("Helicopter Controller :")] 
        public Transform h_playerParent;
        public GameObject h_PlayerCamera;
        public GameObject h_enterButton;
        public GameObject h_exitButton;
        public GameObject h_inputUI;

        #endregion

        private void OnAwake()
        {
            DisableShootingPlayer();
            DisableCar();
            DisableHeli();

            //IndicatorState(1);
        }
        private void Start()
        {
            // Car enter and exit
            c_enterButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerEnterCar();
            });
            
            c_exitButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerExitCar();
            });
            
            
            // Helicopter enter exit
            
            h_enterButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerEnterHeli();
            });
            
            h_exitButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerExitHeli();
            });
        }

        public void ColliderEnterButton(bool b)
        {
            c_enterButton.SetActive(b);
        }
        
        
        public void HeliEnterButton(bool b)
        {
            h_enterButton.SetActive(b);
        }
        

        private void DisableShootingPlayer()
        {
            shooting_PlayerObj.SetActive(false);
            shooting_PlayerUI.SetActive(false);
            shooting_UIHp.SetActive(false);
            foreach (var enemy in enemyList)
            {
                enemy.SetActive(false);
            }
        }

        private void DisableCar()
        {
            c_car.SetActive(false);
            c_enterButton.SetActive(false);
            c_inputUI.SetActive(false);
            c_exitButton.SetActive(false);
        }

        private void DisableHeli()
        {
            h_PlayerCamera.SetActive(false);
            h_enterButton.SetActive(false);
            h_exitButton.SetActive(false);
            h_inputUI.SetActive(false);
        }

        // ------------     CAR      ---------------

        public void PlayerEnterCar()
        {
            //Player
            controller_PlayerObj.transform.parent = c_playerParent;
            controller_PlayerObj.SetActive(false);
            controller_PlayerUI.SetActive(false);
            
            //IndicatorState(2);
            
            //car
            c_car.SetActive(true);
            c_enterButton.SetActive(false);
            c_inputUI.SetActive(true);
        }

        public void PlayerExitButton(bool b)
        {
            c_exitButton.SetActive(b);
        }

        public void PlayerExitCar()
        {
            controller_PlayerObj.transform.parent = null;
            controller_PlayerObj.SetActive(true);
            controller_PlayerUI.SetActive(true);
            
            //IndicatorState(3);
            
            c_car.SetActive(false);
            c_inputUI.SetActive(false);
            PlayerExitButton(false);
        }
        
        
        // --------------------      HELICOPTER    ------------------

        public void PlayerEnterHeli()
        {
            //Player
            controller_PlayerObj.transform.parent = h_playerParent;
            controller_PlayerObj.SetActive(false);
            controller_PlayerUI.SetActive(false);
            
            //IndicatorState(4);
            //Heli
            h_PlayerCamera.SetActive(true);
            h_inputUI.SetActive(true);
            HeliEnterButton(false);
        }
        
        public void PlayerExitHeli(bool b)
        {
            h_exitButton.SetActive(b);
        }

        public void PlayerExitHeli()
        {
            h_PlayerCamera.SetActive(false);
            h_inputUI.SetActive(false);
            PlayerExitHeli(false);
            
            shooting_PlayerObj.SetActive(true);
            shooting_PlayerUI.SetActive(true);
            shooting_UIHp.SetActive(true);
            
            foreach (var enemy in enemyList)
            {
                enemy.SetActive(true);
            }
            // foreach (var indicator in _indicator)
            // {
            //     indicator.SetActive(false);
            // }
        }
        
        //  --------------    INDICATOR

        // public void IndicatorState(int val)
        // {
        //     switch (val)
        //     {
        //         case 1:
        //             foreach (var indicator in _indicator)
        //             {
        //                 indicator.SetActive(false);
        //             }
        //             _indicator[val-1].SetActive(true);
        //             break;
        //         case 2:
        //             foreach (var indicator in _indicator)
        //             {
        //                 indicator.SetActive(false);
        //             }
        //             _indicator[val-1].SetActive(true);
        //             break;
        //         case 3:
        //             foreach (var indicator in _indicator)
        //             {
        //                 indicator.SetActive(false);
        //             }
        //             _indicator[val-1].SetActive(true);
        //             break;
        //         case 4:
        //             foreach (var indicator in _indicator)
        //             {
        //                 indicator.SetActive(false);
        //             }
        //             _indicator[val-1].SetActive(true);
        //             break;
        //     }
        // }
    }
}


