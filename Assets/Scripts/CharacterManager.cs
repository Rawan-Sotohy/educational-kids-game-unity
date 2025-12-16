using UnityEngine;
using Firebase.Database;

/// <summary>
/// SIMPLE character system - stores name and character choice
/// Add to Loading scene
/// </summary>
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    [Header("Character Images")]
    public Sprite[] characterSprites; // Drag your 2 character images here

    [Header("Current Selection")]
    public string playerName = "Player";
    public int selectedCharacter = 0; // 0 or 1 (default is 0)

    [Header("Firebase Settings")]
    public string databaseURL = "https://educational-kids-game-un-4ef4d-default-rtdb.firebaseio.com";

    private DatabaseReference databaseRef;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize database with URL
        databaseRef = FirebaseDatabase.GetInstance(databaseURL).RootReference;

        // Try to load saved data
        LoadFromFirebase();
    }

    /// <summary>
    /// Load name and character from Firebase
    /// </summary>
    public void LoadFromFirebase()
    {
        if (FirebaseManager.Instance == null || !FirebaseManager.Instance.IsUserLoggedIn())
        {
            Debug.Log("⚠️ Not logged in, using defaults");
            return;
        }

        string userId = FirebaseManager.Instance.GetCurrentUser().UserId;

        databaseRef
            .Child("users").Child(userId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted && task.Result.Exists)
                {
                    var snapshot = task.Result;

                    // Load from Firebase or use defaults
                    playerName = snapshot.Child("name").Value?.ToString() ?? "Player";

                    string charStr = snapshot.Child("character").Value?.ToString() ?? "0";
                    selectedCharacter = int.Parse(charStr);

                    // Ensure valid range (0 or 1)
                    if (selectedCharacter < 0 || selectedCharacter >= characterSprites.Length)
                        selectedCharacter = 0;

                    Debug.Log($"✅ Loaded: {playerName}, Character {selectedCharacter}");
                }
                else
                {
                    Debug.Log("📝 No saved data, using defaults: Player, Character 0");
                }
            });
    }

    /// <summary>
    /// Save name and character to Firebase
    /// </summary>
    public void SaveToFirebase()
    {
        if (FirebaseManager.Instance == null || !FirebaseManager.Instance.IsUserLoggedIn())
        {
            Debug.LogWarning("⚠️ Can't save - not logged in");
            return;
        }

        string userId = FirebaseManager.Instance.GetCurrentUser().UserId;

        var data = new System.Collections.Generic.Dictionary<string, object>
        {
            { "name", playerName },
            { "character", selectedCharacter }
        };

        databaseRef
            .Child("users").Child(userId)
            .SetValueAsync(data).ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    Debug.Log($"✅ Saved: {playerName}, Character {selectedCharacter}");
                }
                else
                {
                    Debug.LogError("❌ Save failed!");
                }
            });
    }

    /// <summary>
    /// Get current character sprite
    /// </summary>
    public Sprite GetCurrentCharacterSprite()
    {
        if (selectedCharacter >= 0 && selectedCharacter < characterSprites.Length)
            return characterSprites[selectedCharacter];

        Debug.LogWarning("⚠️ Invalid character index, returning first sprite");
        return characterSprites.Length > 0 ? characterSprites[0] : null;
    }
}