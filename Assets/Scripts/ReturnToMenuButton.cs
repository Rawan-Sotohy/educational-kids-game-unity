using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuButton : MonoBehaviour
{
    public void OnReturnToMenuClicked()
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.PlayButtonClick();

        SceneManager.LoadScene("MainMenu");
    }
}
