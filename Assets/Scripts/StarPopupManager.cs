using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarPopupManager : MonoBehaviour
{
    public static StarPopupManager Instance;

    public Sprite starEmpty;
    public Sprite starFull;

    private GameObject popupPanel;
    private TMP_Text winText;
    private Image[] stars;
    private TMP_Text scoreText;
    private Button playAgainButton;
    private Button mainMenuButton;

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
        FindPopupInScene();

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    void FindPopupInScene()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in canvases)
        {
            Transform popup = canvas.transform.Find("StarPopup");
            if (popup != null)
            {
                popupPanel = popup.gameObject;
                Debug.Log("Found StarPopup in scene!");

                winText = popup.Find("WinText")?.GetComponent<TMP_Text>();
                scoreText = popup.Find("ScoreText")?.GetComponent<TMP_Text>();
                playAgainButton = popup.Find("PlayAgainButton")?.GetComponent<Button>();
                mainMenuButton = popup.Find("MainMenuButton")?.GetComponent<Button>();
                Transform starsContainer = popup.Find("StarsContainer");

                stars = new Image[5];
                for (int i = 0; i < 5; i++)
                {
                    Transform star = starsContainer.Find("Star" + (i + 1));
                    stars[i] = star.GetComponent<Image>();
                }
                return;
            }
        }
    }

    public void ShowStars(int starCount, string message)
    {
        if (popupPanel == null)
        {
            FindPopupInScene();
            if (popupPanel != null)
                popupPanel.SetActive(false);
        }

        starCount = Mathf.Clamp(starCount, 0, 5);

        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
                stars[i].sprite = i < starCount ? starFull : starEmpty;
        }

        switch (starCount)
        {
            case 5:
                winText.text = "Amazing!";
                break;
            case 4:
                winText.text = "Great Job!";
                break;
            case 3:
                winText.text = "Nice Work!";
                break;
            case 2:
                winText.text = "Keep Practicing!";
                break;
            case 1:
                winText.text = "Almost There!";
                break;
            case 0:
                winText.text = "Uh-Oh!";
                break;
            default:
                winText.text = "";
                break;
        }

        scoreText.text = message;

        popupPanel.SetActive(true);

        if (starCount <= 1)
            SettingsManager.Instance.PlayLoseSound();
        else
            SettingsManager.Instance.PlayWinFanfare();
    }

    public void PlayAgain()
    {
        SettingsManager.Instance.PlayButtonClick();

        if (popupPanel != null)
            popupPanel.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SettingsManager.Instance.PlayButtonClick();

        if (popupPanel != null)
            popupPanel.SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }
}