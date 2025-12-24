using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip buttonClick;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip cardFlip;
    public AudioClip puzzleSnap;
    public AudioClip winFanfare;
    public AudioClip loseSound;

    private GameObject settingsPopup;
    private Slider musicSlider;
    private Slider sfxSlider;
    private Button musicMuteButton;
    private Button sfxMuteButton;
    private Image musicMuteIcon;
    private Image sfxMuteIcon;
    private Button closeButtonTop;
    private Button closeButtonBottom;

    [Header("Sprites")]
    public Sprite musicUnmutedSprite;
    public Sprite musicMutedSprite;
    public Sprite sfxUnmutedSprite;
    public Sprite sfxMutedSprite;

    private bool musicIsMuted = false;
    private bool sfxIsMuted = false;
    private float musicPreviousVolume = 1f;
    private float sfxPreviousVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayButtonClick() => sfxSource.PlayOneShot(buttonClick);
    public void PlayCorrect() => sfxSource.PlayOneShot(correctSound);
    public void PlayWrong() => sfxSource.PlayOneShot(wrongSound);
    public void PlayCardFlip() => sfxSource.PlayOneShot(cardFlip);
    public void PlayPuzzleSnap() => sfxSource.PlayOneShot(puzzleSnap);
    public void PlayWinFanfare() => sfxSource.PlayOneShot(winFanfare);
    public void PlayLoseSound() => sfxSource.PlayOneShot(loseSound);

    void FindPopupInScene()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in canvases)
        {
            Transform popup = canvas.transform.Find("SettingsPopUp");
            if (popup != null)
            {
                settingsPopup = popup.gameObject;

                musicSlider = popup.Find("MusicSlider")?.GetComponent<Slider>();
                sfxSlider = popup.Find("SFXSlider")?.GetComponent<Slider>();
                musicMuteButton = popup.Find("MusicMuteButton")?.GetComponent<Button>();
                sfxMuteButton = popup.Find("SFXMuteButton")?.GetComponent<Button>();
                closeButtonTop = popup.Find("CloseButton")?.GetComponent<Button>();
                closeButtonBottom = popup.Find("CharacterButton")?.GetComponent<Button>();

                musicMuteIcon = musicMuteButton.transform.GetChild(0)?.GetComponent<Image>();
                sfxMuteIcon = sfxMuteButton.transform.GetChild(0)?.GetComponent<Image>();

                return;
            }
        }
    }
    void SetupPopup()
    {
        settingsPopup.SetActive(false);

        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        musicMuteButton.onClick.RemoveAllListeners();
        sfxMuteButton.onClick.RemoveAllListeners();
        closeButtonTop.onClick.RemoveAllListeners();
        closeButtonBottom.onClick.RemoveAllListeners();

        if (SettingsManager.Instance != null)
        {
            musicSlider.value = musicSource.volume;
            sfxSlider.value = sfxSource.volume;

            musicPreviousVolume = musicSlider.value > 0 ? musicSlider.value : 1f;
            sfxPreviousVolume = sfxSlider.value > 0 ? sfxSlider.value : 1f;
        }

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicMuteButton.onClick.AddListener(ToggleMusicMute);
        sfxMuteButton.onClick.AddListener(ToggleSFXMute);
        closeButtonTop.onClick.AddListener(CloseSettings);
        closeButtonBottom.onClick.AddListener(GoToCharacterSelection);

        UpdateMusicMuteIcon();
        UpdateSFXMuteIcon();
    }

    public void OpenSettings()
    {
        FindPopupInScene();

        if (settingsPopup == null)
        {
            Debug.LogError("SettingsPopup not found in current scene!");
            return;
        }

        SetupPopup();

        settingsPopup.SetActive(true);
        PlayButtonClick();
    }

    public void CloseSettings()
    {
        settingsPopup.SetActive(false);
        PlayButtonClick();
    }

    public void GoToCharacterSelection()
    {
        settingsPopup.SetActive(false);
        PlayButtonClick();

        SceneManager.LoadScene("CharacterSelection");
    }

    void OnMusicVolumeChanged(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);

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
        musicIsMuted = !musicIsMuted;

        if (musicIsMuted)
        {
            musicPreviousVolume = musicSource.volume;
            musicSource.volume = 0f;
        }
        else
        {
            musicSource.volume = musicPreviousVolume;
        }

        musicSlider.value = musicSource.volume;
        PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);

        UpdateMusicMuteIcon();
        PlayButtonClick();
    }

    void UpdateMusicMuteIcon()
    {
        bool muted = musicSource.volume == 0f;
        musicMuteIcon.sprite = muted ? musicMutedSprite : musicUnmutedSprite;
    }

    void OnSFXVolumeChanged(float volume)
    {
        sfxSource.volume = volume;
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

        if (volume > 0)
            PlayButtonClick();
    }

    void ToggleSFXMute()
    {
        sfxIsMuted = !sfxIsMuted;

        if (sfxIsMuted)
        {
            sfxPreviousVolume = sfxSource.volume;
            sfxSource.volume = 0f;
        }
        else
        {
            sfxSource.volume = sfxPreviousVolume;
        }

        sfxSlider.value = sfxSource.volume;
        PlayerPrefs.SetFloat("SFXVolume", sfxSource.volume);

        UpdateSFXMuteIcon();

        if (!sfxIsMuted)
            PlayButtonClick();
    }

    void UpdateSFXMuteIcon()
    {
        bool muted = sfxSource.volume == 0f;
        sfxMuteIcon.sprite = muted ? sfxMutedSprite : sfxUnmutedSprite;
    }

}