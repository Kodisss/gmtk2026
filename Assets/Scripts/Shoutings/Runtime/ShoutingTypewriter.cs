using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(AudioSource))]
public class ShoutingTypewriter : MonoBehaviour
{
    private GameSettings gameSettings;

    private float charactersPerSecond;
    private float commaPause;
    private float periodPause;
    private float ellipsisPause;

    private AudioSource audioSource;
    private AudioClip currentVoice;

    private float minPitch;
    private float maxPitch;
    private int charactersPerBlip;


    private TMP_Text textField;

    private Coroutine typingCoroutine;

    private float previousPitch;

    public bool IsTyping { get; private set; }


    private void Awake()
    {
        textField = GetComponent<TMP_Text>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gameSettings = GameSettings.Instance;
    }

    public void SetupPunctuation(float charPerSec, float cPause, float pPause, float ePause)
    {
        charactersPerSecond = charPerSec;
        commaPause = cPause;
        periodPause = pPause;
        ellipsisPause = ePause;
    }

    public void SetupVoiceSettings(float mini, float max, int charPerBlip)
    {
        minPitch = mini;
        maxPitch = max;
        charactersPerBlip = charPerBlip;
    }

    public void Type(string text, AudioClip voice, float typingSpeed = 1f, Action onFinished = null)
    {
        StopTyping();

        currentVoice = voice;

        typingCoroutine = StartCoroutine(TypeRoutine(text, typingSpeed, onFinished));
    }



    private IEnumerator TypeRoutine(string text, float typingSpeed, Action onFinished)
    {
        IsTyping = true;

        textField.text = "";

        float delay = 1f / (charactersPerSecond * typingSpeed);

        int blipCounter = 0;


        for (int i = 0; i < text.Length; i++)
        {
            char character = text[i];

            textField.text += character;


            if (!char.IsWhiteSpace(character) && !char.IsPunctuation(character))
            {
                blipCounter++;

                if (blipCounter >= charactersPerBlip)
                {
                    PlayVoice();

                    blipCounter = 0;
                }
            }


            yield return new WaitForSeconds(delay + GetPunctuationPause(text, i));
        }


        IsTyping = false;

        typingCoroutine = null;

        onFinished?.Invoke();
    }



    private float GetPunctuationPause(string text, int index)
    {
        char character = text[index];


        if (character == '.')
        {
            bool isEllipsis = index + 1 < text.Length && text[index + 1] == '.';

            bool lastEllipsisDot = index >= 2 && text[index - 1] == '.' && text[index - 2] == '.';

            if (lastEllipsisDot) return ellipsisPause;

            if (isEllipsis) return 0.2f;

            return periodPause;
        }

        if (character == ',') return commaPause;

        if (character == '!' || character == '?') return periodPause;

        return 0f;
    }

    private void PlayVoice()
    {
        if (audioSource == null || currentVoice == null) return;

        float pitch;

        do
        {
            pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        }
        while (Mathf.Abs(pitch - previousPitch) < 0.03f);

        previousPitch = pitch;

        audioSource.pitch = pitch;

        audioSource.PlayOneShot(currentVoice, gameSettings.VoiceVolume);
    }

    public void StopTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);

            typingCoroutine = null;
        }

        IsTyping = false;
    }


    public void Clear()
    {
        StopTyping();

        textField.text = "";
    }


    public void FinishImmediately()
    {
        if (!IsTyping) return;

        StopTyping();
    }
}