using UnityEngine;
using System.Collections.Generic;
using RestlessLib.Attributes;
using Lean.Gui;
using RestlessEngine.Diagnostics;
using RestlessEngine.Application;
using RestlessEngine.GameSaves;

namespace RestlessUI.Components
{
    public class MainMenuHandler : MonoBehaviour, ISetupable, IValidable
    {
        [Header("Main Menu Buttons")]
        public bool FilterMenuButtons = true;
        public GameObject ContinueButtonObject;
        public GameObject NewGameButtonObject;
        public GameObject SaveGameButtonObject;
        public GameObject LoadGameButtonObject;
        public GameObject SettingsButtonObject;
        public GameObject QuitGameButtonObject;

        [Space(5)]
        public GameObject EraseGameButtonObject;

        [HorizontalLine]
        [Header("Prompt Data")]
        public PromptData ContinuePromptData;
        public PromptData NewGamePromptData;
        public PromptData SaveGamePromptData;
        public PromptData LoadGamePromptData;
        public PromptData SettingsDefaultPromptData;
        public PromptData ErasePromptData;
        public PromptData QuitGamePromptData;

        [Header("Auto Setup")]
        public bool UseAutoSetup = true;
        public Transform ButtonAutoRefrenceParent;
        [ReadOnly]
        public List<string> ButtonNames = new List<string>(){
        "Menu Button Continue",
        "Menu Button New Game",
        "Menu Button Save",
        "Menu Button Load",
        "Menu Button Settings",
        "Menu Button Quit",
    };
        private GameSavesPanelManager _gameSavesPanelManager;
        private SettingsWindowManager _settingsWindowManager;

        private void Start()
        {
            if (UseAutoSetup)
            {
                Setup();
            }
            UpdateMenuButtons();
        }

        public void Setup()
        {
            // Get Refrences
            if (ButtonAutoRefrenceParent == null)
            {
                ButtonAutoRefrenceParent = transform;
            }

            _gameSavesPanelManager = FindAnyObjectByType<GameSavesPanelManager>();
            _settingsWindowManager = FindAnyObjectByType<SettingsWindowManager>();

            ContinueButtonObject = ButtonAutoRefrenceParent.Find(ButtonNames[0]).gameObject;
            NewGameButtonObject = ButtonAutoRefrenceParent.Find(ButtonNames[1]).gameObject;
            SaveGameButtonObject = ButtonAutoRefrenceParent.Find(ButtonNames[2]).gameObject;
            LoadGameButtonObject = ButtonAutoRefrenceParent.Find(ButtonNames[3]).gameObject;
            SettingsButtonObject = ButtonAutoRefrenceParent.Find(ButtonNames[4]).gameObject;
            QuitGameButtonObject = ButtonAutoRefrenceParent.Find(ButtonNames[5]).gameObject;

            //Validate
            ValidateUtility.Validate(this);

            // Setup Buttons
            ContinueButtonObject.GetComponent<LeanButton>().OnClick.AddListener(ContinueGameButton);
            NewGameButtonObject.GetComponent<LeanButton>().OnClick.AddListener(NewGameButton);
            SaveGameButtonObject.GetComponent<LeanButton>().OnClick.AddListener(SaveGameButton);
            LoadGameButtonObject.GetComponent<LeanButton>().OnClick.AddListener(LoadGameButton);
            SettingsButtonObject.GetComponent<LeanButton>().OnClick.AddListener(SettingsButton);
            QuitGameButtonObject.GetComponent<LeanButton>().OnClick.AddListener(QuitGameButton);

            EraseGameButtonObject.GetComponent<LeanButton>().OnClick.AddListener(EraseGameButton);

        }

        [ContextMenu("Update Menu Buttons")]
        public void UpdateMenuButtons()
        {
            if (FilterMenuButtons)
            {
                AppState currentGameState = ApplicationManager.Instance.CurrentAppState;
                ContinueButtonObject.SetActive(currentGameState == AppState.MainMenu && GameSavefileSystem.Instance.HasSaveFiles);
                SaveGameButtonObject.SetActive(currentGameState == AppState.Game);
            }
        }
        public void ContinueGameButton()
        {
            if (GameSavefileSystem.Instance.HasSaveFiles)
            {
                ContinuePromptData.StaticAdditionalInfo = GameSavefileSystem.Instance.GetLastSaveInfo();
                PromptManager.Instance.ShowPrompt(ContinuePromptData/*, SubmitAction: GameManager.Instance.LoadGame*/);
            }
            else
            {
                LogManager.LogError("No save files found. Cannot continue game.");
            }
        }
        public void NewGameButton()
        {
            //PromptManager.Instance.ShowPrompt(NewGamePromptData, SubmitAction: GameManager.Instance.NewGame);
        }
        public void SaveGameButton()
        {
            if (_gameSavesPanelManager != null)
            {
                _gameSavesPanelManager.OpenWindow(false);
            }
            else
            {
                LogManager.LogError("GameSavesPanelManager not found. Cannot save game.");
            }
        }
        public void LoadGameButton()
        {
            if (_gameSavesPanelManager != null)
            {
                _gameSavesPanelManager.OpenWindow(true);
            }
            else
            {
                LogManager.LogError("GameSavesPanelManager not found. Cannot save game.");
            }
        }
        public void SettingsButton()
        {
            if (_settingsWindowManager != null)
            {
                _settingsWindowManager.window.TurnOn();
            }
            else
            {
                LogManager.LogError("SettingsWindowManager not found. Cannot open settings.");
            }
        }
        public void QuitGameButton()
        {
            PromptManager.Instance.ShowPrompt(QuitGamePromptData, SubmitAction: ApplicationManager.Instance.QuitGame);
        }
        public void EraseGameButton()
        {
            PromptManager.Instance.ShowPrompt(ErasePromptData);
        }
        public bool SelfValidation()
        {
            bool validation = true;
            validation &= ValidateUtility.ValidateReference(ContinueButtonObject);
            validation &= ValidateUtility.ValidateReference(NewGameButtonObject);
            validation &= ValidateUtility.ValidateReference(SaveGameButtonObject);
            validation &= ValidateUtility.ValidateReference(LoadGameButtonObject);
            validation &= ValidateUtility.ValidateReference(SettingsButtonObject);
            validation &= ValidateUtility.ValidateReference(QuitGameButtonObject);

            validation &= ValidateUtility.ValidateReference(EraseGameButtonObject);

            return validation;
        }
    }

}
