using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.Events;
using RestlessLib.Attributes;

namespace RestlessUI
{
    [System.Serializable]
    public class LoadingTask
    {
        [SerializeField]
        private float _progress = 0f;
        public float progress
        {
            get => _progress;
            set
            {
                _progress = Mathf.Clamp01(value);

                if (UsePredefinedProgressDescription && PredefinedProgressDescriptions.Count > 0)
                {
                    // Wybieramy najbliższy opis do aktualnego progressu
                    float closestKey = 0f;
                    foreach (var key in PredefinedProgressDescriptions.Keys)
                    {
                        if (key <= _progress && key > closestKey)
                            closestKey = key;
                    }
                    LoadingDescription = PredefinedProgressDescriptions[closestKey];
                }
            }
        }

        public string LoadingTitle = "Loading";
        public string LoadingDescription = "Please wait...";
        public bool UsePredefinedProgressDescription = false;
        public AYellowpaper.SerializedCollections.Dictionary<float, string> PredefinedProgressDescriptions = new AYellowpaper.SerializedCollections.Dictionary<float, string>();
        [SerializeField]
        public LoadingScreenOptions Options = new LoadingScreenOptions();

        [InfoBox("If true, the loading screen will refresh automatically, otherwise you need to call OnChangedEvent.Invoke() manually, externally.")]
        public bool RefreshAutomatically = false;
        public UnityEvent OnChangedEvent = new UnityEvent();

        public string GetProgressDescription(float progress)
        {
            if (PredefinedProgressDescriptions.Count == 0)
                return LoadingDescription;

            float closestKey = 0f;
            foreach (var key in PredefinedProgressDescriptions.Keys)
            {
                if (key <= progress && key > closestKey)
                    closestKey = key;
            }
            return PredefinedProgressDescriptions[closestKey];
        }

        [System.Serializable]
        public struct LoadingScreenOptions
        {
            public bool ShowTitle;
            public bool ShowDescription;
            public bool ShowProgress;
            public bool ShowLoadingCircle;

            public LoadingScreenOptions(bool showTitle = true, bool showDescription = true, bool showProgress = true, bool showLoadingCircle = true)
            {
                ShowTitle = showTitle;
                ShowDescription = showDescription;
                ShowProgress = showProgress;
                ShowLoadingCircle = showLoadingCircle;
            }
        }
    }

}
