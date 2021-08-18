using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FXnRXn.PlaneHijack.Cinematic
{
    public class LoadSceneWithName : MonoBehaviour
    {


        public void SceneToLoad(string scene)
        {
            SceneManager.LoadScene(scene);
        }

    }
}

