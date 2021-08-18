using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


namespace MassiveStar
{
    public class MissionManager : MonoBehaviour
    {
        #region MyRegion

        public static MissionManager singleton;

        private void Awake()
        {
            if (singleton == null)
                singleton = this;
        }

        #endregion
        
        

        [Header("Mission :")] 
        [SerializeField] private int totalMission;
        public List<CheckpointInfo> checkpointInfos = new List<CheckpointInfo>();
        
        [Header("UI Fader")]
        public UIFader UI_fader;
        
        
        private string sceneToLoad;
        
        
        
        private void Start()
        {
           //SelectMissionAtStart();
           //SelectMissionAtStart();
            //PlacementCheckpointToMission();

            //Debug.Log((int)Random.Range(1, 3));
        }

        // private void SelectMissionAtStart()
        // {
        //     GameGlobalData.checkPoint = (int)Random.Range(1, 4);
        //     sceneToLoad = GameGlobalData.GetSceneNameToLoad();
        // }
        //
        // private Vector3 AddCheckpointInfo()
        // {
        //     Vector3 posOfCheckpoint = Vector3.zero;
        //     if (GameGlobalData.checkPoint <= totalMission)
        //     {
        //         posOfCheckpoint = checkpointInfos[GameGlobalData.checkPoint - 1].checkpointTrans.localPosition;
        //     }
        //
        //     return posOfCheckpoint;
        //
        // }
        //
        //
        // private void PlacementCheckpointToMission()
        // {
        //     // Add Hud Display
        //     GameObject indicatorObject = FXnRXn.Utilities.ObjectPool.GetObject("IndicatorPoint") as GameObject;
        //     indicatorObject.SetActive(true);
        //     indicatorObject.transform.position = AddCheckpointInfo();
        //     
        //     // Add Collider script
        //     GameObject checkpointObj = null;
        //     checkpointObj = checkpointInfos[GameGlobalData.checkPoint - 1].checkpointTrans.gameObject;
        //     checkpointObj.AddComponent<PlayerColliderTrigger>();
        // }
        
        //Interface call
        public void OnPlayerCollide(string scene)
        {
            if (UI_fader != null)
            {
                UI_fader.Fade (UIFader.FADE.FadeOut, 1f, .1f);
            }//UI_fader.gameObject.SetActive (true);
            
            //Load scene
            this.Wait(2f,() =>
            {
                SceneManager.LoadScene(scene);
            });
        }
    }


    [System.Serializable]
    public class CheckpointInfo
    {
        public int id;
        public Transform checkpointTrans;
    }
}

