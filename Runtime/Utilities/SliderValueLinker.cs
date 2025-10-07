using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RestlessUI
{
    /// <summary>
    /// This class is used to link a slider value to a TextMeshProUGUI component.
    /// It allows you to display the slider value in different formats (e.g., percentage, time, angle).
    /// </summary>
    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("RestlessEngine/UI/Utility/Slider Value Linker")]
    public class SliderValueLinker : MonoBehaviour
    {
        public TextMeshProUGUI SliderValueReciverLink;
        [Space(10)]
        public bool AddDecorations = true;
        public SliderValueType sliderValueType = SliderValueType.Value;

        void Start()
        {
            GetComponent<Slider>().onValueChanged.AddListener(OnValueChangeEvent);
        }

        public void OnValueChangeEvent(float value)
        {
            if (SliderValueReciverLink == null)
            {
                Debug.LogWarning("SliderValueReciverLink is not assigned.");
                return;
            }

            if (!AddDecorations)
            {
                SliderValueReciverLink.text = value.ToString("F2");
                return;
            }

            switch (sliderValueType)
            {
                case SliderValueType.Value:
                    SliderValueReciverLink.text = value.ToString("F2");
                    break;
                case SliderValueType.Percentage:
                    SliderValueReciverLink.text = (value * 100).ToString("F0") + "%";
                    break;
                case SliderValueType.Time:
                    int hours = Mathf.FloorToInt(value * 24);
                    int minutes = Mathf.FloorToInt((value * 24 - hours) * 60);
                    SliderValueReciverLink.text = $"{hours:D2}:{minutes:D2}";
                    break;
                case SliderValueType.Angle:
                    SliderValueReciverLink.text = (value * 360).ToString("F0") + "°";
                    break;
                default:
                    SliderValueReciverLink.text = "Unknown";
                    break;
            }
        }

        public enum SliderValueType
        {
            Value,
            Percentage,
            Time,
            Angle,

        }
    }
}
