using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemId; // trùng với ItemStats
    public int quantity = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // gán tag Player cho player
        {
            Debug.Log($" Nhặt item: {itemId}");

            InventoryManager.Instance.AddItem(itemId, quantity);

            Destroy(gameObject); // xoá vật phẩm trên map sau khi nhặt
        }
        else
        {
            Debug.LogError(" Không tìm thấy InventoryManager trong scene.");
        }
    }
}
