using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_Text errorText;
    public Button loginButton;
    public Button registerButton;

    bool isProcessing = false;

    void Start()
    {
        errorText.text = "";

        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);

        SetButtons(false);
        errorText.text = "Connecting...";

        if (FirebaseManager.Instance.isFirebaseReady)
        {
            OnFirebaseReady();
        }
        else
        {
            FirebaseManager.Instance.OnFirebaseInitialized += OnFirebaseReady;
        }
    }

    void OnDestroy()
    {
        if (FirebaseManager.Instance != null)
            FirebaseManager.Instance.OnFirebaseInitialized -= OnFirebaseReady;
    }

    void OnFirebaseReady()
    {
        errorText.text = "";
        SetButtons(true);
    }

    public void OnLoginClicked()
    {
        if (isProcessing) return;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (!ValidateLogin(email, password)) return;

        isProcessing = true;
        SetButtons(false);
        errorText.text = "Logging in...";

        FirebaseManager.Instance.LoginUser(email, password, OnLoginResult);
    }

    public void OnRegisterClicked()
    {
        if (isProcessing) return;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        string email = emailInput.text.Trim();
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (!ValidateRegistration(email, password, confirmPassword)) return;

        isProcessing = true;
        SetButtons(false);
        errorText.text = "Creating account...";

        FirebaseManager.Instance.RegisterUser(email, password, OnRegisterResult);
    }

    void OnLoginResult(bool success, string message)
    {
        isProcessing = false;

        if (success)
        {
            // Load character data after successful login
            if (CharacterManager.Instance != null)
            {
                Debug.Log("🔄 Loading character data after login...");
                CharacterManager.Instance.LoadFromFirebase();
            }

            SceneLoader.Instance.LoadMainMenu();
        }
        else
        {
            errorText.text = message;
            SetButtons(true);
        }
    }

    void OnRegisterResult(bool success, string message)
    {
        isProcessing = false;

        if (success)
        {
            // Set defaults for new user
            if (CharacterManager.Instance != null)
            {
                CharacterManager.Instance.playerName = "Player";
                CharacterManager.Instance.selectedCharacter = 0;
                CharacterManager.Instance.SaveToFirebase();
                Debug.Log("✅ Saved defaults for new user");
            }

            SceneLoader.Instance.LoadCharacterSelection();
        }
        else
        {
            errorText.text = message;
            SetButtons(true);
        }
    }

    bool ValidateLogin(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorText.text = "Fill all fields!";
            return false;
        }

        if (!email.Contains("@"))
        {
            errorText.text = "Invalid email!";
            return false;
        }

        if (password.Length < 6)
        {
            errorText.text = "Password too short!";
            return false;
        }

        return true;
    }

    bool ValidateRegistration(string email, string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            errorText.text = "Fill all fields!";
            return false;
        }

        if (!email.Contains("@"))
        {
            errorText.text = "Invalid email!";
            return false;
        }

        if (password.Length < 6)
        {
            errorText.text = "Password must be at least 6 characters!";
            return false;
        }

        if (password != confirmPassword)
        {
            errorText.text = "Passwords do not match!";
            return false;
        }

        return true;
    }

    void SetButtons(bool value)
    {
        loginButton.interactable = value;
        registerButton.interactable = value;
    }
}