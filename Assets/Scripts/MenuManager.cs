using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using StarterAssets;

public class MenuManager : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingPanel;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("UI Texts")]
    public Text audioToggleText;
    public Text musicToggleText;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isMusicOn = true;
    private bool isAudioOn = true;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        Debug.Log("MenuManager Awake - Checking PlayerPrefs: SavedScene = " + PlayerPrefs.GetString("SavedScene", "None"));
    }

    void Start()
    {
        mainMenuPanel.SetActive(true);
        settingPanel.SetActive(false);
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSource.mute = PlayerPrefs.GetInt("MusicMute", 0) == 1;
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxSource.mute = PlayerPrefs.GetInt("SFXMute", 0) == 1;
        isMusicOn = !musicSource.mute;
        isAudioOn = !sfxSource.mute || !musicSource.mute;
        musicSource.volume = Mathf.Clamp(musicSource.volume, 0f, 1f);
        sfxSource.volume = Mathf.Clamp(sfxSource.volume, 0f, 1f);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;

        UpdateMusicToggleText();
        UpdateAudioToggleText();

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

    // ----- Continue button -----
    public void OnContinueButton()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        string savedScene = PlayerPrefs.GetString("SavedScene", "");
        float savedX = PlayerPrefs.GetFloat("PlayerX", 0f);
        Debug.Log($"Loading saved scene: {savedScene}, PlayerX: {savedX}, PlayerY: {PlayerPrefs.GetFloat("PlayerY", 0f)}, PlayerZ: {PlayerPrefs.GetFloat("PlayerZ", 0f)}");
        if (!string.IsNullOrEmpty(savedScene) && savedX != 0f)
        {
            float playerX = PlayerPrefs.GetFloat("PlayerX", 0f);
            float playerY = PlayerPrefs.GetFloat("PlayerY", 0f);
            float playerZ = PlayerPrefs.GetFloat("PlayerZ", 0f);
            Debug.Log($"Loading scene {savedScene} with position ({playerX}, {playerY}, {playerZ})");
            FirstPersonController.IsLoadingFromSave = true; 
            SceneManager.LoadScene(savedScene, LoadSceneMode.Single); 
            StartCoroutine(SetPlayerPositionAfterLoad(playerX, playerY, playerZ));
        }
        else
        {
            Debug.LogWarning("No valid saved game found! Starting new game instead.");
            OnNewGameButton();
        }
    }

    private IEnumerator SetPlayerPositionAfterLoad(float x, float y, float z)
    {
        yield return null; // Wait one frame
        yield return new WaitForSeconds(0.2f); 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(x, y, z);
            Debug.Log($"Player loaded at position: {player.transform.position}");
        }
        else
        {
            Debug.LogError("Player not found after loading scene! Check if 'Player' tag is assigned.");
        }
        yield return new WaitForEndOfFrame(); 
        FirstPersonController.IsLoadingFromSave = false; 
    }

    // ----- New Game button -----
    public void OnNewGameButton()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        FirstPersonController.IsLoadingFromSave = false; 
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    // ----- Quit button -----
    public void OnQuitButton()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ----- Setting button -----
    public void OnSettingButton()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        mainMenuPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    // ----- Back button from Setting to Main Menu -----
    public void OnBackButton()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        settingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ----- Toggle ON/OFF Music -----
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

    // ----- Toggle ON OFF Audio -----
    public void ToggleAudio()
    {
        audioManager?.PlaySFX(audioManager.buttonClickClip, sfxSource);
        isAudioOn = !isAudioOn;
        musicSource.mute = !isAudioOn;
        sfxSource.mute = !isAudioOn;
        isMusicOn = isAudioOn;
        musicSlider.value = isAudioOn ? musicSource.volume : 0f;
        sfxSlider.value = isAudioOn ? sfxSource.volume : 0f;
        UpdateMusicToggleText();
        UpdateAudioToggleText();
        PlayerPrefs.SetInt("MusicMute", musicSource.mute ? 1 : 0);
        PlayerPrefs.SetInt("SFXMute", sfxSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ----- Adjust music via Slider -----
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

    // ----- Adjust SFX via Slider -----
    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        sfxSource.mute = value <= 0.001f;
        musicSource.volume = value;
        musicSlider.value = value;
        isMusicOn = value > 0.001f;
        musicSource.mute = !isMusicOn;
        UpdateMusicToggleText();
        UpdateAudioToggleText();
        PlayerPrefs.SetFloat("SFXVolume", sfxSource.volume);
        PlayerPrefs.SetInt("SFXMute", sfxSource.mute ? 1 : 0);
        PlayerPrefs.SetInt("MusicMute", musicSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }
}