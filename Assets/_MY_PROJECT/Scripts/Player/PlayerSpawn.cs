using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MassiveStar
{
    public class PlayerSpawn : MonoBehaviour
    {
        [Header("Player Prefab :")] 
        
        [SerializeField] private GameObject _playerPref;


        private void Awake()
        { 
            if(_playerPref == null) return;
            
            Instantiate(_playerPref, transform.position, Quaternion.identity);
        }
    }
}

