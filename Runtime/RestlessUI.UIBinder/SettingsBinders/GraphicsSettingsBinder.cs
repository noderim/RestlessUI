using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RestlessEngine.Diagnostics;
using RestlessEngine.Application.Settings;

namespace RestlessUI.Components
{
    public partial class SettingsWindowManager
    {
        [System.Serializable]
        public class GraphicsSettingsBinder : IUIDataBinder<GraphicsSettings>, IValidable
        {
            public SettingsWindowManager manager;
            [Header("Graphics Settings")]
            public TMP_Dropdown QualityPresetDropdown; // Reference to the quality preset dropdown
            public TMP_Dropdown ResolutionDropdown; // Reference to the resolution dropdown
            public TMP_Dropdown DisplayModeDropdown; // Reference to the display mode dropdown
            public Toggle VSyncToggle; // Reference to the VSync toggle
            public TMP_Dropdown PreferredFPSDropdown; // Reference to the preferred FPS input field
            public Slider BrightnessSlider; // Reference to the brightness slider

            public GraphicsSettings BuildObject()
            {
                return new GraphicsSettings
                {
                    Preset = (QualityPreset)QualityPresetDropdown.value,
                    resolution = (ResolutionSetting)ResolutionDropdown.value,
                    ScreenModecreen = (FullScreenMode)DisplayModeDropdown.value,
                    VSync = VSyncToggle.isOn ? 1 : 0,
                    PreferredRefreshRate = int.Parse(PreferredFPSDropdown.options[PreferredFPSDropdown.value].text),
                    Brightness = BrightnessSlider.value
                };
            }

            public void RefreshUI(GraphicsSettings data)
            {
                QualityPresetDropdown.value = (int)data.Preset;
                ResolutionDropdown.value = (int)data.resolution;
                DisplayModeDropdown.value = (int)data.ScreenModecreen;
                VSyncToggle.isOn = data.VSync == 1;
                PreferredFPSDropdown.value = PreferredFPSDropdown.options.FindIndex(option => option.text == data.PreferredRefreshRate.ToString());
                BrightnessSlider.value = data.Brightness;
            }

            public void RegisterEvents()
            {
                // Register Settings Window SettingsChanged event with value 2 - meaning Graphics Settings
                QualityPresetDropdown.onValueChanged.AddListener((value) => manager.SettingsChanged(2));
                ResolutionDropdown.onValueChanged.AddListener((value) => manager.SettingsChanged(2));
                DisplayModeDropdown.onValueChanged.AddListener((value) => manager.SettingsChanged(2));
                VSyncToggle.onValueChanged.AddListener((value) => manager.SettingsChanged(2));
                PreferredFPSDropdown.onValueChanged.AddListener((value) => manager.SettingsChanged(2));
                BrightnessSlider.onValueChanged.AddListener((value) => manager.SettingsChanged(2));
            }

            public bool SelfValidation()
            {
                bool validate = true;

                if (manager == null)
                {
                    manager = FindAnyObjectByType<SettingsWindowManager>();
                }

                validate &= ValidateUtility.ValidateCondition(manager != null, "SettingsWindowManager not found in the scene.");

                validate &= ValidateUtility.ValidateCondition(QualityPresetDropdown != null, "QualityPresetDropdown is null.");
                validate &= ValidateUtility.ValidateCondition(ResolutionDropdown != null, "ResolutionDropdown is null.");
                validate &= ValidateUtility.ValidateCondition(DisplayModeDropdown != null, "DisplayModeDropdown is null.");
                validate &= ValidateUtility.ValidateCondition(VSyncToggle != null, "VSyncToggle is null.");
                validate &= ValidateUtility.ValidateCondition(PreferredFPSDropdown != null, "PreferredFPSDropdown is null.");
                validate &= ValidateUtility.ValidateCondition(BrightnessSlider != null, "BrightnessSlider is null.");

                return validate;
            }
        }
    }
}
