using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace IndiePixel
{
    [CustomEditor(typeof(IP_Airplane_Controller))]
    public class IP_AirplaneController_Editor : Editor 
    {
        #region Variables
        private IP_Airplane_Controller targetController;
        #endregion


        #region Builtin Metods
        void OnEnable()
        {
            targetController = (IP_Airplane_Controller)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            GUILayout.Space(15);
            if(GUILayout.Button("Get Airplane Components", GUILayout.Height(35)))
            {
                //Find All Engines
                targetController.engines.Clear();
                targetController.engines = FindAllEngines().ToList<IP_Airplane_Engine>();

                //Find All Wheels
                targetController.wheels.Clear();
                targetController.wheels = FindAllWheels().ToList<IP_Airplane_Wheel>();

                //Find All Control Surfaces
                targetController.controlSurfaces.Clear();
                targetController.controlSurfaces = FindAllControlSurfaces().ToList<IP_Airplane_ControlSurface>();
            }

            if(GUILayout.Button("Create Airplane Preset", GUILayout.Height(35)))
            {
                string filePath = EditorUtility.SaveFilePanel("Save Airplane Preset", "Assets", "New_Airplane_Preset", "asset");
                SaveAirplanePreset(filePath);
            }
            GUILayout.Space(15);

        }
        #endregion



        #region Custom Methods
        IP_Airplane_Engine[] FindAllEngines()
        {
            IP_Airplane_Engine[] engines = new IP_Airplane_Engine[0];
            if(targetController)
            {
                engines = targetController.transform.GetComponentsInChildren<IP_Airplane_Engine>(true);
            }

            return engines;
        }

        IP_Airplane_Wheel[] FindAllWheels()
        {
            IP_Airplane_Wheel[] wheels = new IP_Airplane_Wheel[0];
            if(targetController)
            {
                wheels = targetController.transform.GetComponentsInChildren<IP_Airplane_Wheel>(true);
            }

            return wheels;
        }

        IP_Airplane_ControlSurface[] FindAllControlSurfaces()
        {
            IP_Airplane_ControlSurface[] controlSurfaces = new IP_Airplane_ControlSurface[0];
            if(targetController)
            {
                controlSurfaces = targetController.transform.GetComponentsInChildren<IP_Airplane_ControlSurface>(true);
            }

            return controlSurfaces;
        }

        void SaveAirplanePreset(string aPath)
        {
            if(targetController && !string.IsNullOrEmpty(aPath))
            {
                string appPath = Application.dataPath;

                string finalPath = "Assets" + aPath.Substring(appPath.Length);
//                Debug.Log(finalPath);

                //Create new Preset
                IP_Airplane_Preset newPreset = ScriptableObject.CreateInstance<IP_Airplane_Preset>();
                newPreset.airplaneWeight = targetController.airplaneWeight;
                if(targetController.centerOfGravity)
                {
                    newPreset.cogPosition = targetController.centerOfGravity.localPosition;
                }

                if(targetController.charactistics)
                {
                    newPreset.dragFactor = targetController.charactistics.dragFactor;
                    newPreset.flapDragFactor = targetController.charactistics.flapDragFactor;
                    newPreset.maxMPH = targetController.charactistics.maxMPH;
                    newPreset.rbLerpSpeed = targetController.charactistics.rbLerpSpeed;
                    newPreset.liftCurve = targetController.charactistics.liftCurve;
                    newPreset.maxLiftPower = targetController.charactistics.maxLiftPower;
                    newPreset.pitchSpeed = targetController.charactistics.pitchSpeed;
                    newPreset.rollSpeed = targetController.charactistics.rollSpeed;
                    newPreset.yawSpeed = targetController.charactistics.yawSpeed;
                }

                //Create Final Preset
                AssetDatabase.CreateAsset(newPreset, finalPath);
            }
        }
        #endregion
    }
}
