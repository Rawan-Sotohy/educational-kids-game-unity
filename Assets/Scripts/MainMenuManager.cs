using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Game Buttons")]
    public Button memoryMatchButton;
    public Button puzzleGameButton;
    public Button mathGameButton;
    public Image characterAvatar;

    void Start()
    {
        // Add button listeners
        memoryMatchButton.onClick.AddListener(() => SceneLoader.Instance.LoadMemoryMatch());
        puzzleGameButton.onClick.AddListener(() => SceneLoader.Instance.LoadPuzzleGame());
        mathGameButton.onClick.AddListener(() => SceneLoader.Instance.LoadMathGame());

        // TODO: Load character avatar from Firebase when character selection is done
        // For now, leave it as placeholder
    }

    void OpenSettings()
    {
        AudioManager.Instance.PlayButtonClick();
        SettingsManager.Instance.OpenSettings();
    }

    void Logout()
    {
        AudioManager.Instance.PlayButtonClick();

        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.LogoutUser();
        }

        SceneLoader.Instance.LoadLogin();
    }
}