using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [Header("Bảng mua đồ gán từ Canvas")]
    public GameObject shopPanel;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click chuột trái
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.name);

                // Chỉ mở shop nếu đối tượng bị click chính là object này
                if (hit.collider.gameObject == gameObject)
                {
                    if (shopPanel != null)
                    {
                        bool nextState = !shopPanel.activeSelf;
                        shopPanel.SetActive(nextState);
                        Debug.Log("Đã đổi trạng thái ShopPanel: " + nextState);
                    }
                    else
                    {
                        Debug.LogWarning("ShopPanel chưa được gán trong Inspector!");
                    }
                }
            }
            else
            {
                Debug.Log("Không raycast trúng gì");
            }
        }
    }
}
