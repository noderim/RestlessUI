using UnityEngine;
using UnityEngine.UI;
using RestlessEngine.Diagnostics;
using AudioSettings = RestlessEngine.Application.Settings.AudioSettings;

namespace RestlessUI.Components
{
    public partial class SettingsWindowManager
    {
        [System.Serializable]
        public class AudioSettingsBinder : IUIDataBinder<AudioSettings>, IValidable
        {
            public SettingsWindowManager manager;
            [Header("Audio Settings")]
            public Toggle MuteAllToggle; // Reference to the mute all toggle
            public Slider MasterVolumeSlider; // Reference to the master volume slider
            [Space(10)]
            public Toggle MuteMusicToggle; // Reference to the mute music toggle
            public Slider MusicVolumeSlider; // Reference to the music volume slider
            [Space(10)]
            public Slider SFXVolumeSlider; // Reference to the SFX volume slider
            public Slider AmbientVolumeSlider; // Reference to the ambient volume slider
            public Slider UIVolumeSlider; // Reference to the UI volume slider
            [Space(10)]
            public Toggle MuteWhenInBackgroundToggle; // Reference to the mute when in background toggle
            public AudioSettings BuildObject()
            {
                return new AudioSettings
                {
                    MuteAll = MuteAllToggle.isOn,
                    MasterVolume = MasterVolumeSlider.value,
                    MuteMusic = MuteMusicToggle.isOn,
                    MusicVolume = MusicVolumeSlider.value,
                    SFXVolume = SFXVolumeSlider.value,
                    AmbientVolume = AmbientVolumeSlider.value,
                    UIVolume = UIVolumeSlider.value,
                    MuteWhenInBackground = MuteWhenInBackgroundToggle.isOn
                };
            }

            public void RefreshUI(AudioSettings data)
            {
                MuteAllToggle.isOn = data.MuteAll;
                MasterVolumeSlider.value = data.MasterVolume;
                MuteMusicToggle.isOn = data.MuteMusic;
                MusicVolumeSlider.value = data.MusicVolume;
                SFXVolumeSlider.value = data.SFXVolume;
                AmbientVolumeSlider.value = data.AmbientVolume;
                UIVolumeSlider.value = data.UIVolume;
                MuteWhenInBackgroundToggle.isOn = data.MuteWhenInBackground;
            }

            public void RegisterEvents()
            {
                // Register Settings Window SettingsChanged event with value 1 - meaning Audio Settings
                MuteAllToggle.onValueChanged.AddListener((value) => manager.SettingsChanged(1));
                MasterVolumeSlider.onValueChanged.AddListener((value) => manager.SettingsChanged(1));

                MuteMusicToggle.onValueChanged.AddListener((value) => manager.SettingsChanged(1));
                MusicVolumeSlider.onValueChanged.AddListener((value) => manager.SettingsChanged(1));

                SFXVolumeSlider.onValueChanged.AddListener((value) => manager.SettingsChanged(1));
                AmbientVolumeSlider.onValueChanged.AddListener((value) => manager.SettingsChanged(1));
                UIVolumeSlider.onValueChanged.AddListener((value) => manager.SettingsChanged(1));

                MuteWhenInBackgroundToggle.onValueChanged.AddListener((value) => manager.SettingsChanged(1));
            }

            public bool SelfValidation()
            {
                bool validate = true;

                if (manager == null)
                {
                    manager = FindAnyObjectByType<SettingsWindowManager>();
                }

                validate &= ValidateUtility.ValidateCondition(manager != null, "SettingsWindowManager not found in the scene.");

                validate &= ValidateUtility.ValidateCondition(MuteAllToggle != null, "MuteAllToggle is null.");
                validate &= ValidateUtility.ValidateCondition(MasterVolumeSlider != null, "MasterVolumeSlider is null.");
                validate &= ValidateUtility.ValidateCondition(MuteMusicToggle != null, "MuteMusicToggle is null.");
                validate &= ValidateUtility.ValidateCondition(MusicVolumeSlider != null, "MusicVolumeSlider is null.");
                validate &= ValidateUtility.ValidateCondition(SFXVolumeSlider != null, "SFXVolumeSlider is null.");
                validate &= ValidateUtility.ValidateCondition(AmbientVolumeSlider != null, "AmbientVolumeSlider is null.");
                validate &= ValidateUtility.ValidateCondition(UIVolumeSlider != null, "UIVolumeSlider is null.");
                validate &= ValidateUtility.ValidateCondition(MuteWhenInBackgroundToggle != null, "MuteWhenInBackgroundToggle is null.");

                return validate;
            }
        }
    }
}
