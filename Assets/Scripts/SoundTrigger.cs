using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioClip triggerSound; // Sound to play (e.g., glass breaking or giggle)
    public GameObject soundSourceObject; // Optional GameObject to play sound from (e.g., position B)
    public float volumeMultiplier = 1.2f; // Multiplier to increase volume when playing from soundSourceObject
    private AudioSource triggerAudioSource; // AudioSource for playing sound
    private AudioManager audioManager;
    private bool hasTriggered = false; // Prevent sound from playing multiple times

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        // Configure AudioSource based on whether soundSourceObject is assigned
        if (soundSourceObject != null)
        {
            // Use or add AudioSource on soundSourceObject (position B)
            triggerAudioSource = soundSourceObject.GetComponent<AudioSource>();
            if (triggerAudioSource == null)
            {
                triggerAudioSource = soundSourceObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            // Use or add AudioSource on this GameObject (position A)
            triggerAudioSource = gameObject.GetComponent<AudioSource>();
            if (triggerAudioSource == null)
            {
                triggerAudioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Configure AudioSource for 3D sound
        triggerAudioSource.spatialBlend = 1f; // 3D audio
        triggerAudioSource.minDistance = 1f; // Max volume within 1 meter
        triggerAudioSource.maxDistance = 10f; // Volume drops to 0 at 10 meters
        triggerAudioSource.rolloffMode = AudioRolloffMode.Logarithmic; // Smooth volume falloff
        triggerAudioSource.playOnAwake = false; // Don't play on start

        // Ensure the GameObject has a BoxCollider with IsTrigger enabled
        var collider = GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true; // Enable trigger to allow player to pass through
        }
        else
        {
            collider.isTrigger = true; // Ensure existing collider is a trigger
        }
    }

    private void Update()
    {
        if (audioManager != null && triggerAudioSource != null)
        {
            // Sync with PauseMenuManager or MenuManager
            var pauseMenuManager = FindObjectOfType<PauseMenuManager>();
            var menuManager = FindObjectOfType<MenuManager>();
            AudioSource sfxSource = pauseMenuManager != null ? pauseMenuManager.sfxSource : menuManager?.sfxSource;

            if (sfxSource != null)
            {
                // Apply volume multiplier only if sound is played from soundSourceObject
                float volume = soundSourceObject != null ? sfxSource.volume * volumeMultiplier : sfxSource.volume;
                triggerAudioSource.volume = Mathf.Clamp01(volume); // Ensure volume doesn't exceed 1
                triggerAudioSource.mute = sfxSource.mute; // Sync mute state
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger (assuming player has tag "Player")
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (audioManager != null && triggerSound != null)
            {
                audioManager.PlaySFX(triggerSound, triggerAudioSource); // Play 3D sound
                hasTriggered = true; // Prevent replaying
            }
        }
    }
}