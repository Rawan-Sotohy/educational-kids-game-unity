using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;
    public Button loginButton;
    public Button registerButton;

    void Start()
    {
        // Hide error on start
        errorText.text = "";

        // Add button listeners
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);

        // Play click sound on buttons
        AddClickSound(loginButton);
        AddClickSound(registerButton);
    }

    public void OnLoginClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // Validate input
        if (!ValidateInput(email, password))
            return;

        Debug.Log("Login clicked: " + email);

        // TEMPORARY
        TempLogin();
    }

    public void OnRegisterClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // Validate input
        if (!ValidateInput(email, password))
            return;

        Debug.Log("Register clicked: " + email);

        // TEMPORARY
        TempRegister();
    }

    bool ValidateInput(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            ShowError("Oops! Please enter your email!");
            return false;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowError("Oops! Please enter a password!");
            return false;
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            ShowError("That doesn't look like an email!");
            return false;
        }

        if (password.Length < 6)
        {
            ShowError("Password must be at least 6 characters!");
            return false;
        }

        return true;
    }

    void ShowError(string message)
    {
        errorText.text = message;

        if (AudioManager.Instance != null)
        {
            // AudioManager.Instance.PlaySFX("wrong");
        }
    }

    void TempLogin()
    {
        ShowError("");
        SceneLoader.Instance.LoadCharacterSelection();
    }

    void TempRegister()
    {
        ShowError("");
        SceneLoader.Instance.LoadCharacterSelection();
    }

    void AddClickSound(Button button)
    {
        button.onClick.AddListener(() =>
        {
            if (AudioManager.Instance != null)
            {
                // AudioManager.Instance.PlaySFX("click");
            }
        });
    }
}
