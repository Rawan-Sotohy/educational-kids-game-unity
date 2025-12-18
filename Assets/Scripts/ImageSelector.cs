using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelector : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject imageSelectionPanel;
    [SerializeField] private GameObject puzzleGamePanel;

    [Header("Image Data")]
    [SerializeField] private PuzzleImageData[] puzzleImages;

    [Header("Buttons")]
    [SerializeField] private Button imageButton1;
    [SerializeField] private Button imageButton2;
    [SerializeField] private Button imageButton3;

    private PuzzleManager puzzleManager;

    void Start()
    {
        // Find PuzzleManager
        puzzleManager = FindObjectOfType<PuzzleManager>();

        if (puzzleManager == null)
        {
            Debug.LogError(" PuzzleManager not found in scene!");
            return;
        }

        // Check panels
        if (imageSelectionPanel == null)
        {
            Debug.LogError(" ImageSelectionPanel is not assigned!");
            return;
        }

        if (puzzleGamePanel == null)
        {
            Debug.LogError(" PuzzleGamePanel is not assigned!");
            return;
        }

        // Check buttons
        if (imageButton1 == null || imageButton2 == null || imageButton3 == null)
        {
            Debug.LogError(" One or more buttons are not assigned!");
            return;
        }

        // Setup button listeners
        imageButton1.onClick.AddListener(() => SelectImage(0));
        imageButton2.onClick.AddListener(() => SelectImage(1));
        imageButton3.onClick.AddListener(() => SelectImage(2));

        // Show selection panel, hide game panel
        imageSelectionPanel.SetActive(true);
        puzzleGamePanel.SetActive(false);

        Debug.Log(" ImageSelector initialized successfully!");
    }

    void SelectImage(int imageIndex)
    {
        Debug.Log($" Image {imageIndex} selected");

        // Validate index
        if (imageIndex < 0 || imageIndex >= puzzleImages.Length)
        {
            Debug.LogError($"Invalid image index: {imageIndex}");
            return;
        }

        // Check if image data exists
        if (puzzleImages[imageIndex] == null)
        {
            Debug.LogError($" PuzzleImageData at index {imageIndex} is null!");
            return;
        }

        if (puzzleImages[imageIndex].puzzlePieces == null || puzzleImages[imageIndex].puzzlePieces.Length == 0)
        {
            Debug.LogError($"No puzzle pieces assigned for image {imageIndex}!");
            return;
        }

        Debug.Log($"Starting puzzle with {puzzleImages[imageIndex].puzzlePieces.Length} pieces");

        // Hide selection panel
        imageSelectionPanel.SetActive(false);

        // Show game panel
        puzzleGamePanel.SetActive(true);

        Debug.Log($" PuzzleGamePanel is now: {puzzleGamePanel.activeSelf}");

        // Start the puzzle
        if (puzzleManager != null)
        {
            puzzleManager.StartPuzzle(puzzleImages[imageIndex]);
        }
        else
        {
            Debug.LogError("PuzzleManager is null!");
        }
    }

    public void BackToSelection()
    {
        Debug.Log("Back to selection");
        imageSelectionPanel.SetActive(true);
        puzzleGamePanel.SetActive(false);
    }
}

[System.Serializable]
public class PuzzleImageData
{
    public string imageName;
    public Sprite completeImage; // Complete image for reference
    public Sprite[] puzzlePieces; // 16 puzzle pieces
}