using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameTrigger : MonoBehaviour
{
    public Canvas endGameCanvas; // Canvas containing fade image and End text
    public Image fadeImage; // Image for fade-to-black effect
    public Text endText; // Text to display "End"
    public float fadeDuration = 2f; // Duration for fade-to-black
    public float endTextDisplayTime = 2f; // Time to display "End" before loading MenuScene
    private AudioManager audioManager;
    private bool hasTriggered = false;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

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

        // Initialize UI elements
        if (endGameCanvas != null)
        {
            endGameCanvas.gameObject.SetActive(false);
        }
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            fadeImage.color = new Color(color.r, color.g, color.b, 0f); // Start fully transparent
        }
        if (endText != null)
        {
            Color color = endText.color;
            endText.color = new Color(color.r, color.g, color.b, 0f); // Start fully transparent
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(EndGameSequence());
        }
    }

    private IEnumerator EndGameSequence()
    {
        // Disable player movement
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<StarterAssets.FirstPersonController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
        }

        // Activate canvas and start fade
        if (endGameCanvas != null)
        {
            endGameCanvas.gameObject.SetActive(true);
        }

        // Fade to black
        if (fadeImage != null)
        {
            float elapsedTime = 0f;
            Color startColor = fadeImage.color;
            Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Fully opaque
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
                // Optionally reduce music volume
                if (audioManager != null && audioManager.musicAudioSource != null)
                {
                    audioManager.musicAudioSource.volume = Mathf.Lerp(PlayerPrefs.GetFloat("MusicVolume", 1f), 0f, elapsedTime / fadeDuration);
                }
                yield return null;
            }
            fadeImage.color = targetColor;
        }

        // Fade in End text
        if (endText != null)
        {
            float elapsedTime = 0f;
            Color startColor = endText.color;
            Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Fully opaque
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                endText.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
                yield return null;
            }
            endText.color = targetColor;
        }

        // Wait for End text to display
        yield return new WaitForSeconds(endTextDisplayTime);
        UIController.UnlockCursor();
        // Restore music volume and load MenuScene
        if (audioManager != null && audioManager.musicAudioSource != null)
        {
            audioManager.musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        }
        SceneManager.LoadScene("MenuScene");
    }
}
