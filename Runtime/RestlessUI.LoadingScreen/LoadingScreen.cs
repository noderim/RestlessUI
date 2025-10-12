using System.Collections.Generic;
using Lean.Gui;
using RestlessEngine;
using RestlessEngine.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RestlessUI
{
    public class LoadingScreen : SingletonSystem<LoadingScreen>
    {
        [Header("Loading Task")]
        public LoadingScreenState CurrentState = LoadingScreenState.Inactive;
        public LoadingTask LinkedLoadingTask;
        public List<LoadingTask> LoadingScreenQueue = new List<LoadingTask>();

        [Header("UI Elements")]
        public TextMeshProUGUI Titletext;
        public TextMeshProUGUI DescriptionText;
        public TextMeshProUGUI ProgressText;
        public Slider ProgressBarSlider;
        public RectTransform LoadingCircle;
        [Header("Simulation")]
        public float LoadingCircleRotationSpeed = 360f;
        public bool AnimateProgressBar;
        public float progressBarLerpFactor = 5f;
        private float targetProgress = 0f;
        public float LoadingCompleteDelay = 0.5f; // seconds
        private float loadingCompleteTimer = 0f;

        [Header("Setup")]
        public LeanToggle window;

        public void FixedUpdate()
        {
            if (CurrentState == LoadingScreenState.Active)
            {
                UpdatePanel();
            }
        }
        private void UpdatePanel()
        {
            if (LoadingCircle != null)
                LoadingCircle.Rotate(Vector3.forward, -LoadingCircleRotationSpeed * Time.deltaTime);

            if (LinkedLoadingTask.RefreshAutomatically)
            {
                Refresh();
            }

            if (ProgressBarSlider != null && AnimateProgressBar) ProgressBarSlider.value = Mathf.Lerp(ProgressBarSlider.value, targetProgress, Time.deltaTime * progressBarLerpFactor);

            if (LinkedLoadingTask != null && LinkedLoadingTask.progress >= 1f)
            {
                loadingCompleteTimer += Time.fixedDeltaTime;
                if (loadingCompleteTimer >= LoadingCompleteDelay)
                {
                    CompleteCurrentTask();
                    loadingCompleteTimer = 0f;
                }
            }
        }

        public static void CallLoadingScreen(LoadingTask task, bool appendToQueue = true, bool forceLoadingScreen = false)
        {
            if (Instance.CurrentState == LoadingScreenState.Inactive || forceLoadingScreen)
            {
                Instance.CurrentState = LoadingScreenState.Loading;
                Instance.LoadingScreenQueue.Add(task);
                Instance.LoadLoadingScreen();
            }
            else if (appendToQueue)
            {
                Instance.LoadingScreenQueue.Add(task);
            }
            else
            {
                LogManager.Log("Loading screen already active - skipping new loading screen call.", LogTag.UI);
            }
        }
        private void LoadLoadingScreen()
        {
            if (window == null)
            {
                window = GetComponent<LeanToggle>();
            }

            LinkedLoadingTask = LoadingScreenQueue[0];
            SetupLoadingPanel();
            // LogManager.Log($"Loading screen started : {LinkedLoadingTask.LoadingTitle}, {LinkedLoadingTask.LoadingDescription}, progress: {LinkedLoadingTask.progress}, options: {LinkedLoadingTask.Options.ShowTitle}, {LinkedLoadingTask.Options.ShowDescription}, {LinkedLoadingTask.Options.ShowProgress}, {LinkedLoadingTask.Options.ShowLoadingCircle} ", LogTag.Debug);
            CurrentState = LoadingScreenState.Active;
            window.TurnOn();
            Refresh();

            if (!LinkedLoadingTask.RefreshAutomatically)
            {
                LinkedLoadingTask.OnChangedEvent.AddListener(Refresh);
            }
        }
        private void CompleteCurrentTask()
        {
            LoadingScreenQueue.Remove(LinkedLoadingTask);

            if (!LinkedLoadingTask.RefreshAutomatically)
            {
                LinkedLoadingTask.OnChangedEvent.RemoveListener(Refresh);
            }

            LinkedLoadingTask = null;

            if (LoadingScreenQueue.Count > 0)
            {
                LoadLoadingScreen();
            }
            else
            {
                UnloadLoadingScreen();
            }
        }

        private void UnloadLoadingScreen()
        {
            CurrentState = LoadingScreenState.Unloading;
            if (window != null)
            {
                window.TurnOff();
            }
            CurrentState = LoadingScreenState.Inactive;
        }
        public void Refresh()
        {
            if (LinkedLoadingTask == null) return;
            if (Titletext != null) Titletext.text = LinkedLoadingTask.LoadingTitle;
            if (DescriptionText != null) DescriptionText.text = LinkedLoadingTask.LoadingDescription;
            if (ProgressText != null) ProgressText.text = LinkedLoadingTask.progress.ToString("P0");
            targetProgress = LinkedLoadingTask.progress;

            if (!AnimateProgressBar)
            {
                if (ProgressBarSlider != null) ProgressBarSlider.value = LinkedLoadingTask.progress;
            }
        }
        private void SetupLoadingPanel()
        {
            Titletext.gameObject.SetActive(LinkedLoadingTask.Options.ShowTitle);
            DescriptionText.gameObject.SetActive(LinkedLoadingTask.Options.ShowDescription);
            ProgressText.gameObject.SetActive(LinkedLoadingTask.Options.ShowProgress);
            ProgressBarSlider.gameObject.SetActive(LinkedLoadingTask.Options.ShowProgress);
            LoadingCircle.gameObject.SetActive(LinkedLoadingTask.Options.ShowLoadingCircle);
        }
    }
    public enum LoadingScreenState
    {
        Inactive,
        Loading,
        Active,
        Unloading
    }
}
