using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Lean.Gui;
using RestlessEngine.Diagnostics;
using RestlessLib.Attributes;
using RestlessEngine.Application.Settings;
using AudioSettings = RestlessEngine.Application.Settings.AudioSettings;

namespace RestlessUI.Components
{
    public class SettingsWindowManager : MonoBehaviour, IValidable, ISetupable
    {
        public UnityEvent OnSettingsChanged;
        [SerializeField]
        public AudioSettingsBinder audioSettingsBinder; // Reference to the audio settings binder
        [SerializeField]
        public GraphicsSettingsBinder graphicsSettingsBinder; // Reference to the graphics settings binder
        [SerializeField]
        public ControllsSettingsBinder controllsSettingsBinder; // Reference to the controlls settings binder
        [SerializeField]
        public InterfaceSettingsBinder interfaceSettingsBinder; // Reference to the interface settings binder
        [SerializeField]
        public GameplaySettingsBinder gameplaySettingsBinder; // Reference to the gameplay settings binder

        [Header("Buttons")]
        public LeanButton RevertButtonRef; // Reference to the revert button
        public LeanButton ApplyButtonRef; // Reference to the apply button
        public LeanButton ResetToDefaultButtonRef; // Reference to the reset to default button

        [Space(10)]
        [Header("Settings Refrence")]
        [ReadOnly]
        public SettingsData CurrentSettingsData;
        [ReadOnly]
        public SettingsManager settingsSystem; // Reference to the Settings System
        [ReadOnly]
        public LeanWindow window;

        void Start()
        {
            Setup();
        }
        public void ApplyButton()
        {
            RevertButtonRef.interactable = false;
            ApplyButtonRef.interactable = false;
            settingsSystem.ApplySettings();
            RefreshAll();
            settingsSystem.SaveSettings(); // Save the settings after applying them
        }
        public void RevertButton()
        {
            RevertButtonRef.interactable = false;
            ApplyButtonRef.interactable = false;
            ResetToDefaultButtonRef.interactable = true;
            settingsSystem.RevertSettings();
            RefreshAll();
        }
        public void ResetToDefaultButton()
        {
            RevertButtonRef.interactable = true;
            ApplyButtonRef.interactable = true;
            ResetToDefaultButtonRef.interactable = false;
            settingsSystem.ResetToDefault();
            RefreshAll();
        }
        public void RegisterBindings()
        {
            gameplaySettingsBinder.RegisterEvents();
            audioSettingsBinder.RegisterEvents();
            graphicsSettingsBinder.RegisterEvents();
            controllsSettingsBinder.RegisterEvents();
            interfaceSettingsBinder.RegisterEvents();
        }
        public void OnWindowOpen()
        {
            RevertButtonRef.interactable = false;
            ResetToDefaultButtonRef.interactable = true;
            ApplyButtonRef.interactable = false;
            RefreshAll();
        }
        private enum SettingsType
        {
            Gameplay = 1,
            Audio = 2,
            Graphics = 3,
            Controls = 4,
            Interface = 5
        }
        public void SettingsChanged(int SettingsTypeIndex)
        {
            RevertButtonRef.interactable = true;
            ApplyButtonRef.interactable = true;
            ResetToDefaultButtonRef.interactable = true;
            OnSettingsChanged.Invoke();

            switch ((SettingsType)SettingsTypeIndex)
            {
                case SettingsType.Gameplay:
                    //IMPLEMENT
                    return;
                case SettingsType.Audio:
                    settingsSystem.ChangeSettings(audioSettingsBinder.BuildObject());
                    return;
                case SettingsType.Graphics:
                    settingsSystem.ChangeSettings(graphicsSettingsBinder.BuildObject());
                    return;
                case SettingsType.Controls:
                    //IMPLEMENT
                    return;
                case SettingsType.Interface:
                    settingsSystem.ChangeSettings(interfaceSettingsBinder.BuildObject());
                    return;
            }
        }

