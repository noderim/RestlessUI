using TMPro;
using RestlessEngine.Diagnostics;
using RestlessEngine.Application.Settings;

namespace RestlessUI.Components
{
    public partial class SettingsWindowManager
    {
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
    }
}
