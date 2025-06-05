using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// hiển thị bảng panel thông tin trang bị nhân vật
public class ItemDetailsPanel : MonoBehaviour
{
    public static ItemDetailsPanel Instance;

    public GameObject panel;
    public Image icon;
    public TMP_Text description;
    public TMP_Text Type;
    public TMP_Text Name;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(string id, Sprite iconSprite, string type = null)
    {
        panel.SetActive(true);
        icon.sprite = iconSprite;

        string name = id.Split('.').Length > 0 ? id.Split('.').Last() : id;
        string displayType = type ?? "Không rõ loại";

        description.text = $"{displayType}\n\n{GetStatsFromId(id)}";
        Type.text = $"Loại: {displayType}";
        Name.text = $"Tên: {name}";
    }


    public void Hide()
    {
        panel.SetActive(false);
    }

    private string GetStatsFromId(string id)
    {
        // TODO: Replace with real data from ScriptableObject/JSON
        switch (id)
        {
            case "helmet_dragon":
                return "Giáp: +50\nKháng Băng: +10";
            case "bow_ice":
                return "Sát thương: 45-60\nTốc độ đánh: +5%";
            default:
                return "Không có thông tin.";
        }
    }
}
