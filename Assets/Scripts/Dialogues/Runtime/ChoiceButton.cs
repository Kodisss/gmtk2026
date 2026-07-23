using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Dialogue
{
    /// <summary>
    /// Represents one selectable dialogue choice button.
    /// The DialogueUI creates these dynamically at runtime.
    /// </summary>
    public class ChoiceButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TMP_Text buttonText;

        [SerializeField]
        private Button button;



        /// <summary>
        /// Initializes the button with text and click behaviour.
        /// </summary>
        public void Initialize(string text, UnityAction onClicked, bool canInteract)
        {
            buttonText.text = text;

            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(onClicked);

            button.interactable = canInteract;
        }



        /// <summary>
        /// Removes listeners before destruction.
        /// </summary>
        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        public void SetInteractable(bool value)
        {
            button.interactable = value;
        }
    }
}