using UnityEngine;
using UnityEngine.Events;
using Lean.Gui;
using RestlessEngine.Diagnostics;
using RestlessLib.Attributes;
using RestlessEngine.Application.Settings;

namespace RestlessUI.Components
{
    public partial class SettingsWindowManager : MonoBehaviour, IValidable, ISetupable
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
    }
}
