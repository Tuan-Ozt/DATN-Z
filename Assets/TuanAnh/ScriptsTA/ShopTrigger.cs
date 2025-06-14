using UnityEngine;

/// <summary>
/// Script này quản lý việc mở Shop.
/// Nâng cấp: Yêu cầu người chơi phải ở trong một khoảng cách nhất định
/// thì mới có thể click vào để mở Shop.
/// </summary>
public class ShopTriggerTA : MonoBehaviour
{
    [Header("Thiết lập Shop")]
    [Tooltip("Gán bảng UI Shop từ Canvas vào đây")]
    public GameObject shopPanel;

    [Tooltip("Khoảng cách tối đa người chơi có thể tương tác")]
    public float interactionDistance = 5f;

    [Header("Tham chiếu (Tự động tìm)")]
    [Tooltip("Transform của người chơi, sẽ được tự động tìm bằng Tag 'Player'")]
    private Transform playerTransform;

    void Start()
    {
        // Tự động tìm GameObject có tag "Player" khi game bắt đầu
        // và lấy component Transform của nó.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            // In ra cảnh báo nếu không tìm thấy Player
            Debug.LogError("Không tìm thấy đối tượng nào có Tag 'Player' trong Scene!");
        }
    }

    void Update()
    {
        // --- BƯỚC 1: KIỂM TRA KHOẢNG CÁCH TRƯỚC TIÊN ---
        // Nếu không tìm thấy player thì không làm gì cả
        if (playerTransform == null) return;

        // Tính khoảng cách giữa người chơi và đối tượng shop này
        float distance = Vector2.Distance(playerTransform.position, transform.position);

        // Nếu người chơi ở quá xa, không xử lý gì thêm.
        if (distance > interactionDistance)
        {
            // (Tùy chọn) Bạn có thể thêm logic để hiện một thông báo
            // ví dụ: "Hãy lại gần hơn!" ở đây.
            return;
        }

        // --- BƯỚC 2: XỬ LÝ CLICK CHUỘT (LOGIC CŨ CỦA BẠN) ---
        // Chỉ khi người chơi đã ở đủ gần, chúng ta mới kiểm tra click chuột.
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                // Chỉ mở shop nếu click trúng đối tượng này
                if (hit.collider.gameObject == gameObject)
                {
                    if (shopPanel != null)
                    {
                        bool nextState = !shopPanel.activeSelf;
                        shopPanel.SetActive(nextState);
                    }
                    else
                    {
                        Debug.LogWarning("ShopPanel chưa được gán trong Inspector!");
                    }
                }
            }
        }
    }
}