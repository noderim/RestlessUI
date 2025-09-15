using System;
using RestlessLib.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace RestlessUI
{
    [Serializable]
    public class PromptData
    {
        public LocalizedString PromptTitle;
        public bool useStaticAdditionalInfo = false;
        public string StaticAdditionalInfo;
        public LocalizedString AdditionalInfo;
        public LocalizedString Warning;
        [Space(10)]

        [SerializeField]
        public bool useCancelAction = true;
        public PromptAction cancelAction = new PromptAction()
        {
            ButtonLabel = new LocalizedString { TableReference = "Main Menu User Interface", TableEntryReference = "No" },
        };
        [HorizontalLine]
        public bool useSubmit = true;
        [SerializeField]
        public PromptAction submitAction = new PromptAction()
        {
            ButtonLabel = new LocalizedString { TableReference = "Main Menu User Interface", TableEntryReference = "Yes" },
        };
        [HorizontalLine]

        public bool useCustomAction = false;
        [SerializeField]
        public PromptAction customAction;
        private PromptData defaultPromptData;

        public PromptData(PromptData defaultPromptData)
        {
            this.defaultPromptData = defaultPromptData;
        }
    }
    [Serializable]
    public class PromptAction
    {
        public LocalizedString ButtonLabel;
        [SerializeField]
        public UnityEvent ButtonEvent;
    }
}
