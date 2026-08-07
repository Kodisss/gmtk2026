using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private GameSettings gameSettings;

    [Header("Music Library")]
    [SerializeField] private List<MusicTrack> musicTracks = new List<MusicTrack>();

    [Header("Audio Sources")]
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource loopSource;

    private float musicVolume = 1f;
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
    }

    private void Start()
    {
        gameSettings = GameSettings.Instance;
        introSource.loop = false;
        loopSource.loop = true;
        UpdateVolume();
    }

    private void Update()
    {
        if (musicVolume != gameSettings.MusicVolume) UpdateVolume();
    }

    private void UpdateVolume()
    {
        musicVolume = gameSettings.MusicVolume;
        introSource.volume = musicVolume * currentTrackVolume;
        loopSource.volume = musicVolume * currentTrackVolume;
    }

    public void PlayTrackNb(int trackNumber)
    {
        // Debug.Log(trackNumber);

        FadeToMusic(musicTracks[trackNumber]);
    }

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
        if (track == null) return;

        if (track == currentTrack) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(track));
    }

    public void StopMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        introSource.Stop();
        loopSource.Stop();

        currentTrack = null;
    }

    private IEnumerator FadeRoutine(MusicTrack newTrack)
    {
        currentTrackVolume = newTrack.volume;

        introSource.Stop();
        loopSource.Stop();

        if (newTrack.HasIntro && newTrack.HasLoop)
        {
            introSource.clip = newTrack.intro;
            loopSource.clip = newTrack.loop;

            double startTime = AudioSettings.dspTime + 0.1;

            introSource.PlayScheduled(startTime);

            double loopStartTime = startTime + newTrack.intro.length;

            loopSource.PlayScheduled(loopStartTime);

            yield return new WaitForSeconds(newTrack.intro.length);
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