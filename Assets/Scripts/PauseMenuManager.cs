using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingPanel;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public Text audioToggleText;
    public Text musicToggleText;

    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Player Control")]
    public GameObject playerScripts; // Reference to the GameObject with FirstPersonController

    private bool isPaused = false;
    private bool isMusicOn = true;
    private bool isAudioOn = true;

    private AudioManager audioManager;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
        settingPanel.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (playerScripts == null)
            playerScripts = GameObject.FindGameObjectWithTag("Player"); // Find player if not assigned
        if (playerScripts == null)
            Debug.LogWarning("PlayerScripts not assigned or found with tag 'Player'!");

        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSource.mute = PlayerPrefs.GetInt("MusicMute", 0) == 1;
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxSource.mute = PlayerPrefs.GetInt("SFXMute", 0) == 1;

        // Initialize audio states based on AudioSource
        isMusicOn = !musicSource.mute;
        isAudioOn = !sfxSource.mute || !musicSource.mute;
        musicSource.volume = Mathf.Clamp(musicSource.volume, 0f, 1f);
        sfxSource.volume = Mathf.Clamp(sfxSource.volume, 0f, 1f);

        // Initialize sliders
        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;

        // Update UI text
        UpdateMusicToggleText();
        UpdateAudioToggleText();

        // Ensure sliders are linked to methods
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void UpdateMusicToggleText()
    {
        musicToggleText.text = "MUSIC: " + (isMusicOn ? "ON" : "OFF");
    }

    private void UpdateAudioToggleText()
    {
        isAudioOn = !sfxSource.mute || !musicSource.mute;
        audioToggleText.text = "AUDIO: " + (isAudioOn ? "ON" : "OFF");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        UIController.UnlockCursor();
        if (playerScripts != null)
            playerScripts.GetComponent<FirstPersonController>().enabled = false; // Disable player controller
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        settingPanel.SetActive(false);
        Time.timeScale = 1f;
        UIController.LockCursor();
        if (playerScripts != null)
            playerScripts.GetComponent<FirstPersonController>().enabled = true; // Enable player controller
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void OnSettingButton()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        pauseMenuPanel.SetActive(false);
        settingPanel.SetActive(true);
        if (playerScripts != null)
            playerScripts.GetComponent<FirstPersonController>().enabled = false; // Disable player controller in settings
    }

    public void OnBackButton()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        settingPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        if (playerScripts != null)
            playerScripts.GetComponent<FirstPersonController>().enabled = false; // Keep player controller disabled in pause menu
    }

    public void SaveGame()
    {
        if (playerScripts != null)
        {
            Vector3 playerPosition = playerScripts.transform.position;
            PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
            PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
            PlayerPrefs.SetFloat("PlayerZ", playerPosition.z);
            PlayerPrefs.Save();
            // Verify save
            string savedScene = PlayerPrefs.GetString("SavedScene", "");
            float savedX = PlayerPrefs.GetFloat("PlayerX", 0f);
            Debug.Log($"Game Saved! Scene: {savedScene}, Position: ({savedX}, {playerPosition.y}, {playerPosition.z})");
        }
        else
        {
            Debug.LogWarning("PlayerScripts not found! Save failed.");
        }
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
    }

    public void ToggleMusic()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        isMusicOn = !isMusicOn;
        musicSource.mute = !isMusicOn;
        musicSlider.value = isMusicOn ? musicSource.volume : 0f;
        UpdateMusicToggleText();
        UpdateAudioToggleText();
        PlayerPrefs.SetInt("MusicMute", musicSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleAudio()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        isAudioOn = !isAudioOn;
        sfxSource.mute = !isAudioOn;
        musicSource.mute = !isAudioOn;
        isMusicOn = isAudioOn; // Sync music state with audio
        musicSlider.value = isAudioOn ? sfxSlider.value : 0f; // Sync music slider with audio
        sfxSlider.value = isAudioOn ? sfxSource.volume : 0f;
        UpdateMusicToggleText();
        UpdateAudioToggleText();

        PlayerPrefs.SetInt("MusicMute", musicSource.mute ? 1 : 0);
        PlayerPrefs.SetInt("SFXMute", sfxSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        isMusicOn = value > 0.001f;
        musicSource.mute = !isMusicOn;
        UpdateMusicToggleText();
        UpdateAudioToggleText();
        PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
        PlayerPrefs.SetInt("MusicMute", musicSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        sfxSource.mute = value <= 0.001f;
        // Always sync music volume with audio, regardless of isMusicOn
        musicSource.volume = value;
        musicSlider.value = value;
        isMusicOn = value > 0.001f; // Update music state based on volume
        musicSource.mute = !isMusicOn;
        UpdateMusicToggleText();
        UpdateAudioToggleText();
        PlayerPrefs.SetFloat("SFXVolume", sfxSource.volume);
        PlayerPrefs.SetInt("SFXMute", sfxSource.mute ? 1 : 0);
        PlayerPrefs.SetInt("MusicMute", musicSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }
}