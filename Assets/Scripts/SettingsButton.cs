using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(SettingsManager.Instance.OpenSettings);
    }

}