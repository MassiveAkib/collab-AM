using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IndiePixel
{
    public class IP_Rotor_Blur : MonoBehaviour, IP_IHeliRotor
    {
        #region Varaiables
        [Header("Rotor Blur Propeties")]
        public float maxDps = 1000f;
        public List<GameObject> blades = new List<GameObject>();
        public GameObject blurGeo;

        public List<Texture2D> blurTextures = new List<Texture2D>();
        public Material blurMat;
        #endregion


        #region Interface Methods
        public void UpdateRotor(float dps, IP_Input_Controller input)
        {
            //Debug.Log("Blurring Main Rotor");
            float normalizedDPS = Mathf.InverseLerp(0f, maxDps, dps);
            int blurTexID = Mathf.FloorToInt(normalizedDPS * blurTextures.Count - 1);
            blurTexID = Mathf.Clamp(blurTexID, 0, blurTextures.Count - 1);

            //check to see if we have Blur Textures and A blur Mat
            if(blurMat && blurTextures.Count > 0)
            {
                blurMat.SetTexture("_MainTex", blurTextures[blurTexID]);
            }

            //Handle the Geo Blades Visibility
            if(blurTexID > 2 && blades.Count > 0)
            {
                HandleGeoBladeViz(false);
            }
            else
            {
                HandleGeoBladeViz(true);
            }

            //Handle the Blur Geo Visibility
            if(blurTexID > 1)
            {
                HandleBlurGeo(true);
            }
            else
            {
                HandleBlurGeo(false);
            }
        }
        #endregion

        void HandleGeoBladeViz(bool viz)
        {
            foreach (var blade in blades)
            {
                if(blade.activeSelf != viz)
                {
                    blade.SetActive(viz);
                }
            }
        }

        void HandleBlurGeo(bool viz)
        {
            if(blurGeo && blurGeo.activeSelf != viz)
            {
                blurGeo.SetActive(viz);
            }
        }
    }
}
