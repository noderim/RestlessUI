namespace RestlessUI
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Lean.Gui;
    using RestlessEngine;
    using RestlessEngine.Diagnostics;
    using RestlessEngine.SceneManagement;
    using TMPro;
    using UnityEngine;

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
        public RectTransform ProgressBarFill;
        public RectTransform LoadingCircle;
        public float LoadingCircleRotationSpeed = 360f; // degrees per second

        [Header("Setup")]
        public static SceneObject LoadingScreenScene;
        public LeanWindow window;

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

            Refresh(); // odświeżamy UI progress w czasie rzeczywistym

            // Jeśli LinkedLoadingTask osiągnęła 100%, przechodzimy do następnego lub kończymy
            if (LinkedLoadingTask != null && LinkedLoadingTask.progress >= 1f)
            {
                CompleteCurrentTask();
            }
        }

        public static async Task CallLoadingScreen(LoadingTask task, bool appendToQueue = true, bool forceLoadingScreen = false)
        {
            if (LoadingScreenScene.State != SceneState.Loaded)
            {
                await LoadingScreenScene.LoadScene();
            }

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
                window = GetComponent<LeanWindow>();
            }

            SetupLoadingPanel();
            LinkedLoadingTask = LoadingScreenQueue[0];
            CurrentState = LoadingScreenState.Active;

            window.TurnOn();
            Refresh();
        }
        private void CompleteCurrentTask()
        {
            LoadingScreenQueue.Remove(LinkedLoadingTask);
            LinkedLoadingTask = null;

            if (LoadingScreenQueue.Count > 0)
            {
                // Ładujemy kolejny task
                LoadLoadingScreen();
            }
            else
            {
                // Kończymy ładowanie
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
            if (ProgressBarFill != null) ProgressBarFill.localScale = new Vector3(LinkedLoadingTask.progress, 1f, 1f);
        }
        private void SetupLoadingPanel()
        {
            Titletext.gameObject.SetActive(LinkedLoadingTask.Options.ShowTitle);
            DescriptionText.gameObject.SetActive(LinkedLoadingTask.Options.ShowDescription);
            ProgressText.gameObject.SetActive(LinkedLoadingTask.Options.ShowProgress);
            ProgressBarFill.gameObject.SetActive(LinkedLoadingTask.Options.ShowProgress);
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
