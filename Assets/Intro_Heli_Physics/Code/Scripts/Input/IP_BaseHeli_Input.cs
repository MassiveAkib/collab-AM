using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IndiePixel
{
    public class IP_BaseHeli_Input : MonoBehaviour
    {
        #region Variables
        [Header("Base Input Properties")]
        protected float vertical = 0f;
        protected float horizontal = 0f;
        #endregion


        #region BuiltIn Methods
        protected virtual void Update()
        {
            HandleInputs();
        }
        #endregion


        #region Custom Methods
        protected virtual void HandleInputs()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }
        #endregion
    }
}
