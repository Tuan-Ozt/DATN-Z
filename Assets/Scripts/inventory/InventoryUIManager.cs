using UnityEngine;
using UnityEngine.UI;
using TMPro; // ✅ Thêm dòng này nếu bạn dùng TextMeshPro
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    public Transform gridParent;
    public GameObject slotPrefab;
    public List<InventoryItem> testItems;

    void Start()
    {
        DisplayInventory(testItems);
    }

    public void DisplayInventory(List<InventoryItem> items)
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            GameObject slot = Instantiate(slotPrefab, gridParent); // hoặc content transform
            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.Init(item); // Đây là phần QUAN TRỌNG  đang thiếu
            }

            var icon = slot.transform.Find("Icon")?.GetComponent<Image>();
            if (icon != null)
                icon.sprite = item.icon;

            // 🔧 Fix dùng đúng TextMeshProUGUI thay vì Text
            var qtyText = slot.transform.Find("QuantityText")?.GetComponent<TextMeshProUGUI>();
            if (qtyText != null)
                qtyText.text = item.quantity > 1 ? item.quantity.ToString() : "";
        }
    }
}
