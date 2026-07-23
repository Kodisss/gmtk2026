using System.Collections;
using System;
using TMPro;
using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueTypewriter : MonoBehaviour
    {
        [Header("Typing Settings")]
        [SerializeField]
        private float charactersPerSecond = 40f;



        private Coroutine typingCoroutine;


        private bool isTyping;


        private TMP_Text currentTextField;


        private string currentFullText;



        public bool IsTyping => isTyping;



        /// <summary>
        /// Starts typing a new line of dialogue.
        /// </summary>
        public void Type(
            TMP_Text textField,
            string text,
            Action onFinished)
        {
            StopTyping();


            currentTextField = textField;
            currentFullText = text;


            typingCoroutine =
                StartCoroutine(
                    TypeRoutine(onFinished));
        }



        private IEnumerator TypeRoutine(
            Action onFinished)
        {
            isTyping = true;


            currentTextField.text = "";


            float delay =
                1f / charactersPerSecond;



            foreach (char character in currentFullText)
            {
                currentTextField.text += character;


                yield return new WaitForSeconds(delay);
            }


            FinishTyping();


            onFinished?.Invoke();
        }



        /// <summary>
        /// Immediately displays the entire sentence.
        /// </summary>
        public void FinishImmediately()
        {
            if (!isTyping)
                return;


            StopTyping();


            currentTextField.text =
                currentFullText;


            isTyping = false;
        }



        private void FinishTyping()
        {
            isTyping = false;

            typingCoroutine = null;
        }



        private void StopTyping()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(
                    typingCoroutine);

                typingCoroutine = null;
            }


            isTyping = false;
        }



        private void OnDisable()
        {
            StopTyping();
        }
    }
}