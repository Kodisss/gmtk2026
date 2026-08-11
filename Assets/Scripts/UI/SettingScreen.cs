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

    public void ModifyMusicVolume(float value)
    {
        gameSettings.MusicVolume = Mathf.Pow(value, 2.3f);
    }

    public void ModifySFXVolume(float value)
    {
        gameSettings.SoundVolume = Mathf.Pow(value, 2.3f);
    }
    
    public void ModifyVoiceVolume(float value)
    {
        gameSettings.VoiceVolume = Mathf.Pow(value, 2.3f);
    }
}
