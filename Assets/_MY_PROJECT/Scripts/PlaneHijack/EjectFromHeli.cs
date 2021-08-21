using System.Collections;
using System.Collections.Generic;
using FXnRXn.PlaneHijack;
using UnityEngine;

public class EjectFromHeli : MonoBehaviour
{
    public GameObject WinScreen;
    public string _heliTag;
    public GameObject block;
    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(_heliTag))
        {
            if (other.gameObject.tag == _heliTag)
            {
                HijackMissionManager.singleton.PlayerExitHeli(true);
                WinScreen.SetActive(true);
                block.SetActive(true);
                Time.timeScale = 0;
            }
        }
            
    }
    
    public void NextScene()
    {
        Time.timeScale = 1;
        WinScreen.SetActive(false);
    }
}
