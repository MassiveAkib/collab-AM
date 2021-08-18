using System.Collections;
using System.Collections.Generic;
using MassiveStar;
using UnityEngine;

namespace FXnRXn.PlaneHijack
{
    public class InputPlaneHijack : MonoBehaviour
    {
        #region Singleton

        public static InputPlaneHijack singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }

        #endregion
        
        
        [Header("REFFERENCES :")]
        [SerializeField] private FixedJoystick m_fixedJoystick;
        [SerializeField] private FixedTouchField m_fixedTouchField;
        
        [Header("Player controller")]
        [SerializeField] private FixedJoystick p_fixedJoystick;
        [SerializeField] private FixedTouchField p_fixedTouchField;
        
        
        [HideInInspector] public float horizontal;
        [HideInInspector] public float vertical;
        [HideInInspector] public float mouseX;
        [HideInInspector] public float mouseY;
        [HideInInspector] public float lookAngle;
        [HideInInspector] public float tiltAngle;
        
        #region Unity Method

        private void Start()
        {
            
        }

        private void Update()
        {
            SetupPlayerMovementInput();
            SetupCameraMovementInput();

            if (PlayerController.singleton != null)
            {
                PlayerController.singleton.horizontal = p_fixedJoystick.Horizontal;
                PlayerController.singleton.vertical = p_fixedJoystick.Vertical;

                PlayerController.singleton.mouseX = p_fixedTouchField.TouchDist.x;
                PlayerController.singleton.mouseY = p_fixedTouchField.TouchDist.y;
            }
        }

        #endregion
        
        
        #region Public Method

        public void SetupPlayerMovementInput()
        {
            if ( m_fixedJoystick == null) return;

            horizontal = m_fixedJoystick.Horizontal;
            vertical = m_fixedJoystick.Vertical;

        }
        
        public void SetupCameraMovementInput()
        {
            if (m_fixedTouchField == null) return;

            mouseX = m_fixedTouchField.TouchDist.x;
            mouseY = m_fixedTouchField.TouchDist.y;
        }

        #endregion
    }
}


