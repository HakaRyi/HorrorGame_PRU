using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
  
        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }
    // ----- New Game button -----
    public void OnNewGameButton()
    {
        audioManager.PlaySFX(audioManager.buttonClickClip);
        SceneManager.LoadScene("SampleScene");
    }

    // ----- Quit button -----
    public void OnQuitButton()
    {
        audioManager.PlaySFX(audioManager.buttonClickClip);
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // -----  Setting button -----
    public void OnSettingButton()
    {
        audioManager.PlaySFX(audioManager.buttonClickClip);
        mainMenuPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    // ----- Back buttton from Setting to Main Menu -----
    public void OnBackButton()
    {
        audioManager.PlaySFX(audioManager.buttonClickClip);
        settingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ----- Toggle ON/OFF Music -----
    public void ToggleMusic()
    {
        audioManager.PlaySFX(audioManager.buttonClickClip);
        isMusicOn = !isMusicOn;
        musicSource.mute = !isMusicOn;
        musicToggleText.text = "MUSIC: " + (isMusicOn ? "ON" : "OFF");
        musicSlider.value = isMusicOn ? musicSource.volume : 0;
    }

    // ----- Toggle ON OFF Audio -----
    public void ToggleAudio()
    {
        audioManager.PlaySFX(audioManager.buttonClickClip);
        isAudioOn = !isAudioOn;
        musicSource.mute = !isAudioOn;
        sfxSource.mute = !isAudioOn;
        audioToggleText.text = "AUDIO: " + (isAudioOn ? "ON" : "OFF");
        musicToggleText.text = "MUSIC: " + (isAudioOn ? "ON" : "OFF");
        isMusicOn = isAudioOn;
        musicSlider.value = isAudioOn ? musicSource.volume : 0;
        sfxSlider.value = isAudioOn ? sfxSource.volume : 0;
    }

    // ----- fix music qua Slider -----
    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        if (value <= 0.001f)
        {
            isMusicOn = false;
            musicSource.mute = true;
            musicToggleText.text = "MUSIC: OFF";
        }
        else
        {
            isMusicOn = true;
            musicSource.mute = false;
            musicToggleText.text = "MUSIC: ON";
        }
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        musicSource.volume = value;

        if (value <= 0.001f)
        {
            isAudioOn = false;
            isMusicOn = false;

            sfxSource.mute = true;
            musicSource.mute = true;

            audioToggleText.text = "AUDIO: OFF";
            musicToggleText.text = "MUSIC: OFF";

            musicSlider.value = 0;
        }
        else
        {
            isAudioOn = true;
            isMusicOn = true;

            sfxSource.mute = false;
            musicSource.mute = false;

            audioToggleText.text = "AUDIO: ON";
            musicToggleText.text = "MUSIC: ON";

            musicSlider.value = value;
        }
    }
}
