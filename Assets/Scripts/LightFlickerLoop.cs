using System.Collections;
using UnityEngine;

public class LightFlickerLoop : MonoBehaviour
{
    public Light spotlight;
    public float flickerMin = 0.05f;
    public float flickerMax = 0.2f;
    public float steadyTime = 5f;
    public AudioClip vfxClips; // Flicker sound clip
    private AudioManager audioManager;
    private AudioSource vfxAudioSource; // AudioSource attached to this GameObject

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();
            if (spotlight == null)
            {
                Debug.LogError("Light component not found on this GameObject!");
                enabled = false;
            }
        }

        // Add and configure AudioSource for 3D sound
        vfxAudioSource = gameObject.AddComponent<AudioSource>();
        vfxAudioSource.spatialBlend = 1f; // Set to 3D audio
        vfxAudioSource.minDistance = 1f; // Distance where volume is max
        vfxAudioSource.maxDistance = 10f; // Distance where volume drops to 0
        vfxAudioSource.rolloffMode = AudioRolloffMode.Logarithmic; // Smooth volume falloff
    }

    private void Start()
    {
        StartCoroutine(FlickerLoop());
    }

    private void Update()
    {
        if (audioManager != null && vfxAudioSource != null)
        {
            // Check for PauseMenuManager or MenuManager to sync audio state
            var pauseMenuManager = FindObjectOfType<PauseMenuManager>();
            var menuManager = FindObjectOfType<MenuManager>();
            AudioSource sfxSource = pauseMenuManager != null ? pauseMenuManager.sfxSource : menuManager?.sfxSource;

            if (sfxSource != null)
            {
                vfxAudioSource.volume = sfxSource.volume; // Sync volume with sfxSource
                vfxAudioSource.mute = sfxSource.mute; // Sync mute state with sfxSource
            }
        }
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            int flickerCount = Random.Range(1, 3);

            for (int i = 0; i < flickerCount * 2; i++)
            {
                spotlight.enabled = !spotlight.enabled;
                if (audioManager != null && vfxClips != null)
                {
                    audioManager.PlaySFX(vfxClips, vfxAudioSource); // Play sound using local AudioSource
                }
                float wait = Random.Range(flickerMin, flickerMax);
                yield return new WaitForSeconds(wait);
            }

            spotlight.enabled = true;
            yield return new WaitForSeconds(steadyTime);
        }
    }
}