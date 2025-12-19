using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{
    public void OnLogoutClicked()
    {
        Debug.Log("👋 Logout clicked");
        SettingsManager.Instance.PlayButtonClick();
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.LogoutUser();
        }

        SceneManager.LoadScene("Login");
    }
}
