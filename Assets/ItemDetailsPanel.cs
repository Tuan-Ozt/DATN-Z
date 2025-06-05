using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailsPanel : MonoBehaviour
{
    public static ItemDetailsPanel Instance;

    [Header("References")]
    public GameObject panelObject;
    public Image iconImage;
    public TMP_Text descriptionText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        panelObject.SetActive(false); // ẩn ban đầu
    }

    public void Show(string id, Sprite iconSprite)
    {
        panelObject.SetActive(true);
        iconImage.sprite = iconSprite;
        descriptionText.text = GetStatsFromId(id);
    }

    public void Hide()
    {
        panelObject.SetActive(false);
    }

    private string GetStatsFromId(string id)
    {
        switch (id)
        {
            case "helmet_dragon": return "Giáp: +50\nKháng băng: +10";
            case "bow_ice": return "Sát thương: 45-60\nTốc độ đánh: +5%";
            default: return "Không có thông tin.";
        }
    }
}

