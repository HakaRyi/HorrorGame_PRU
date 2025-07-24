using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;  

    public AudioClip musicClips;
    public AudioClip buttonClickClip;
    public AudioClip paperPickupClip; 
    public AudioClip keyPickupClip;
    public AudioClip doorOpenClip;
    void Start()
    {
        // Load audio settings from PlayerPrefs
        musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicAudioSource.mute = PlayerPrefs.GetInt("MusicMute", 0) == 1;
        vfxAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        vfxAudioSource.mute = PlayerPrefs.GetInt("SFXMute", 0) == 1;

        musicAudioSource.clip = musicClips;
        musicAudioSource.Play();

        vfxAudioSource.spatialBlend = 0f; // 2D audio
        vfxAudioSource.playOnAwake = false;
    }
    public void PlaySFX(AudioClip sfx, AudioSource source)
    {
        if (sfx != null && source != null)
        {
            source.PlayOneShot(sfx);
        }   
    }

}
