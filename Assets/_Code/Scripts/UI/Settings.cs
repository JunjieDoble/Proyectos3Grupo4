using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumSlider;
    [SerializeField] private Slider _musicVolumSlider;
    [SerializeField] private Slider _sfxVolumSlider;

    private const string MasterBusPath = "bus:/";
    private const string MusicBusPath = "bus:/Music";
    private const string SFXBusPath = "bus:/SFX";

    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _sfxBus;

    private void Start()
    {
        _masterBus = RuntimeManager.GetBus(MasterBusPath);
        _musicBus = RuntimeManager.GetBus(MusicBusPath);
        _sfxBus = RuntimeManager.GetBus(SFXBusPath);

        _masterBus.getVolume(out float masterVolume);
        _musicBus.getVolume(out float musicVolume);
        _sfxBus.getVolume(out float sfxVolume);

        _masterVolumSlider.value = masterVolume;
        _musicVolumSlider.value = musicVolume;
        _sfxVolumSlider.value = sfxVolume;
    }

    public void SetMasterVolume(float volume)
    {
        _masterBus.setVolume(volume);
        Debug.Log($"Master volume set to: {volume}");
    }

    public void SetMusicVolume(float volume)
    {
        _musicBus.setVolume(volume);
        Debug.Log($"Music volume set to: {volume}");
    }

    public void SetSFXVolume(float volume)
    {
        _sfxBus.setVolume(volume);
        Debug.Log($"SFX volume set to: {volume}");
    }

}
