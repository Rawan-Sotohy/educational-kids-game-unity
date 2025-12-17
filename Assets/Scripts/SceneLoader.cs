using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    void Awake()
    {
        // Singleton pattern
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

    // Scene loading methods
    public void LoadLoading()
    {
        LoadScene("Loading");
    }

    public void LoadLogin()
    {
        LoadScene("Login");
    }

    public void LoadCharacterSelection()
    {
        LoadScene("CharacterSelection");
    }

    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void LoadMemoryMatch()
    {
        LoadScene("MemoryMatch");
    }

    public void LoadPuzzleGame()
    {
        LoadScene("PuzzleGame");
    }

    public void LoadMathGame()
    {
        LoadScene("MathGame");
    }

    //public void LoadSettings()
    //{
    //    LoadScene("Settings");
    //}

    void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // Reload current scene
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        LoadScene(currentScene.name);
    }

    // Quit game (for testing/mobile)
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}