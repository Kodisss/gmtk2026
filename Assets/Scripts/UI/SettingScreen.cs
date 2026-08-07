using UnityEngine;
using UnityEngine.UI;

public class SettingScreen : MonoBehaviour
{
    private GameSettings gameSettings;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider voiceSlider;

    private void Start()
    {
        gameSettings = GameSettings.Instance;

        musicSlider.value = gameSettings.MusicVolume;
        sfxSlider.value = gameSettings.SoundVolume;
        voiceSlider.value = gameSettings.VoiceVolume;
    }

    public void ModifySFXVolume(float value)
    {
        gameSettings.SoundVolume = value;
    }
    public void ModifyMusicVolume(float value)
    {
        gameSettings.MusicVolume = value;
    }
    public void ModifyVoiceVolume(float value)
    {
        gameSettings.VoiceVolume = value;
    }
}
