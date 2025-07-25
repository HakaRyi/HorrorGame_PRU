using UnityEngine;
using System.Collections;

public class GhostTrigger : MonoBehaviour
{
    public GameObject ghostPrefab; // Prefab of the zombie to spawn
    public Transform spawnPoint; // Transform defining spawn position and rotation
    public float displayDuration = 1f; // Time zombie is visible (seconds)
    private AudioManager audioManager;
    private bool hasTriggered = false;
    private AudioSource ghostAudioSource; // For 3D zombie SFX

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        Debug.Log($"AudioManager: {(audioManager != null ? audioManager.gameObject.name : "null")}");

        // Ensure the GameObject has a BoxCollider with IsTrigger enabled
        var collider = GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }
        else
        {
            collider.isTrigger = true;
        }

        // Add and configure AudioSource for 3D sound
        ghostAudioSource = gameObject.AddComponent<AudioSource>();
        ghostAudioSource.spatialBlend = 1f; // 3D audio
        ghostAudioSource.minDistance = 1f;
        ghostAudioSource.maxDistance = 10f;
        ghostAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        ghostAudioSource.playOnAwake = false;
        Debug.Log($"Ghost AudioSource initialized on {gameObject.name}");
    }

    void Start()
    {
        // Initial sync with AudioManager's vfxAudioSource
        SyncAudioSettings();
        // Validate spawnPoint
        if (spawnPoint == null)
        {
            Debug.LogWarning("SpawnPoint not assigned in GhostTrigger! Zombie may not spawn correctly.");
        }
    }

    private void SyncAudioSettings()
    {
        if (audioManager != null && audioManager.vfxAudioSource != null)
        {
            ghostAudioSource.volume = audioManager.vfxAudioSource.volume;
            ghostAudioSource.mute = audioManager.vfxAudioSource.mute;
            Debug.Log($"Ghost AudioSource synced with vfxAudioSource: Volume={ghostAudioSource.volume}, Mute={ghostAudioSource.mute}, PlayerPrefs: SFXVolume={PlayerPrefs.GetFloat("SFXVolume", 1f)}, SFXMute={PlayerPrefs.GetInt("SFXMute", 0)}");
        }
        else
        {
            // Fallback if vfxAudioSource not found
            ghostAudioSource.volume = 1f; // Default volume
            ghostAudioSource.mute = false; // Default unmute
            Debug.LogWarning("No vfxAudioSource found in AudioManager! Using default audio settings.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            Debug.Log($"Trigger activated by Player at {transform.position}");
            StartCoroutine(ShowGhost());
        }
    }

    private IEnumerator ShowGhost()
    {
        if (ghostPrefab != null && spawnPoint != null)
        {
            // Spawn zombie at spawnPoint's position and rotation
            GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Zombie spawned at {spawnPoint.position}, Rotation: {spawnPoint.rotation.eulerAngles}");

            // Sync audio settings before playing SFX
            SyncAudioSettings();

            // Play SFX with fallback
            if (audioManager != null && audioManager.ghostSfxClip != null)
            {
                Debug.Log($"Playing ghost SFX: {audioManager.ghostSfxClip.name}, Volume: {ghostAudioSource.volume}, Mute: {ghostAudioSource.mute}");
                if (ghostAudioSource.mute || ghostAudioSource.volume <= 0)
                {
                    ghostAudioSource.volume = Mathf.Max(ghostAudioSource.volume, 0.5f); // Minimum volume
                    ghostAudioSource.mute = false; // Force unmute
                    Debug.LogWarning("Forcing audio settings: Volume set to 0.5, Mute set to false due to mute or zero volume.");
                }
                audioManager.PlaySFX(audioManager.ghostSfxClip, ghostAudioSource);
            }
            else
            {
                Debug.LogWarning($"Cannot play SFX: AudioManager={(audioManager != null ? "exists" : "null")}, GhostSfxClip={(audioManager?.ghostSfxClip != null ? audioManager.ghostSfxClip.name : "null")}");
            }

            // Wait for display duration
            yield return new WaitForSeconds(displayDuration);

            // Destroy zombie
            if (ghost != null)
            {
                Destroy(ghost);
                Debug.Log("Zombie destroyed");
            }
        }
        else
        {
            Debug.LogError($"Cannot spawn zombie! GhostPrefab: {(ghostPrefab != null ? ghostPrefab.name : "null")}, SpawnPoint: {(spawnPoint != null ? spawnPoint.name : "null")}");
        }
    }
}