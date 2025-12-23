using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Game Buttons")]
    public Button memoryMatchButton;
    public Button puzzleGameButton;
    public Button mathGameButton;

    void Start()
    {
        memoryMatchButton.onClick.AddListener(() => {
            SettingsManager.Instance.PlayButtonClick();
            SceneManager.LoadScene("MemoryMatch");
        });

        puzzleGameButton.onClick.AddListener(() => {
            SettingsManager.Instance.PlayButtonClick();
            SceneManager.LoadScene("PuzzleGame");
        });

        mathGameButton.onClick.AddListener(() => {
            SettingsManager.Instance.PlayButtonClick();
            SceneManager.LoadScene("MathGame");
        });

    }    
}