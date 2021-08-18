using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace IndiePixel
{
    [CustomEditor(typeof(IP_Heli_Engine))]
    public class IP_HeliEngine_Editor : Editor
    {
        #region Variables
        private IP_Heli_Engine targetEngine;
        #endregion

        #region Builtin Methods
        private void OnEnable()
        {
            targetEngine = (IP_Heli_Engine)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Engine Stats:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("RPM's: " + targetEngine.CurrentRPM);
            EditorGUILayout.LabelField("HP: " + targetEngine.CurrentHP);

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            Repaint();
        }
        #endregion
    }
}
