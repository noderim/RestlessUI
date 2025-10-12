using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RestlessUI.Utilities
{
    public class RefreshRateDropdownUtility : MonoBehaviour
    {
        public TMP_Dropdown dropdown;
        private static readonly string[] PopularRefreshRates = { "Unlimited", "1000", "360", "240", "175", "144", "100", "60", "30" };

        private void Awake()
        {
            PopulateDropdown();
        }
        /// <summary>
        /// Populates a TMP_Dropdown with refresh rate options up to the closest ceiling of the monitor's highest refresh rate.
        /// </summary>
        [ContextMenu("Populate Refresh Rate Dropdown")]
        public void PopulateDropdown()
        {
            int monitorMaxRefreshRate = GetMonitorMaxRefreshRate();
            int ceilRefreshRate = Mathf.CeilToInt(monitorMaxRefreshRate);

            List<string> options = new List<string>();
            for (int i = 0; i < PopularRefreshRates.Length; i++)
            {
                if (PopularRefreshRates[i] == "Unlimited" || int.Parse(PopularRefreshRates[i]) <= ceilRefreshRate)
                {
                    options.Add(PopularRefreshRates[i]);
                }
            }

            // If the monitor's max refresh rate is not in the list, add it
            if (!options.Contains(ceilRefreshRate.ToString()))
            {
                options.Add(ceilRefreshRate.ToString());
            }
            options.Sort((a, b) =>
            {
                if (a == "Unlimited") return -1;
                if (b == "Unlimited") return 1;
                return int.Parse(b).CompareTo(int.Parse(a));
            });

            dropdown.ClearOptions();
            dropdown.AddOptions(options);
        }

        /// <summary>
        /// Gets the highest refresh rate of the current monitor.
        /// </summary>
        /// <returns>Highest refresh rate in Hz as an integer.</returns>
        private static int GetMonitorMaxRefreshRate()
        {
            // Unity's Screen.currentResolution.refreshRate returns the current refresh rate.
            // To get all possible rates, use Screen.resolutions.
            int maxRate = 0;
            foreach (var res in Screen.resolutions)
            {
                if (res.refreshRateRatio.value > maxRate)
                    maxRate = Mathf.CeilToInt((float)res.refreshRateRatio.value);
            }
            return maxRate > 0 ? maxRate : Mathf.CeilToInt((float)Screen.currentResolution.refreshRateRatio.value);
        }
    }
}