using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public InventoryItem itemData;
    public Image iconImage;
    public TextMeshProUGUI quantityText;

    public void Init(InventoryItem data)
    {
        itemData = data;

        if (iconImage != null)
            iconImage.sprite = data.icon;

        if (quantityText != null)
            quantityText.text = data.quantity > 1 ? data.quantity.ToString() : "";

        // Debug
        Debug.Log($"Slot init: {data.itemName} (x{data.quantity})");
    }


    public void OnClick()
    {
        if (itemData != null)
            ItemDetailsUI.Instance.Show(itemData);
        else
            Debug.LogWarning("ItemData null khi click vào slot.");
    }
}
