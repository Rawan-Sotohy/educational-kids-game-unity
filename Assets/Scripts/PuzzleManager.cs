using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform puzzleGrid; // Grid with 16 EMPTY Slots
    [SerializeField] private Transform piecesContainer; // Container with your 16 existing pieces
    [SerializeField] private Image referenceImage; // Small complete image at top

    [Header("Game Settings")]
    [SerializeField] private int gridSize = 4;

    private List<PuzzlePiece> pieces = new List<PuzzlePiece>();
    private List<PuzzleSlot> slots = new List<PuzzleSlot>();
    private PuzzlePiece selectedPiece = null;
    private PuzzleImageData currentImageData;
    private Animator characterAnimator;
    private int correctPiecesCount = 0;

    void Start()
    {
        characterAnimator = FindObjectOfType<Animator>();

        // Get all slots from the grid
        foreach (Transform child in puzzleGrid)
        {
            PuzzleSlot slot = child.GetComponent<PuzzleSlot>();
            if (slot != null)
            {
                slots.Add(slot);
            }
        }

        // Assign slot indices
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].slotIndex = i;
        }
    }

    public void StartPuzzle(PuzzleImageData imageData)
    {
        currentImageData = imageData;
        correctPiecesCount = 0;

        // Show reference image
        if (referenceImage != null)
            referenceImage.sprite = imageData.completeImage;

        // Setup existing pieces with new images
        SetupExistingPieces();

        // Shuffle pieces
        ShufflePieces();
    }

    void SetupExistingPieces()
    {
        pieces.Clear();

        // Get all pieces from the container
        PuzzlePiece[] existingPieces = piecesContainer.GetComponentsInChildren<PuzzlePiece>(true);

        for (int i = 0; i < existingPieces.Length && i < 16; i++)
        {
            PuzzlePiece piece = existingPieces[i];

            // Assign the sprite from the selected image data
            if (i < currentImageData.puzzlePieces.Length)
            {
                Image pieceImage = piece.GetComponent<Image>();
                if (pieceImage != null)
                {
                    pieceImage.sprite = currentImageData.puzzlePieces[i];
                }

                // Set correct index
                piece.correctIndex = i;
                piece.currentPosition = i;
                piece.manager = this;

                // Reset color
                pieceImage.color = Color.white;

                pieces.Add(piece);
            }
        }
    }

    void ShufflePieces()
    {
        // Create random positions
        List<int> positions = new List<int>();
        for (int i = 0; i < pieces.Count; i++)
            positions.Add(i);

        // Fisher-Yates shuffle
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // Apply shuffled positions
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].currentPosition = positions[i];
            pieces[i].transform.SetSiblingIndex(positions[i]);
        }
    }

    public void OnPieceClicked(PuzzlePiece piece)
    {
        // First click - select piece
        if (selectedPiece == null)
        {
            selectedPiece = piece;
            piece.pieceImage.color = Color.yellow; // Highlight
        }
        // Second click - swap pieces
        else
        {
            SwapPieces(selectedPiece, piece);

            // Deselect
            selectedPiece.pieceImage.color = Color.white;
            selectedPiece = null;

            // Check if puzzle is complete
            CheckWin();
        }
    }

    void SwapPieces(PuzzlePiece piece1, PuzzlePiece piece2)
    {
        // Swap sibling indices (visual position)
        int siblingIndex1 = piece1.transform.GetSiblingIndex();
        int siblingIndex2 = piece2.transform.GetSiblingIndex();

        piece1.transform.SetSiblingIndex(siblingIndex2);
        piece2.transform.SetSiblingIndex(siblingIndex1);

        // Swap logical positions
        int tempPos = piece1.currentPosition;
        piece1.currentPosition = piece2.currentPosition;
        piece2.currentPosition = tempPos;

        // Play sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
    }

    void CheckWin()
    {
        bool allCorrect = true;

        foreach (PuzzlePiece piece in pieces)
        {
            if (!piece.IsInCorrectPosition())
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            StartCoroutine(OnPuzzleComplete());
        }
    }

    IEnumerator OnPuzzleComplete()
    {
        Debug.Log("Puzzle Complete!");

        // Animation & Sound
        if (characterAnimator != null)
            characterAnimator.SetTrigger("Happy");

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWinFanfare();

        yield return new WaitForSeconds(0.5f);

        // Show stars
        if (StarPopupManager.Instance != null)
        {
            StarPopupManager.Instance.ShowStars(5, "Puzzle Complete!");
        }
    }

    public void RestartPuzzle()
    {
        selectedPiece = null;
        correctPiecesCount = 0;
        ShufflePieces();

        if (StarPopupManager.Instance != null)
            StarPopupManager.Instance.PlayAgain();
    }

    public void BackToSelection()
    {
        FindObjectOfType<ImageSelector>()?.BackToSelection();
    }
}