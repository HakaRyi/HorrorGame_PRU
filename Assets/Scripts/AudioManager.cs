using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;  

    public AudioClip musicClips;
    public AudioClip vfxClips;
    public AudioClip buttonClickClip;
    void Start()
    {
        musicAudioSource.clip = musicClips;
        musicAudioSource.Play();
    }
    public void PlaySFX(AudioClip sfx)
    {
        vfxAudioSource.clip = sfx;
        vfxAudioSource.PlayOneShot(sfx);
    }

}
