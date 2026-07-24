using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("Main Panel")]
        [SerializeField] private GameObject dialogueRoot;


        [Header("Text")]
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private TMP_Text dialogueText;

        [Header("Yes No Reaction Sprites")]
        [SerializeField] private Sprite lookCenter;
        [SerializeField] private Sprite lookUp;
        [SerializeField] private Sprite lookDown;
        [SerializeField] private Sprite lookLeft;
        [SerializeField] private Sprite lookRight;
        [SerializeField] private float centerDeadZone = 0.25f;

        [Header("Portrait")]
        [SerializeField] private Image portraitImage;

        private Coroutine portraitCoroutine;

        [Header("Choices")]
        [SerializeField] private Transform choiceContainer;
        [SerializeField] private ChoiceButton choicePrefab;

        private readonly List<ChoiceButton> spawnedChoices = new();

        public TMP_Text DialogueText => dialogueText;

        private void Awake()
        {
            if (dialogueRoot == null) dialogueRoot = gameObject;
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

            if (dialogueText != null) dialogueText.text = "";

            if (speakerText != null) speakerText.text = "";

            if (portraitImage != null) portraitImage.sprite = null;
        }

        public void ClearText()
        {
            if (dialogueText != null) dialogueText.text = "";
        }

        public void DisplayYesNoText()
        {
            ClearText();
            dialogueText.text = "Shake your head or Nod to answer the question!";
        }

        public void SetReactionDirection(DialogueLookDirection direction)
        {
            switch (direction)
            {
                case DialogueLookDirection.Center:
                    portraitImage.sprite = lookCenter;
                    break;


                case DialogueLookDirection.Up:
                    portraitImage.sprite = lookUp;
                    break;


                case DialogueLookDirection.Down:
                    portraitImage.sprite = lookDown;
                    break;


                case DialogueLookDirection.Left:
                    portraitImage.sprite = lookLeft;
                    break;


                case DialogueLookDirection.Right:
                    portraitImage.sprite = lookRight;
                    break;
            }
        }

        #endregion

        #region Text

        public void SetSpeaker(string speaker)
        {
            if (speakerText == null) return;

            speakerText.text = speaker;
        }



        public void SetPortrait(Sprite[] portraits, int animationFPS)
        {
            if (portraitCoroutine != null)
            {
                StopCoroutine(portraitCoroutine);
                portraitCoroutine = null;
            }

            if (portraits == null || portraits.Length == 0)
            {
                portraitImage.sprite = null;
                return;
            }

            if (portraits.Length == 1)
            {
                portraitImage.sprite = portraits[0];
                return;
            }

            portraitCoroutine = StartCoroutine(AnimatePortrait(portraits, animationFPS));
        }

        private IEnumerator AnimatePortrait(Sprite[] portraits, int animationFPS)
        {
            int index = 0;

            float delay = 1f / animationFPS;

            while (true)
            {
                portraitImage.sprite = portraits[index];

                index++;

                if (index >= portraits.Length) index = 0;

                yield return new WaitForSeconds(delay);
            }
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