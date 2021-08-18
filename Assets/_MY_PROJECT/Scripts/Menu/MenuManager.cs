using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{


    public void LoadScene(string scene)
    {
        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
    }


    public void OnQuit()
    {
        Application.Quit();
    }
    
    
}
