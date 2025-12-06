using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public InputField emailInput;
    public InputField passwordInput;
    public Text errorText;
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

        // TODO: Firebase login after TA responds
        // For now, just go to character selection
        Debug.Log("Login clicked: " + email);

        // TEMPORARY - remove this after Firebase ready
        TempLogin();
    }

    public void OnRegisterClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // Validate input
        if (!ValidateInput(email, password))
            return;

        // TODO: Firebase register after TA responds
        Debug.Log("Register clicked: " + email);

        // TEMPORARY - remove this after Firebase ready
        TempRegister();
    }

    bool ValidateInput(string email, string password)
    {
        // Check if empty
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

        // Check email format (basic)
        if (!email.Contains("@") || !email.Contains("."))
        {
            ShowError("That doesn't look like an email!");
            return false;
        }

        // Check password length
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
        // Play error sound if available
        if (AudioManager.Instance != null)
        {
            // AudioManager.Instance.PlaySFX("wrong"); // Add when audio ready
        }
    }

    void TempLogin()
    {
        // TEMPORARY - goes to main menu
        // After Firebase: check if character selected, then redirect
        ShowError(""); // Clear error
        SceneLoader.Instance.LoadCharacterSelection();
    }

    void TempRegister()
    {
        // TEMPORARY - goes to character selection for first time
        ShowError(""); // Clear error
        SceneLoader.Instance.LoadCharacterSelection();
    }

    void AddClickSound(Button button)
    {
        button.onClick.AddListener(() => {
            if (AudioManager.Instance != null)
            {
                // AudioManager.Instance.PlaySFX("click"); // Add when audio ready
            }
        });
    }
}