using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// SIMPLE character selection - just pick name and 1 of 3 characters
/// Add to CharacterSelection scene
/// </summary>
public class CharacterSelectionManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField nameInput;
    public Button character1Button;
    public Button character2Button;
    public Button confirmButton;
    public Image previewImage; // Shows selected character
    public TMP_Text errorText;

    private int selectedChar = 0;

    void Start()
    {
        // Button clicks
        character1Button.onClick.AddListener(() => SelectCharacter(0));
        character2Button.onClick.AddListener(() => SelectCharacter(1));
        confirmButton.onClick.AddListener(Confirm);

        errorText.text = "";
        SelectCharacter(0); // Start with first character selected
    }

    void SelectCharacter(int index)
    {
        selectedChar = index;

        // Show preview
        if (CharacterManager.Instance != null && previewImage != null)
        {
            previewImage.sprite = CharacterManager.Instance.characterSprites[index];
        }

        // Visual feedback - highlight selected button
        ResetButtonColors();
        Button selectedButton = index == 0 ? character1Button : character2Button;
        selectedButton.GetComponent<Image>().color = Color.yellow;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
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
        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.playerName = name;
            CharacterManager.Instance.selectedCharacter = selectedChar;
            CharacterManager.Instance.SaveToFirebase();
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        // Go to main menu
        SceneLoader.Instance.LoadMainMenu();
    }
}