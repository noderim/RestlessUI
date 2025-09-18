using System.Collections.Generic;
using UnityEngine;

namespace RestlessUI
{
    public class LoadingTask
    {
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
        public Dictionary<float, string> PredefinedProgressDescriptions = new Dictionary<float, string>();

        public LoadingScreenOptions Options = new LoadingScreenOptions();

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
