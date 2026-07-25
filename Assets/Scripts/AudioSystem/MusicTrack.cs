using UnityEngine;

[CreateAssetMenu(fileName = "New Music Track", menuName = "Audio/Music Track")]
public class MusicTrack : ScriptableObject
{
    [Header("Identification")]
    public string trackID;


    [Header("Audio")]
    [Tooltip("Played once before the loop starts")]
    public AudioClip intro;

    [Tooltip("Played repeatedly after intro")]
    public AudioClip loop;


    [Header("Settings")]
    [Range(0f, 1f)]
    public float volume = 1f;


    public bool HasIntro => intro != null;
    public bool HasLoop => loop != null;
}