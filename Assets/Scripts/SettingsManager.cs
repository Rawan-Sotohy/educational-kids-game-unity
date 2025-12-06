using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Music Controls")]
    public Slider musicSlider;
    public Button musicMuteButton;
    public Image musicMuteIcon;
    public Sprite musicUnmutedSprite;
    public Sprite musicMutedSprite;

    [Header("SFX Controls")]
    public Slider sfxSlider;
    public Button sfxMuteButton;
    public Image sfxMuteIcon;
    public Sprite sfxUnmutedSprite;
    public Sprite sfxMutedSprite;

    [Header("Other UI")]
    public Button resetButton;
    public Button backButton;
    public GameObject confirmPopup;
    public Button yesButton;
    public Button noButton;

    private bool musicIsMuted = false;
    private bool sfxIsMuted = false;
    private float musicPreviousVolume = 1f;
    private float sfxPreviousVolume = 1f;

    void Start()
    {
        confirmPopup.SetActive(false);

        // Load saved volumes
        if (AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.musicSource.volume;
            sfxSlider.value = AudioManager.Instance.sfxSource.volume;

            musicPreviousVolume = musicSlider.value > 0 ? musicSlider.value : 1f;
            sfxPreviousVolume = sfxSlider.value > 0 ? sfxSlider.value : 1f;
        }

        // Add listeners
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicMuteButton.onClick.AddListener(ToggleMusicMute);
        sfxMuteButton.onClick.AddListener(ToggleSFXMute);
        resetButton.onClick.AddListener(OnResetClicked);
        backButton.onClick.AddListener(OnBackClicked);
        yesButton.onClick.AddListener(OnResetConfirmed);
        noButton.onClick.AddListener(OnResetCancelled);

        // Update button visuals
        UpdateMusicMuteIcon();
        UpdateSFXMuteIcon();
    }

    // ==================== MUSIC ====================

    void OnMusicVolumeChanged(float volume)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);

        // Auto update mute state
        if (volume == 0f)
        {
            musicIsMuted = true;
        }
        else
        {
            musicIsMuted = false;
            musicPreviousVolume = volume;
        }

        UpdateMusicMuteIcon();
    }

    void ToggleMusicMute()
    {
        if (AudioManager.Instance == null) return;

        musicIsMuted = !musicIsMuted;

        if (musicIsMuted)
        {
            musicPreviousVolume = AudioManager.Instance.musicSource.volume;
            AudioManager.Instance.musicSource.volume = 0f;
        }
        else
        {
            AudioManager.Instance.musicSource.volume = musicPreviousVolume;
        }

        musicSlider.value = AudioManager.Instance.musicSource.volume;
        PlayerPrefs.SetFloat("MusicVolume", AudioManager.Instance.musicSource.volume);

        UpdateMusicMuteIcon();
        PlayClickSound();
    }

    void UpdateMusicMuteIcon()
    {
        if (musicMuteIcon == null) return;

        if (musicIsMuted || musicSlider.value == 0f)
        {
            musicMuteIcon.sprite = musicMutedSprite != null ? musicMutedSprite : musicMuteIcon.sprite;
            if (musicMutedSprite == null) musicMuteIcon.color = Color.red;
        }
        else
        {
            musicMuteIcon.sprite = musicUnmutedSprite != null ? musicUnmutedSprite : musicMuteIcon.sprite;
            if (musicUnmutedSprite == null) musicMuteIcon.color = Color.white;
        }
    }

    // ==================== SFX ====================

    void OnSFXVolumeChanged(float volume)
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);

        if (volume == 0f)
        {
            sfxIsMuted = true;
        }
        else
        {
            sfxIsMuted = false;
            sfxPreviousVolume = volume;
        }

        UpdateSFXMuteIcon();

        // Play test sound
        if (volume > 0)
            PlayClickSound();
    }

    void ToggleSFXMute()
    {
        if (AudioManager.Instance == null) return;

        sfxIsMuted = !sfxIsMuted;

        if (sfxIsMuted)
        {
            sfxPreviousVolume = AudioManager.Instance.sfxSource.volume;
            AudioManager.Instance.sfxSource.volume = 0f;
        }
        else
        {
            AudioManager.Instance.sfxSource.volume = sfxPreviousVolume;
        }

        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.Instance.sfxSource.volume);

        UpdateSFXMuteIcon();

        if (!sfxIsMuted)
            PlayClickSound();
    }

    void UpdateSFXMuteIcon()
    {
        if (sfxMuteIcon == null) return;

        if (sfxIsMuted || sfxSlider.value == 0f)
        {
            sfxMuteIcon.sprite = sfxMutedSprite != null ? sfxMutedSprite : sfxMuteIcon.sprite;
            if (sfxMutedSprite == null) sfxMuteIcon.color = Color.red;
        }
        else
        {
            sfxMuteIcon.sprite = sfxUnmutedSprite != null ? sfxUnmutedSprite : sfxMuteIcon.sprite;
            if (sfxUnmutedSprite == null) sfxMuteIcon.color = Color.white;
        }
    }

    // ==================== OTHER ====================

    void PlayClickSound()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
    }

    void OnResetClicked()
    {
        confirmPopup.SetActive(true);
        PlayClickSound();
    }

    void OnResetConfirmed()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        confirmPopup.SetActive(false);

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadLogin();
    }

    void OnResetCancelled()
    {
        confirmPopup.SetActive(false);
        PlayClickSound();
    }

    void OnBackClicked()
    {
        PlayClickSound();

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadMainMenu();
    }
}