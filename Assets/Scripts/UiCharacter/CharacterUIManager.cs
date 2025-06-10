using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    public GameObject characterPanel;
    public GameObject TiemNang;
    public GameObject Kynang;
    public GameObject button;
    public GameObject Tui;
    public GameObject button2;
    private void Start()
    {
        characterPanel.SetActive(false);
        TiemNang.SetActive(false);
        Kynang.SetActive(false);
        button.SetActive(false);
        Tui.SetActive(false);
        button2.SetActive(false);

    }
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
            Tui.SetActive(false);
            button2.SetActive(false);
            
        }
        else
        {
            // Nếu đang tắt, thì bật tất cả
            characterPanel.SetActive(true);
            TiemNang.SetActive(true);
            Kynang.SetActive(true);
            button.SetActive(true);
            Tui.SetActive(true);
            button2.SetActive(true);
        }
    }

    public void ToggleThongtin()
    {
        characterPanel.SetActive(true);
        TiemNang.SetActive(false);
        Kynang.SetActive(false);
        Tui.SetActive(false);
    }
    public void ToggleTiemNang()
    {
        TiemNang.SetActive(true);
        Kynang.SetActive(false);
        characterPanel.SetActive(false);
        Tui.SetActive(false);

    }
    public void ToggleKyNang()
    {
        Kynang.SetActive(true);
        TiemNang.SetActive(false);
        characterPanel.SetActive(false);
    }
    public void TuiButton()
    {
        Tui.SetActive(true);
        Kynang.SetActive(false);
        TiemNang.SetActive(false);
        characterPanel.SetActive(false);
        button.SetActive(false);
    }
    public void TTButton()
    {
        characterPanel.SetActive(true);

        Kynang.SetActive(false);
        TiemNang.SetActive(false);
        Tui.SetActive(false);
        button.SetActive(true);

    }
}
