using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEditor;
using RestlessEngine.Application;

namespace RestlessUI.Utilities
{
    /// <summary>
    /// This class populates a TMP_Dropdown with the supported screen resolutions.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class ResolutionDropdownHelper : MonoBehaviour
    {
        public TMP_Dropdown resolutionDropdown;

        void Awake()
        {
            resolutionDropdown = GetComponent<TMP_Dropdown>();
        }

        void Start()
        {
            PopulateResolutionDropdown();
        }
        [ContextMenu("Populate Resolution Dropdown")]
        void PopulateResolutionDropdown()
        {
            if (resolutionDropdown == null)
                resolutionDropdown = GetComponent<TMP_Dropdown>();
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();

            foreach (Resolution resolution in SupportedResolutions.SupportedScreenResolutions)
            {
                options.Add($"{resolution.width} x {resolution.height}");
            }

            resolutionDropdown.AddOptions(options);
        }
#if UNITY_EDITOR
        [CustomEditor(typeof(ResolutionDropdownHelper))]
        public class ResolutionDropdownHelperEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                ResolutionDropdownHelper helper = (ResolutionDropdownHelper)target;

                if (GUILayout.Button("Fill Dropdown"))
                {
                    helper.PopulateResolutionDropdown();
                }
            }
        }
#endif
    }
}
