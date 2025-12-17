using UnityEngine;

public class SceneCharacterSwitcher : MonoBehaviour
{
    public GameObject character0;
    public GameObject character1;

    void Start()
    {
        int index = CharacterManager.Instance.selectedCharacter;
        character0.SetActive(index == 0);
        character1.SetActive(index == 1);
    }
}
