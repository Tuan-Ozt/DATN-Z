using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    public GameObject characterPanel;

    public void TogglePanel()
    {
        characterPanel.SetActive(!characterPanel.activeSelf);
    }
}
