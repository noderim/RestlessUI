using RestlessEngine.Application;
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
            _ = LoadingScreen.CallLoadingScreen(InitializationTask);
        }
        public void OnInitializationStateChanged()
        {
            InitializationTask.progress = GameInitializationManager.Instance.InitializationProgress;
            InitializationTask.LoadingDescription = GameInitializationManager.Instance.InitializationState;
        }
    }
}
