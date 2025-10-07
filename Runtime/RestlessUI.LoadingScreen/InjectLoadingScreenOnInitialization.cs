using RestlessEngine.Application;
using RestlessEngine.Diagnostics;
using UnityEngine;

namespace RestlessUI
{
    public class InjectLoadingScreenOnInitialization : MonoBehaviour
    {
        public LoadingTask InitializationTask;
        private void Awake()
        {
            GameInitializationManager.Instance.onGameInitializationStarted.AddListener(CallInitializationLoadingScreen);
            GameInitializationManager.Instance.onGameInitializationProgressChanged.AddListener(OnInitializationStateChanged);
        }
        public void CallInitializationLoadingScreen()
        {
            LogManager.Log("Calling Loading Screen for Initialization", LogTag.Debug);
            LoadingScreen.CallLoadingScreen(InitializationTask);
        }
        public void OnInitializationStateChanged()
        {
            InitializationTask.progress = GameInitializationManager.Instance.InitializationProgress;
            if (InitializationTask.UsePredefinedProgressDescription)
            {
                InitializationTask.LoadingDescription = InitializationTask.GetProgressDescription(InitializationTask.progress);
            }
            else
            {
                InitializationTask.LoadingDescription = GameInitializationManager.Instance.InitializationState;
            }


            InitializationTask.OnChangedEvent?.Invoke();
        }
    }
}
