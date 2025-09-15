using UnityEngine;
using RestlessEngine;
using Lean.Gui;
using RestlessEngine.GameSaves;

namespace RestlessUI.Components
{
    public class SavePanel : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI saveNameText;
        public TMPro.TextMeshProUGUI saveDateText;
        public TMPro.TextMeshProUGUI saveSlotText;
        public TMPro.TextMeshProUGUI saveVersionText;

        public TMPro.TextMeshProUGUI savePlayTimeText;

        public TMPro.TextMeshProUGUI isAutosave;
        private SaveMetadata saveData;
        public LeanButton panelButton;
        private bool isInLoadMode = true; // Set to true if you want to load the save, false if you want to save
        public GameSavesPanelManager gameSavesPanelManager;

        public void SetupPanel(SaveMetadata data, bool isinLoadMode, GameSavesPanelManager gameSavesPanelManager)
        {
            this.gameSavesPanelManager = gameSavesPanelManager;
            isInLoadMode = isinLoadMode;
            saveData = data;
            UpdateSavePanel();
        }

        private void UpdateSavePanel()
        {
            if (saveData != null)
            {
                saveNameText.text = saveData.savefileName;
                saveDateText.text = saveData.lastSaved.date;
                saveSlotText.text = saveData.slot.ToString();
                saveVersionText.text = saveData.version.ToString();
                isAutosave.text = saveData.isAutosave ? "[Autosave]" : "[Manual]";
            }
            if (isInLoadMode)
            {
                panelButton.OnClick.AddListener(LoadSave);
            }
            else
            {
                panelButton.OnClick.AddListener(SaveGame);
            }
        }
        public void LoadSave()
        {
            GameSavefileSystem.Instance.LoadSave(saveData);
        }
        public void SaveGame()
        {
            GameSavefileSystem.Instance.SaveGameManual(saveData.slot);
            gameSavesPanelManager.PopulateSavesPanels(); // Refresh the save panels after saving
        }
        public void SaveGameInNewSlot()
        {
            GameSavefileSystem.Instance.SaveGameManual();
            gameSavesPanelManager.PopulateSavesPanels(); // Refresh the save panels after saving
        }
    }

}
