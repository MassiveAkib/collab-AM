using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IndiePixel
{
    public static class IP_Airplane_SetupTools 
    {
        public static void BuildDefaultAirplane(string aName)
        {
            //Create the root GO
            GameObject rootGO = new GameObject(aName, typeof(IP_Airplane_Controller), typeof(IP_BaseAirplane_Input));

            //Create the Center of Gravity
            GameObject cogGO = new GameObject("COG");
            cogGO.transform.SetParent(rootGO.transform, false);

            //Create the Base Components or Find them
            IP_BaseAirplane_Input input = rootGO.GetComponent<IP_BaseAirplane_Input>();
            IP_Airplane_Controller controller = rootGO.GetComponent<IP_Airplane_Controller>();
            IP_Airplane_Characteristics characteristics = rootGO.GetComponent<IP_Airplane_Characteristics>();

            //Setup the Airplane
            if(controller)
            {
                //Assing core Components
                controller.input = input;
                controller.charactistics = characteristics;
                controller.centerOfGravity = cogGO.transform;

                //Create Structure
                GameObject graphicsGrp = new GameObject("Graphics_GRP");
                GameObject collisionGrp = new GameObject("Collision_GRP");
                GameObject controlSurfaceGrp = new GameObject("ControlSurfaces_GRP");

                graphicsGrp.transform.SetParent(rootGO.transform, false);
                collisionGrp.transform.SetParent(rootGO.transform, false);
                controlSurfaceGrp.transform.SetParent(rootGO.transform, false);

                //Create First Engine
                GameObject engineGO = new GameObject("Engine", typeof(IP_Airplane_Engine));
                IP_Airplane_Engine engine = engineGO.GetComponent<IP_Airplane_Engine>();
                controller.engines.Add(engine);
                engineGO.transform.SetParent(rootGO.transform, false);

                //Create the base Airplane
                GameObject defaultAirplane = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/AirplanePhysics/Art/Objects/Airplanes/Indie-Pixel_Airplane/IndiePixel_Airplane.fbx", typeof(GameObject));
                if (defaultAirplane)
                {
                    GameObject.Instantiate(defaultAirplane, graphicsGrp.transform);
                }
            }

            //Select the Airplane Setup
            Selection.activeGameObject = rootGO;

        }
    }
}
