using Lean.Gui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RestlessUI
{
    /// <summary>
    /// This class is used to create a button that can be clicked to perform an action.
    /// </summary>
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("RestlessEngine/UI/Utility/Button Utility")]
    public class ButtonUtillty : MonoBehaviour
    {
        public bool autoClearSelection = true;

        private Button button;
        private LeanButton leanButton;

        void Start()
        {
            if (autoClearSelection)
            {
                button = GetComponent<Button>();
                if (button != null) button.onClick.AddListener(ClearSelection);

                leanButton = GetComponent<LeanButton>();
                if (leanButton != null) leanButton.OnClick.AddListener(ClearSelection);
            }
        }

        public void ClearSelection()
        {
            // Deselect any selected UI element
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
