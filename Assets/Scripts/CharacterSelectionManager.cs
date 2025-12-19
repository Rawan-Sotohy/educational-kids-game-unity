using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField nameInput;
    public Button character1Button;
    public Button character2Button;
    public Button confirmButton;
    public TMP_Text errorText;

    private int selectedChar = 0;

    void Start()
    {
        character1Button.onClick.AddListener(() => SelectCharacter(0));
        character2Button.onClick.AddListener(() => SelectCharacter(1));
        confirmButton.onClick.AddListener(Confirm);

        errorText.text = "";

        if (CharacterManager.Instance != null)
        {
            if (CharacterManager.Instance.IsDataLoaded)
            {
                ApplyLoadedData();
            }
            else
            {
                CharacterManager.Instance.OnCharacterDataLoaded += ApplyLoadedData;
            }
        }
    }

    void ApplyLoadedData()
    {
        if (CharacterManager.Instance == null) return;

        nameInput.text = CharacterManager.Instance.playerName;

        selectedChar = CharacterManager.Instance.selectedCharacter;
        SelectCharacter(selectedChar);

        Debug.Log($"🟢 UI updated: {nameInput.text}, Character {selectedChar}");

        CharacterManager.Instance.OnCharacterDataLoaded -= ApplyLoadedData;
    }


    void LoadCurrentCharacter()
    {
        if (CharacterManager.Instance != null)
        {
            nameInput.text = CharacterManager.Instance.playerName;

            selectedChar = CharacterManager.Instance.selectedCharacter;
            SelectCharacter(selectedChar);
        }
        else
        {
            // Fallback defaults
            nameInput.text = "Player";
            SelectCharacter(0);
        }
    }

    void SelectCharacter(int index)
    {
        if (CharacterManager.Instance == null) return;
        if (index < 0 || index >= CharacterManager.Instance.characterSprites.Length)
            index = 0;

        selectedChar = index;

        ResetButtonColors();

        Button selectedButton = index == 0 ? character1Button : character2Button;
        selectedButton.GetComponent<Image>().color = Color.yellow;

        if (SettingsManager.Instance != null)
            SettingsManager.Instance.PlayButtonClick();
    }

    void ResetButtonColors()
    {
        character1Button.GetComponent<Image>().color = Color.white;
        character2Button.GetComponent<Image>().color = Color.white;
    }

    void Confirm()
    {
        string name = nameInput.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            errorText.text = "Please enter a name!";
            return;
        }

        // Save to CharacterManager
        CharacterManager.Instance.playerName = name;
        CharacterManager.Instance.selectedCharacter = selectedChar;
        CharacterManager.Instance.SaveToFirebase();
        
        SettingsManager.Instance.PlayButtonClick();

        // Go to main menu
        SceneManager.LoadScene("MainMenu");
    }
}