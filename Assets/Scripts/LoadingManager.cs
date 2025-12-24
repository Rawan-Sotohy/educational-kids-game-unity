using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public TMP_Text loadingText;

    void Start()
    {
        StartCoroutine(LoadingFlow());
    }

    void OnCharacterLoaded()
    {
        CharacterManager.Instance.OnCharacterDataLoaded -= OnCharacterLoaded;
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator LoadingFlow()
    {
        loadingText.text = "Starting...";
        yield return new WaitForSeconds(0.5f);

        loadingText.text = "Connecting...";
        while (FirebaseManager.Instance == null || !FirebaseManager.Instance.isFirebaseReady)
        {
            yield return new WaitForSeconds(0.2f);
        }

        loadingText.text = "Checking login...";
        yield return new WaitForSeconds(0.5f);

        if (FirebaseManager.Instance.IsUserLoggedIn())
        {
            Debug.Log("Auto-login success");

            CharacterManager.Instance.OnCharacterDataLoaded += OnCharacterLoaded;
            CharacterManager.Instance.LoadFromFirebase();
        }
        else
        {
            SceneManager.LoadScene("Login");
        }
    }
}
