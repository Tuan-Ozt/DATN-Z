using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    public GameObject characterPanel;
    public GameObject TiemNang;
    public GameObject Kynang;
    public GameObject button;

    public void TogglePanel()
    {
        bool isActive = characterPanel.activeSelf;

        if (isActive)
        {
            // Nếu đang mở, thì ẩn hết
            characterPanel.SetActive(false);
            TiemNang.SetActive(false);
            Kynang.SetActive(false);
            button.SetActive(false);
        }
        else
        {
            // Nếu đang tắt, thì bật tất cả
            characterPanel.SetActive(true);
            TiemNang.SetActive(true);
            Kynang.SetActive(true);
            button.SetActive(true);
        }
    }

    public void ToggleThongtin()
    {
        characterPanel.SetActive(true);
        TiemNang.SetActive(false);
        Kynang.SetActive(false);
    }
    public void ToggleTiemNang()
    {
        TiemNang.SetActive(true);
        Kynang.SetActive(false);
        characterPanel.SetActive(false);

    }
    public void ToggleKyNang()
    {
        Kynang.SetActive(true);
        TiemNang.SetActive(false);
        characterPanel.SetActive(false);
    }
}
