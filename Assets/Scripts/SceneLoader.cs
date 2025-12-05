using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu() => LoadScene("MainMenu");
    public void LoadLogin() => LoadScene("Login");
    public void LoadCharacterSelection() => LoadScene("CharacterSelection");
    public void LoadGameMenu() => LoadScene("GameMenu");
    public void LoadMemoryMatch() => LoadScene("MemoryMatch");
    public void LoadPuzzleGame() => LoadScene("PuzzleGame");
    public void LoadColorMatch() => LoadScene("ColorMatch");
    public void LoadSettings() => LoadScene("Settings");
}