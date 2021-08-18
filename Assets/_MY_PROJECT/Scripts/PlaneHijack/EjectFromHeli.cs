using System.Collections;
using System.Collections.Generic;
using FXnRXn.PlaneHijack;
using UnityEngine;

public class EjectFromHeli : MonoBehaviour
{
    public string _heliTag;
    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(_heliTag))
        {
            if (other.gameObject.tag == _heliTag)
            {
                HijackMissionManager.singleton.PlayerExitHeli(true);
            }
        }
            
    }
}
