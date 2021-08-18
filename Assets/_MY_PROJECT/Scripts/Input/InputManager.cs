using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MassiveStar
{
    public class InputManager : MonoBehaviour
    {
        #region Singleton

        public static InputManager singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }

        #endregion
        
        
        #region Variable

        [Header("REFFERENCES :")]
        [SerializeField] private FixedJoystick m_fixedJoystick;
        [SerializeField] private FixedTouchField m_fixedTouchField;

        private Vector2 _lookInput;
        private float _cameraPitch;

        private PlayerController m_playerController;

        #endregion


        #region Unity Method

        private void Start()
        {
            m_playerController = PlayerController.singleton;
        }

        private void Update()
        {
            SetupPlayerMovementInput();
            SetupCameraMovementInput();
        }

        #endregion



        #region Public Method

        public void SetupPlayerMovementInput()
        {
            if (m_playerController == null || m_fixedJoystick == null) return;

            m_playerController.horizontal = m_fixedJoystick.Horizontal;
            m_playerController.vertical = m_fixedJoystick.Vertical;

        }
        
        public void SetupCameraMovementInput()
        {
            if (m_playerController == null || m_fixedTouchField == null) return;

            m_playerController.mouseX = m_fixedTouchField.TouchDist.x;
            m_playerController.mouseY = m_fixedTouchField.TouchDist.y;
        }

        #endregion



    }
}


