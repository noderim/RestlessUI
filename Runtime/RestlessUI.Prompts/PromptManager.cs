using System;
using System.Collections.Generic;
using Lean.Gui;
using RestlessEngine.Diagnostics;
using RestlessLib;
using RestlessLib.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RestlessUI
{
    [AddComponentMenu("Restless Engine/UI/Prompt Manager")]
    [DisallowMultipleComponent]
    public class PromptManager : MonoSingleton<PromptManager>
    {
        [ReadOnly]
        public PromptData LastPromptData;
        public List<PromptWindow> PromptWindows = new List<PromptWindow>();

        [Space(10)]
        public Transform PromptsParent;
        public GameObject PromptPrefab;
        public PromptData DefaultPromptData;



        public void ShowPrompt(PromptData promptData, UnityAction SubmitAction = null, UnityAction CancelAction = null, UnityAction CustomAction = null)
        {
            if (promptData == null)
            {
                LogManager.LogError("PromptData is null. Cannot show prompt.");
            }

            if (SubmitAction != null)
            {
                promptData.submitAction.ButtonEvent = new UnityEvent();
                promptData.submitAction.ButtonEvent.AddListener(SubmitAction);
            }

            if (CancelAction != null)
            {
                promptData.cancelAction.ButtonEvent = new UnityEvent();
                promptData.cancelAction.ButtonEvent.AddListener(CancelAction);
            }

            if (CustomAction != null)
            {
                promptData.customAction.ButtonEvent = new UnityEvent();
                promptData.customAction.ButtonEvent.AddListener(CustomAction);
            }


            GameObject promptInstance = Instantiate(PromptPrefab, PromptsParent);
            PromptWindow prompt = promptInstance.GetComponent<PromptWindow>();
            if (prompt != null)
            {
                prompt.SetPromptData(promptData);
                promptInstance.GetComponent<LeanWindow>().TurnOn();
                PromptWindows.Add(prompt);
            }
            else
            {
                Debug.LogError("Prompt script not found on prefab.");
            }
        }

        public void ClosePrompt()
        {
            if (PromptWindows.Count == 0)
            {
                LogManager.LogError("No prompts to close.");
                return;
            }
            PromptWindows[PromptWindows.Count - 1].gameObject.GetComponent<LeanWindow>().TurnOff();
            PromptWindows.RemoveAt(PromptWindows.Count - 1);
        }
    }
}
