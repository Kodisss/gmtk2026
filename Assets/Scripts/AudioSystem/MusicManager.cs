using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }


    [Header("Music Library")]
    [SerializeField] private List<MusicTrack> musicTracks = new List<MusicTrack>();


    [Header("Audio Sources")]
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource loopSource;


    [Header("Settings")]
    [SerializeField][Range(0f, 1f)] private float musicVolume = 1f;
    [SerializeField] private float fadeDuration = 1f;

    public float MusicVolume => musicVolume;

    private float currentTrackVolume = 1f;

    private MusicTrack currentTrack;
    private Coroutine fadeCoroutine;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);


        introSource.loop = false;

        loopSource.loop = true;

        introSource.volume = 0f;
        loopSource.volume = 0f;
    }

    private void OnValidate()
    {
        if (musicVolume < 0f)
            musicVolume = 0f;

        if (musicVolume > 1f)
            musicVolume = 1f;


        if (Application.isPlaying)
        {
            ApplyVolume();
        }
    }

    private void ApplyVolume()
    {
        float introVolume = currentTrackVolume * musicVolume;
        float loopVolume = currentTrackVolume * musicVolume;

        introSource.volume = introVolume;
        loopSource.volume = loopVolume;
    }


    // -------------------------------
    // PUBLIC FUNCTIONS
    // -------------------------------


    public void PlayMusic(string trackID)
    {
        MusicTrack track = musicTracks.Find(x => x.trackID == trackID);


        if (track == null)
        {
            Debug.LogWarning($"Music track '{trackID}' not found!");
            return;
        }


        FadeToMusic(track);
    }



    public void FadeToMusic(MusicTrack track)
    {
        if (track == null)
            return;


        if (track == currentTrack)
            return;


        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);


        fadeCoroutine = StartCoroutine(FadeRoutine(track));
    }



    public void StopMusic()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);


        introSource.Stop();
        loopSource.Stop();

        introSource.volume = 0f;
        loopSource.volume = 0f;

        currentTrack = null;
    }



    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        introSource.volume = currentTrackVolume * musicVolume;
        loopSource.volume = currentTrackVolume * musicVolume;
    }



    // -------------------------------
    // MUSIC ROUTINE
    // -------------------------------


    private IEnumerator FadeRoutine(MusicTrack newTrack)
    {
        currentTrackVolume = newTrack.volume;

        // Fade out
        float startVolume = loopSource.volume;


        while (loopSource.volume > 0f)
        {
            loopSource.volume -= startVolume * Time.deltaTime / fadeDuration;

            yield return null;
        }


        introSource.Stop();
        loopSource.Stop();



        if (newTrack.HasIntro && newTrack.HasLoop)
        {
            introSource.clip = newTrack.intro;
            loopSource.clip = newTrack.loop;


            introSource.volume = newTrack.volume * musicVolume;
            loopSource.volume = 0f;


            double startTime = AudioSettings.dspTime + 0.1;


            introSource.PlayScheduled(startTime);


            double loopStartTime = startTime + newTrack.intro.length;


            loopSource.PlayScheduled(loopStartTime);


            yield return new WaitForSeconds(newTrack.intro.length);


            loopSource.volume = newTrack.volume * musicVolume;
        }
        else if (newTrack.HasLoop)
        {
            loopSource.clip = newTrack.loop;
            loopSource.volume = newTrack.volume * musicVolume;
            loopSource.Play();
        }
        else if (newTrack.HasIntro)
        {
            introSource.clip = newTrack.intro;
            introSource.volume = newTrack.volume * musicVolume;
            introSource.Play();
        }


        currentTrack = newTrack;

        fadeCoroutine = null;
    }
}