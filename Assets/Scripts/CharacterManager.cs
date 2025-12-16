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
    public Sprite[] characterSprites; // Drag your 3 character images here

    [Header("Current Selection")]
    public string playerName = "Player";
    public int selectedCharacter = 0; // 0, 1, or 2

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

    [Header("Firebase Settings")]
    public string databaseURL = "https://educational-kids-game-un-4ef4d-default-rtdb.firebaseio.com"; // PUT YOUR URL HERE!

    private DatabaseReference databaseRef;

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
            return;

        string userId = FirebaseManager.Instance.GetCurrentUser().UserId;
        databaseRef
            .Child("users").Child(userId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted && task.Result.Exists)
                {
                    var snapshot = task.Result;
                    playerName = snapshot.Child("name").Value?.ToString() ?? "Player";
                    selectedCharacter = int.Parse(snapshot.Child("character").Value?.ToString() ?? "0");
                    Debug.Log($"✅ Loaded: {playerName}, Character {selectedCharacter}");
                }
            });
    }

    /// <summary>
    /// Save name and character to Firebase
    /// </summary>
    public void SaveToFirebase()
    {
        if (FirebaseManager.Instance == null || !FirebaseManager.Instance.IsUserLoggedIn())
            return;

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
            });
    }

    /// <summary>
    /// Get current character sprite
    /// </summary>
    public Sprite GetCurrentCharacterSprite()
    {
        if (selectedCharacter >= 0 && selectedCharacter < characterSprites.Length)
            return characterSprites[selectedCharacter];
        return null;
    }
}