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
        private float currentTypingSpeed = 1f;

        [Header("Voice")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip defaultVoice;
        [SerializeField] private float minPitch = 0.9f;
        [SerializeField] private float maxPitch = 1.1f;

        [SerializeField] private int charactersPerBlip = 2;

        private AudioClip currentVoice;
        private float currentVolume;
        private float previousPitch;

        [Header("Punctuation")]
        [SerializeField] private float commaPause = 0.15f;
        [SerializeField] private float periodPause = 0.35f;
        [SerializeField] private float ellipsisPause = 0.8f;
        [SerializeField] private float ellipsisDotDelay = 0.2f;
        [SerializeField] private float ellipsisEndPause = 0.8f;
        [SerializeField] private float exclamationPause = 0.45f;
        [SerializeField] private float questionPause = 0.45f;

        private Coroutine typingCoroutine;
        private bool isTyping;

        private TMP_Text currentTextField;
        private string currentFullText;

        public bool IsTyping => isTyping;

        public void Type(TMP_Text textField, string text, AudioClip voice, float volume, float typingSpeed, Action onFinished)
        {
            StopTyping();

            currentTextField = textField;
            currentFullText = text;

            currentVoice = voice;
            currentVolume = volume;

            currentTypingSpeed = typingSpeed;

            typingCoroutine = StartCoroutine(TypeRoutine(onFinished));
        }



        private IEnumerator TypeRoutine(
            Action onFinished)
        {
            isTyping = true;

            currentTextField.text = "";

            float delay = 1f / (charactersPerSecond * currentTypingSpeed);

            int blipCounter = 0;

            for (int i = 0; i < currentFullText.Length; i++)
            {
                char character = currentFullText[i];

                currentTextField.text += character;

                if (!char.IsWhiteSpace(character) &&
                    !char.IsPunctuation(character))
                {
                    blipCounter++;

                    if (blipCounter >= charactersPerBlip)
                    {
                        PlayVoice();
                        blipCounter = 0;
                    }
                }

                float currentDelay = delay;

                currentDelay += GetPunctuationPause(i);

                yield return new WaitForSeconds(currentDelay);
            }

            FinishTyping();

            onFinished?.Invoke();
        }

        private float GetPunctuationPause(int index)
        {
            char character = currentFullText[index];


            if (character == '.')
            {
                bool isEllipsis =
                    index + 1 < currentFullText.Length &&
                    currentFullText[index + 1] == '.';


                bool isLastEllipsisDot =
                    index >= 2 &&
                    currentFullText[index - 1] == '.' &&
                    currentFullText[index - 2] == '.';


                if (isLastEllipsisDot)
                    return ellipsisEndPause;


                if (isEllipsis)
                    return ellipsisDotDelay;


                return periodPause;
            }


            if (character == ',')
                return commaPause;


            if (character == '!' || character == '?')
                return periodPause;


            return 0f;
        }

        private void PlayVoice()
        {
            if (audioSource == null) return;
            if (currentVoice == null) currentVoice = defaultVoice;

            float pitch;

            do
            {
                pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            }
            while (Mathf.Abs(pitch - previousPitch) < 0.03f);

            previousPitch = pitch;

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(currentVoice, currentVolume);
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