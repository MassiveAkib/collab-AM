using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel
{
    [RequireComponent(typeof(Rigidbody))]
    public class IP_Base_RBController : MonoBehaviour
    {
        #region Variables
        [Header("Base Properties")]
        public float weightInLbs = 10f;
        public Transform cog;


        const float lbsToKg = 0.454f;
        const float kgToLbs = 2.205f;

        protected Rigidbody rb;
        protected float weight;
        #endregion


        #region BuiltIn Methods
        // Use this for initialization
        public virtual void Start()
        {
            float finalKG = weightInLbs * lbsToKg;
            weight = finalKG;
            rb = GetComponent<Rigidbody>();
            if(rb)
            {
                rb.mass = weight;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (rb)
            {
                HandlePhysics();
            }
        }
        #endregion


        #region Custom Methods
        protected virtual void HandlePhysics() { }
        #endregion
    }
}
