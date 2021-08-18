using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FXnRXn.PlaneCrash
{
    public class InputPlaneCrash : MonoBehaviour
    {
        #region Singleton

        public static InputPlaneCrash singleton;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
        }

        #endregion


        private bool rightPressed, leftPressed, topPressed, downPressed;

        public float horizontalValue;
        public float verticalValue;
        public float moveSpeed = 1f;
        public float getBackSpeed = 1f;
        
        
        
        #region Button Function

        public void RightButton_Down()
        {
            rightPressed = true;
        }
 
        public void RightButton_Up()
        {
            rightPressed = false;
        }
 
        public void LeftButton_Down()
        {
            leftPressed = true;
        }
 
        public void LeftButton_Up()
        {
            leftPressed = false;
        }
        //
        public void TopButton_Down()
        {
            topPressed = true;
        }
 
        public void TopButton_Up()
        {
            topPressed = false;
        }
 
        public void DownButton_Down()
        {
            downPressed = true;
        }
 
        public void DownButton_Up()
        {
            downPressed = false;
        }
        #endregion
        
        
        void Update()
        {
            bool noInputH = false;
            bool noInputV = false;
 
            // detecting the direction which value shoud be going
            int dirH = 0;
            if (rightPressed && leftPressed) // both directions
                dirH = 0;
            else if (rightPressed) // only right
                dirH = 1;
            else if (leftPressed) // only left
                dirH = -1;
            else // no input at all. force must be lerp into zero
                noInputH = true;
            
            // detecting the direction which value shoud be going
            int dirV = 0;
            if (topPressed && downPressed) // both directions
                dirV = 0;
            else if (downPressed) // only Up
                dirV = 1;
            else if (topPressed) // only Down
                dirV = -1;
            else // no input at all. force must be lerp into zero
                noInputV = true;
 
            // Left and right
            if (noInputH)
            {
                // lerping force into zero if the force is greater than a threshold (0.01)
                if (Mathf.Abs(horizontalValue) >= 0.01f)
                {
                    int opositeDir = (horizontalValue > 0) ? -1 : 1;
                    horizontalValue += Time.deltaTime * getBackSpeed * opositeDir;
                }
                else
                    horizontalValue = 0;
            }
            else
            {
                // increase force towards desired direction
                horizontalValue += Time.deltaTime * dirH * moveSpeed;
                horizontalValue = Mathf.Clamp(horizontalValue, -1, 1);
            }
            
            
            // Top and Down
            
            if (noInputV)
            {
                // lerping force into zero if the force is greater than a threshold (0.01)
                if (Mathf.Abs(verticalValue) >= 0.01f)
                {
                    int opositeDir = (verticalValue > 0) ? -1 : 1;
                    verticalValue += Time.deltaTime * getBackSpeed * opositeDir;
                }
                else
                    verticalValue = 0;
            }
            else
            {
                // increase force towards desired direction
                verticalValue += Time.deltaTime * dirV * moveSpeed;
                verticalValue = Mathf.Clamp(verticalValue, -1, 1);
            }

        }
        
        

    }// END CLASS
}


