using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Main Panel")]
        [SerializeField]
        private GameObject dialogueRoot;


        [Header("Text")]
        [SerializeField]
        private TMP_Text speakerText;

        [SerializeField]
        private TMP_Text dialogueText;


        [Header("Portrait")]
        [SerializeField]
        private Image portraitImage;


        [Header("Choices")]
        [SerializeField]
        private Transform choiceContainer;

        [SerializeField]
        private ChoiceButton choicePrefab;



        private readonly List<ChoiceButton> spawnedChoices =
            new();



        public TMP_Text DialogueText =>
            dialogueText;



        private void Awake()
        {
            if (dialogueRoot == null)
                dialogueRoot = gameObject;
        }



        #region Visibility


        public void Show()
        {
            dialogueRoot.SetActive(true);
        }



        public void Hide()
        {
            dialogueRoot.SetActive(false);
        }


        #endregion



        #region Reset


        public void ResetUI()
        {
            ClearChoices();


            if (dialogueText != null)
                dialogueText.text = "";


            if (speakerText != null)
                speakerText.text = "";


            if (portraitImage != null)
                portraitImage.sprite = null;
        }

        public void ClearText()
        {
            if (dialogueText != null)
                dialogueText.text = "";
        }



        #endregion



        #region Text


        public void SetSpeaker(string speaker)
        {
            if (speakerText == null)
                return;


            speakerText.text = speaker;
        }



        public void SetPortrait(Sprite sprite)
        {
            if (portraitImage == null)
                return;


            portraitImage.sprite = sprite;

            portraitImage.enabled =
                sprite != null;
        }


        #endregion



        #region Choices


        public void CreateChoice(string text, UnityAction action, bool activate)
        {
            ClearText();

            ChoiceButton button = Instantiate(choicePrefab, choiceContainer);

            button.Initialize(text, action, activate);

            spawnedChoices.Add(button);
        }



        public void ClearChoices()
        {
            foreach (ChoiceButton button
                    in spawnedChoices)
            {
                if (button != null)
                    Destroy(button.gameObject);
            }


            spawnedChoices.Clear();
        }


        #endregion
    }
}