        public void RefreshAll()
        {
            CurrentSettingsData = settingsSystem.GetCurrentSettings(); // Get the current settings data from the SettingsSystem

            gameplaySettingsBinder.RefreshUI(CurrentSettingsData.gameplaySettings);
            audioSettingsBinder.RefreshUI(CurrentSettingsData.audioSettings);
            graphicsSettingsBinder.RefreshUI(CurrentSettingsData.graphicsSettings);
            controllsSettingsBinder.RefreshUI(CurrentSettingsData.controlsSettings);
            interfaceSettingsBinder.RefreshUI(CurrentSettingsData.interfaceSettings);
        }
        public bool SelfValidation()
        {
            bool validate = true;

            validate &= ValidateUtility.Validate(gameplaySettingsBinder);
            validate &= ValidateUtility.Validate(audioSettingsBinder);
            validate &= ValidateUtility.Validate(graphicsSettingsBinder);
            validate &= ValidateUtility.Validate(controllsSettingsBinder);
            validate &= ValidateUtility.Validate(interfaceSettingsBinder);

            validate &= ValidateUtility.ValidateCondition(RevertButtonRef != null, "RevertButtonRef is null.");
            validate &= ValidateUtility.ValidateCondition(ApplyButtonRef != null, "ApplyButtonRef is null.");
            validate &= ValidateUtility.ValidateCondition(ResetToDefaultButtonRef != null, "ResetToDefaultButtonRef is null.");
            validate &= ValidateUtility.ValidateCondition(settingsSystem != null, "SettingsSystem is null.");
            validate &= ValidateUtility.ValidateCondition(window != null, "Window is null.");

            return validate;
        }
        public void Setup()
        {
            settingsSystem = SettingsManager.Instance;
            window = GetComponent<LeanWindow>();

            if (!ValidateUtility.Validate(this))
            {
                LogManager.LogError("SettingsWindowManager validation failed. Please check the console for more details.");
                return;
            }
            ;

            RefreshAll();
            RegisterBindings();
        }

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
        [System.Serializable]
        public class ControllsSettingsBinder : IUIDataBinder<ControlsSettings>, IValidable
        {
            public SettingsWindowManager manager;


            public ControlsSettings BuildObject()
            {
                return new ControlsSettings
                {

                };
            }

            public void RefreshUI(ControlsSettings data)
            {

            }

            public void RegisterEvents()
            {
                if (manager == null)
                {
                    manager = FindAnyObjectByType<SettingsWindowManager>();
                    if (manager == null)
                    {
                        LogManager.LogError("SettingsWindowManager not found in the scene.");
                        return;
                    }
                }
            }

            public bool SelfValidation()
            {
                bool validate = true;

                if (manager == null)
                {
                    manager = FindAnyObjectByType<SettingsWindowManager>();
                }

                validate &= ValidateUtility.ValidateCondition(manager != null, "SettingsWindowManager not found in the scene.");


                //To do validation
                //
                //

                return validate;
            }
        }

        [System.Serializable]
        public class InterfaceSettingsBinder : IUIDataBinder<InterfaceSettings>, IValidable
        {
            public SettingsWindowManager manager;
            public TMP_Dropdown LanguageDropdown; // Reference to the language dropdown

            public InterfaceSettings BuildObject()
            {
                return new InterfaceSettings
                {

                    language = (InterfaceSettings.Language)LanguageDropdown.value
                };
            }

            public void RefreshUI(InterfaceSettings data)
            {
                LanguageDropdown.value = (int)data.language;
            }

            public void RegisterEvents()
            {
                LanguageDropdown.onValueChanged.AddListener((value) => manager.SettingsChanged(4));
            }

            public bool SelfValidation()
            {
                bool validate = true;

                if (manager == null)
                {
                    manager = FindAnyObjectByType<SettingsWindowManager>();
                }

                validate &= ValidateUtility.ValidateCondition(manager != null, "SettingsWindowManager not found in the scene.");

                validate &= ValidateUtility.ValidateCondition(LanguageDropdown != null, "LanguageDropdown is null.");


                return validate;
            }
        }
        [System.Serializable]
        public class GameplaySettingsBinder : IUIDataBinder<GameplaySettings>, IValidable
        {
            public SettingsWindowManager manager;
            public GameplaySettings BuildObject()
            {
                return new GameplaySettings
                {

                };
            }

            public void RefreshUI(GameplaySettings data)
            {

            }

            public void RegisterEvents()
            {
                if (manager == null)
                {
                    manager = FindAnyObjectByType<SettingsWindowManager>();
                    if (manager == null)
                    {
                        LogManager.LogError("SettingsWindowManager not found in the scene.");
                        return;
                    }
                }
            }

            public bool SelfValidation()
            {
                bool validate = true;

                if (manager == null)
                {
                    manager = FindAnyObjectByType<SettingsWindowManager>();
                }

                validate &= ValidateUtility.ValidateCondition(manager != null, "SettingsWindowManager not found in the scene.");

                //To do validation
                //
                //

                return validate;
            }
        }
    }
}
