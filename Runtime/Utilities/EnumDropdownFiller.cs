using System;
using System.Linq;
using TMPro;
using UnityEngine;
using RestlessLib.Attributes;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RestlessUI.Utilities
{
    /// <summary>
    /// Automatically fills a TMP_Dropdown with the names of an enum.
    /// You can assign the enum type in the inspector using the EnumTypeName string.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class EnumDropdownFiller : MonoBehaviour
    {
        [InfoBox("The full name of the enum type, e.g. 'MyNamespace.MyEnum' or just 'MyEnum' if global.")]
        public string EnumTypeName;

        [Tooltip("If true, clears existing dropdown options before adding enum names.")]
        public bool ClearExisting = true;

        [Tooltip("If true, automatically update when the component is enabled.")]
        public bool AutoFillOnEnable = true;

        public TMP_Dropdown _dropdown;

        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
        }

        private void OnEnable()
        {
            if (AutoFillOnEnable)
                TryFill();
        }

        [ContextMenu("Fill Dropdown with Enum Values")]
        public void TryFill()
        {
            if (string.IsNullOrEmpty(EnumTypeName))
            {
                Debug.LogWarning($"{nameof(EnumDropdownFiller)}: EnumTypeName is empty.");
                return;
            }

            Type enumType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.IsEnum && t.Name == EnumTypeName || t.FullName == EnumTypeName);

            if (enumType == null)
            {
                Debug.LogError($"{nameof(EnumDropdownFiller)}: Enum type '{EnumTypeName}' not found.");
                return;
            }

            string[] enumNames = Enum.GetNames(enumType);
            if (enumNames == null || enumNames.Length == 0)
            {
                Debug.LogWarning($"{nameof(EnumDropdownFiller)}: Enum '{EnumTypeName}' has no values.");
                return;
            }

            if (ClearExisting)
                _dropdown.ClearOptions();

            _dropdown.AddOptions(enumNames.ToList());
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(EnumDropdownFiller))]
    public class EnumDropdownFillerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EnumDropdownFiller filler = (EnumDropdownFiller)target;

            if (GUILayout.Button("Fill Dropdown"))
            {
                filler.TryFill();
            }
        }
    }
#endif
}
