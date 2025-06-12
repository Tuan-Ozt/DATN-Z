using Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryUIManager uiManager; // gán từ scene
    public List<InventoryItem1> playerInventory = new List<InventoryItem1>();

    void Awake() => Instance = this;

    public void AddItem(string itemId, int quantity)
    {
        // Tìm nếu đã có trong inventory
        var item = playerInventory.Find(i => i.itemId == itemId);
        if (item != null)
        {
            item.quantity += quantity;
        }
        else
        {
            var stats = Resources.LoadAll<ItemStats>("ItemStats");
            foreach (var s in stats)
            {
                Debug.Log($" FOUND IN RESOURCE: {s.itemId}");
            }

            var data = System.Array.Find(stats, s => s.itemId == itemId);
            if (data == null)
            {
                Debug.LogError($" Không tìm thấy ItemStats với ID: {itemId}");
                return;
            }


            item = new InventoryItem1
            {
                itemId = itemId,
                quantity = quantity,
                stats = data
            };
            playerInventory.Add(item);
        }

        uiManager.DisplayInventory(playerInventory); // cập nhật UI
    }
}
