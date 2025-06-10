using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsUI : MonoBehaviour
{
    public static ItemDetailsUI Instance;

    public GameObject panel;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public Button useButton;
    public Button dropButton;
    public Button closeButton;

    private InventoryItem currentItem;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(InventoryItem item)
    {
        currentItem = item;

        if (icon != null)
            icon.sprite = item.icon;

        if (nameText != null)
            nameText.text = item.itemName;

        if (descText != null)
            descText.text = $"ID: {item.itemId}\nSố lượng: {item.quantity}";

        panel.SetActive(true);
    }


    public void UseItem()
    {
        Debug.Log($"Đã dùng {currentItem.itemName}");
        panel.SetActive(false);
     
        // Gọi xử lý dùng item tại đây
    }

    public void DropItem()
    {
        Debug.Log($"Đã vứt {currentItem.itemName}");
        panel.SetActive(false);
        // Gọi xử lý vứt item tại đây
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
