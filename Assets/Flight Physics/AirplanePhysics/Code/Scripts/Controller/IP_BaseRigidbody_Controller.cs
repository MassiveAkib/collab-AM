using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IndiePixel
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class IP_BaseRigidbody_Controller : MonoBehaviour 
    {

        #region
        protected Rigidbody rb;
        protected AudioSource aSource;
        #endregion





        #region Builtin Methods
    	// Use this for initialization
    	public virtual void Start () 
        {
            rb = GetComponent<Rigidbody>();	
            aSource = GetComponent<AudioSource>();
            if(aSource)
            {
                aSource.playOnAwake = false;
            }
    	}
    	
    	// Update is called once per frame
    	void FixedUpdate () 
        {
            if(rb)
            {
                HandlePhysics();
            }
    	}
        #endregion





        #region Custom Methods
        protected virtual void HandlePhysics(){}
        #endregion
    }
}
