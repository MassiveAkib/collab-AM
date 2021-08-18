using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MassiveStar
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region Variable
        
        private Animator m_playerAnimator;
        private float _velocityZ = 0.0f;
        private float _velocityX = 0.0f;
        private float _acceleration = 2f;
        private float _deceleration = 2f;

        private PlayerController m_playerController;

        #endregion

        #region Unity Method

        private void Start()
        {
            m_playerAnimator = GetComponentInChildren<Animator>();
            m_playerController = PlayerController.singleton;
        }

        private void Update()
        {
            
            // Forward Pressed
            if (m_playerController.horizontal > 0.1f)
            {
                _velocityZ += Time.deltaTime * _acceleration;
            }
            
            // Backward Pressed
            if (m_playerController.horizontal < -0.1f)
            {
                _velocityZ -= Time.deltaTime * _acceleration;
            }
            
            //Right Pressed
            if (m_playerController.vertical > 0.1f)
            {
                _velocityX += Time.deltaTime * _acceleration;
            }
            
            //Left Pressed
            if (m_playerController.vertical < -0.1f)
            {
                _velocityX -= Time.deltaTime * _acceleration;
            }
            
            // Decreased forward and backward
            if (m_playerController.horizontal < .1f && _velocityZ > 0.0f)
            {
                _velocityZ -= Time.deltaTime * _deceleration;
            }
            if (m_playerController.horizontal > -0.1f && _velocityZ < 0.0f)
            {
                _velocityZ += Time.deltaTime * _deceleration;
            }
            if (m_playerController.horizontal == 0.0f)
            {
                _velocityZ = 0f;
            }
            
            // Decreased forward and backward
            if (m_playerController.vertical < .1f && _velocityX > 0.0f)
            {
                _velocityX -= Time.deltaTime * _deceleration;
            }
            if (m_playerController.vertical > -0.1f && _velocityX < 0.0f)
            {
                _velocityX += Time.deltaTime * _deceleration;
            }
            if (m_playerController.vertical == 0.0f)
            {
                _velocityX = 0f;
            }
            
            m_playerAnimator.SetFloat("VelocityX", _velocityX);
            m_playerAnimator.SetFloat("VelocityZ", _velocityZ);
        }
        
        

        #endregion

        
    }
}


