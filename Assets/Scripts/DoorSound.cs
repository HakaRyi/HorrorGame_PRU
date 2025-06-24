using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public AudioClip doorsound;
    private AudioSource audioSource;
    public AudioSource doorAudio;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDoorSound()
    {
        if (doorsound != null && audioSource != null)
        {
            audioSource.PlayOneShot(doorsound);
        } else if(doorAudio != null)
        {
            doorAudio.Play();        }
    }
}
