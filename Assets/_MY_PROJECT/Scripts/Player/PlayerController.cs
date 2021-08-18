using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MassiveStar
{
    public class PlayerController : MonoBehaviour
    {
        #region Singleton

        public static PlayerController singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }

        #endregion


        #region Variable

        [Header("Setting :")] 
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _cameraSpeed;

        [Header("Camera :")] 
        [SerializeField] private Transform _pivotTrans;
        [SerializeField] private Transform _cameraTrans;

        public bool _isUnderwater;

        [HideInInspector] public float horizontal;
        [HideInInspector] public float vertical;
        [HideInInspector] public float mouseX;
        [HideInInspector] public float mouseY;

        private CharacterController c;
        private float _lookAngle;
        private float _tiltAngle;
        

        #endregion


        #region Unity Method

        private void Start()
        {
            c = GetComponent<CharacterController>();
            _isUnderwater = false;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (!_isUnderwater)
            {
                HandleMovement(delta);
            }
            
            HandleCameraRotation(delta);
        }

        #endregion

        #region Private Method

        private void HandleMovement(float delta)
        {
            if(c == null) return;

            Vector3 direction = transform.forward * vertical;
            direction += transform.right * horizontal;
            direction.Normalize();

            RaycastHit hit;
            Physics.SphereCast(transform.position, c.radius, Vector3.down, out hit, c.height / 2f, Physics.AllLayers,
                QueryTriggerInteraction.Ignore);

            Vector3 desiredMove = Vector3.ProjectOnPlane(direction, hit.normal).normalized;
            if (!c.isGrounded)
            {
                desiredMove += Physics.gravity;
            }

            c.Move(desiredMove * (delta * _movementSpeed));

        }

        private void HandleCameraRotation(float delta)
        {
            _lookAngle += mouseX * (delta * _cameraSpeed);
            Vector3 camEulers = Vector3.zero;
            camEulers.y = _lookAngle;
            transform.eulerAngles = camEulers;

            _tiltAngle -= mouseY * (delta * _cameraSpeed);
            _tiltAngle = Mathf.Clamp(_tiltAngle,-45f, 55f);
            
            Vector3 tiltEuler = Vector3.zero;
            tiltEuler.x = _tiltAngle;
            _pivotTrans.localEulerAngles = tiltEuler;
        }

        #endregion
        

    }
}

