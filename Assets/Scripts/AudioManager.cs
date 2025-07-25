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
    public AudioClip ghostSfxClip;
    void Start()
    {
        // Load audio settings from PlayerPrefs
        musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicAudioSource.mute = PlayerPrefs.GetInt("MusicMute", 0) == 1;
        vfxAudioSource.volume = Mathf.Max(PlayerPrefs.GetFloat("SFXVolume", 1f), 0.1f); // Minimum volume 0.1
        vfxAudioSource.mute = PlayerPrefs.GetInt("SFXMute", 0) == 1;

        musicAudioSource.clip = musicClips;
        musicAudioSource.Play();

        vfxAudioSource.spatialBlend = 0f; // 2D audio
        vfxAudioSource.playOnAwake = false;
        Debug.Log($"AudioManager initialized: vfxAudioSource Volume={vfxAudioSource.volume}, Mute={vfxAudioSource.mute}, PlayerPrefs SFXVolume={PlayerPrefs.GetFloat("SFXVolume", 1f)}, SFXMute={PlayerPrefs.GetInt("SFXMute", 0)}");
    }
    public void PlaySFX(AudioClip sfx, AudioSource source)
    {
        if (sfx != null && source != null)
        {
            source.PlayOneShot(sfx);
            Debug.Log($"Playing SFX: {sfx.name} on {source.gameObject.name}, Volume={source.volume}, Mute={source.mute}");
        }
        else
        {
            Debug.LogWarning($"Failed to play SFX: sfx={(sfx != null ? sfx.name : "null")}, source={(source != null ? source.gameObject.name : "null")}");
        }
    }

}
