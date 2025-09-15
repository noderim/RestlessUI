using Lean.Gui;
using RestlessLib.Attributes;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace RestlessUI
{
    /// <summary>
    /// The PromptWindow class is responsible for displaying a prompt to the user with options for submission, cancellation, and custom actions.
    /// </summary>
    [AddComponentMenu("Restless Engine/UI/Prompt Window")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LeanWindow))]

    public class PromptWindow : MonoBehaviour
    {
        [Header("References")]
        public TMPro.TextMeshProUGUI PromptTitle;
        public TMPro.TextMeshProUGUI AdditionalInfo;
        public TMPro.TextMeshProUGUI WarningText;
        public LeanButton cancelButton;
        public LeanButton submitButton;
        public LeanButton customButton;

        [HorizontalLine]
        [Header("Prompt Data")]
        [SerializeField]
        public PromptData promptData;

        public void SetPromptData(PromptData data)
        {
            promptData = data;
            UpdatePromptUI();
        }

        [ContextMenu("Update Prompt UI")]
        private void UpdatePromptUI()
        {
            if (promptData == null) return;

            if (promptData.PromptTitle.IsEmpty)
            {
                PromptTitle.gameObject.SetActive(false);
                PromptTitle.text = string.Empty;
            }
            else
            {
                PromptTitle.gameObject.SetActive(true);
                PromptTitle.GetComponent<LocalizeStringEvent>().StringReference = promptData.PromptTitle;
                PromptTitle.GetComponent<LocalizeStringEvent>().RefreshString();
            }


            if (promptData.AdditionalInfo.IsEmpty && !promptData.useStaticAdditionalInfo)
            {
                AdditionalInfo.gameObject.SetActive(false);
                AdditionalInfo.text = string.Empty;
            }
            else if (promptData.useStaticAdditionalInfo)
            {
                AdditionalInfo.gameObject.SetActive(true);
                AdditionalInfo.text = promptData.StaticAdditionalInfo;
            }
            else
            {
                AdditionalInfo.gameObject.SetActive(true);
                AdditionalInfo.GetComponent<LocalizeStringEvent>().StringReference = promptData.AdditionalInfo;
                AdditionalInfo.GetComponent<LocalizeStringEvent>().RefreshString();
            }

            if (promptData.Warning.IsEmpty)
            {
                WarningText.gameObject.SetActive(false);
                WarningText.text = string.Empty;
            }
            else
            {
                WarningText.gameObject.SetActive(true);
                WarningText.GetComponent<LocalizeStringEvent>().StringReference = promptData.Warning;
                WarningText.GetComponent<LocalizeStringEvent>().RefreshString();
            }

            if (promptData.useCancelAction)
            {
                cancelButton.gameObject.SetActive(true);
                cancelButton.GetComponentInChildren<LocalizeStringEvent>().StringReference = promptData.cancelAction.ButtonLabel;
                cancelButton.GetComponentInChildren<LocalizeStringEvent>().RefreshString();
                cancelButton.OnClick.AddListener(() =>
                {
                    promptData.cancelAction.ButtonEvent.Invoke();
                });
            }
            else
            {
                cancelButton.gameObject.SetActive(false);
            }


            if (promptData.useSubmit)
            {
                submitButton.gameObject.SetActive(true);
                submitButton.GetComponentInChildren<LocalizeStringEvent>().StringReference = promptData.submitAction.ButtonLabel;
                submitButton.GetComponentInChildren<LocalizeStringEvent>().RefreshString();
                submitButton.OnClick.AddListener(() =>
                {
                    promptData.submitAction.ButtonEvent.Invoke();
                });
            }
            else
            {
                submitButton.gameObject.SetActive(false);
            }


            if (promptData.useCustomAction)
            {
                customButton.gameObject.SetActive(true);
                customButton.GetComponentInChildren<LocalizeStringEvent>().StringReference = promptData.customAction.ButtonLabel;
                customButton.GetComponentInChildren<LocalizeStringEvent>().RefreshString();
                customButton.OnClick.AddListener(() =>
                {
                    promptData.customAction.ButtonEvent.Invoke();
                });
            }
            else
            {
                customButton.gameObject.SetActive(false);
            }

        }

        public void ClosePrompt()
        {
            Destroy(gameObject);
        }
    }
}
