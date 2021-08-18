using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IndiePixel
{
    public class IP_InputControllerHeli : MonoBehaviour
    {
        #region Singleton

        public static IP_InputControllerHeli singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }

        #endregion

        #region Variable

        private bool throttleUpPressed, throttleDownPressed, collectiveUpPressed, collectiveDownPressed,
            padalUpPressed, padalDownPressed;
        
        [HideInInspector]
        public float throttleVal, collectiveVal, padalVal;

        #endregion
        
        #region Button Function

        public void ThrottleUpButton_Down()
        {
            throttleUpPressed = true;
        }
 
        public void ThrottleUpButton_Up()
        {
            throttleUpPressed = false;
        }
 
        public void ThrottleDownButton_Down()
        {
            throttleDownPressed = true;
        }
 
        public void ThrottleDownButton_Up()
        {
            throttleDownPressed = false;
        }
        
        //=================   
        public void CollectiveUpButton_Down()
        {
            collectiveUpPressed = true;
        }
 
        public void CollectiveUpButton_Up()
        {
            collectiveUpPressed = false;
        }
 
        public void CollectiveDownButton_Down()
        {
            collectiveDownPressed = true;
        }
 
        public void CollectiveDownButton_Up()
        {
            collectiveDownPressed = false;
        }
        
        
        //=================   
        public void PadalUpButton_Down()
        {
            padalUpPressed = true;
        }
 
        public void PadalUpButton_Up()
        {
            padalUpPressed = false;
        }
 
        public void PadalDownButton_Down()
        {
            padalDownPressed = true;
        }
 
        public void PadalDownButton_Up()
        {
            padalDownPressed = false;
        }

        #endregion
        
        
        #region Builtin Methods
        private void Update()
        {
            
            #region Throttle

            bool noBoolThrottle = false;
            int dirT = 0;
            if (throttleUpPressed && throttleDownPressed) 
                dirT = 0;
            else if (throttleDownPressed) 
                dirT = -1;
            else if (throttleUpPressed) 
                dirT = 1;
            else 
                noBoolThrottle = true;
            
            
            if (noBoolThrottle)
            {
                // lerping force into zero if the force is greater than a threshold (0.01)
                if (Mathf.Abs(throttleVal) >= 0.01f)
                {
                    int opositeDir = (throttleVal > 0) ? -1 : 1;
                    throttleVal += Time.deltaTime * 2f * opositeDir;
                }
                else
                {
                    throttleVal = 0;
                }
                    
            }
            else
            {
                // increase force towards desired direction
                throttleVal += Time.deltaTime * dirT * 2f;
                throttleVal = Mathf.Clamp(throttleVal, -1, 1);
            }
            
            #endregion


            #region Collective
            bool noboolcollective = false;
            int dirColl = 0;
            if (collectiveUpPressed && collectiveDownPressed) 
                dirColl = 0;
            else if (collectiveDownPressed) 
                dirColl = 1;
            else if (collectiveUpPressed) 
                dirColl = -1;
            else 
                noboolcollective = true;
            
            
            if (noboolcollective)
            {
                // lerping force into zero if the force is greater than a threshold (0.01)
                if (Mathf.Abs(collectiveVal) >= 0.01f)
                {
                    int opositeDir = (collectiveVal > 0) ? -1 : 1;
                    collectiveVal += Time.deltaTime * 2f * opositeDir;
                }
                else
                    collectiveVal = 0;
            }
            else
            {
                // increase force towards desired direction
                collectiveVal += Time.deltaTime * dirColl * 2f;
                collectiveVal = Mathf.Clamp(collectiveVal, -1, 1);
            }
            #endregion
            
            
            #region Padal
            bool noboolpadal = false;
            int dirP = 0;
            if (padalUpPressed && padalDownPressed) 
                dirP = 0;
            else if (padalDownPressed) 
                dirP = -1;
            else if (padalUpPressed) 
                dirP = 1;
            else 
                noboolpadal = true;
            
            
            if (noboolpadal)
            {
                // lerping force into zero if the force is greater than a threshold (0.01)
                if (Mathf.Abs(padalVal) >= 0.01f)
                {
                    int opositeDir = (padalVal > 0) ? -1 : 1;
                    padalVal += Time.deltaTime * 2f * opositeDir;
                }
                else
                    padalVal = 0;
            }
            else
            {
                // increase force towards desired direction
                padalVal += Time.deltaTime * dirP * 2f;
                padalVal = Mathf.Clamp(padalVal, -1, 1);
            }
            #endregion
            
            
        }
        
        
        #endregion
        

    }
}

