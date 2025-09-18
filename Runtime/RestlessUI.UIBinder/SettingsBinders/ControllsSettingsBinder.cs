using RestlessEngine.Diagnostics;
using RestlessEngine.Application.Settings;

namespace RestlessUI.Components
{
    public partial class SettingsWindowManager
    {
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
    }
}
