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
        var stats = ItemStatDatabase.Instance.GetStats(id);

        description.text = $"{displayType}\n\n{GetStatsFromId(id)}";
        Type.text = $"Loại: {displayType}";
        Name.text = $"{stats.Type}: {name}";
    }


    public void Hide()
    {
        panel.SetActive(false);
    }

    private string GetStatsFromId(string id)
    {
        var stats = ItemStatDatabase.Instance.GetStats(id);
        if (stats == null)
            return "Không có thông tin.";

        return
            $"Sức mạnh: {stats.Strength}\n" +
            $"Phòng thủ: {stats.Defense}\n" +
            $"Nhanh nhẹn: {stats.Agility}\n" +
            $"Trí tuệ: {stats.Intelligence}\n" +
            $"Thể lực: {stats.Vitality}";
    }

}
