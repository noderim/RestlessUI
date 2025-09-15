using System.Collections.Generic;
using Lean.Gui;
using RestlessEngine.GameSaves;
using UnityEngine;

namespace RestlessUI.Components
{

    public class GameSavesPanelManager : MonoBehaviour
    {
        public bool isInLoadMode = true;
        public Transform SavesPanelsParent;
        public GameObject SavePanelPrefab;

        private SavesIndexer savesMetadata;
        public Transform EmptySavePanel;
        public LeanWindow Window;

        void Start()
        {
            // Get the player savefile system
            if (savesMetadata == null)
            {
                savesMetadata = GameSavefileSystem.Instance.savesMetadata;
            }
            EmptySavePanel.GetComponent<SavePanel>().gameSavesPanelManager = this; // Setup the empty save panel
        }

        public void OpenWindow(bool isLoadMode)
        {
            isInLoadMode = isLoadMode;
            Window.TurnOn();
            PopulateSavesPanels();
        }
        public void PopulateSavesPanels()
        {
            ClearSavesPanels();

            // Get all save files
            savesMetadata = GameSavefileSystem.Instance.savesMetadata;
            List<SaveMetadata> saveFiles = new(savesMetadata.saves);
            saveFiles.Reverse(); // Reverse the list to show the latest saves first
                                 // Create a panel for each save file
            EmptySavePanel.gameObject.SetActive(!isInLoadMode); // if not in load mode means we are in save mode so we show empty save panel else we hide it   
            foreach (var saveFile in saveFiles)
            {
                CreateSavePanel(saveFile);
            }
        }

        public void CreateSavePanel(SaveMetadata data)
        {
            // Create a new save panel
            GameObject savePanel = Instantiate(SavePanelPrefab, SavesPanelsParent);
            SavePanel panelScript = savePanel.GetComponent<SavePanel>();
            if (panelScript != null)
            {
                panelScript.SetupPanel(data, isInLoadMode, this);
            }
            else
            {
                Debug.LogError("SavePanel script not found on prefab.");
            }
        }

        public void ClearSavesPanels()
        {
            // Destroy all save panels
            foreach (Transform child in SavesPanelsParent)
            {
                if (child != EmptySavePanel)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

}
