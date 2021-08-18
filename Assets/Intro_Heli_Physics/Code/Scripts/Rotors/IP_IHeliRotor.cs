using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IndiePixel
{
    public interface IP_IHeliRotor
    {
        #region Methods
        void UpdateRotor(float dps, IP_Input_Controller input);
        #endregion  
    }
}
