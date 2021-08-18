using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    public class IP_XboxHeli_Input : IP_KeyboardHeli_Input
    {
        #region Custom Methods
        protected override void HandleThrottle()
        {
            throttleInput = Input.GetAxis("XboxThrottleUp") + -Input.GetAxis("XboxThrottleDown");
        }

        protected override void HandleCollective()
        {
            collectiveInput = Input.GetAxis("XboxCollective");
        }

        protected override void HandleCyclic()
        {
            cyclicInput.y = Input.GetAxis("XboxCyclicVertical");
            cyclicInput.x = Input.GetAxis("XboxCyclicHorizontal");
        }

        protected override void HandlePedal()
        {
            pedalInput = Input.GetAxis("XboxPedal");
        }

        protected override void HandleCamButton()
        {
            camInput = Input.GetButtonDown("XboxCamBtn");
        }

        protected override void HandleFireButton()
        {
            fire = Input.GetButton("XboxFireBtn");
        }
        #endregion
    }
}
