using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Script này quản lý việc hiển thị nhiều danh sách (UI View).
/// Nâng cấp: Giờ đây mỗi button có thể BẬT/TẮT (toggle) list của nó.
/// Nếu một list khác đang hiện, nó sẽ bị tắt đi để nhường chỗ.
/// </summary>
public class ListControllerTA : MonoBehaviour
{
    // Danh sách chứa tất cả các UI View của bạn
    public List<GameObject> allMyLists;

    void Start()
    {
        // Khi game bắt đầu, ẩn tất cả các list
        HideAll();
    }

    // Hàm nội bộ để ẩn tất cả các list
    private void HideAll()
    {
        foreach (var listObject in allMyLists)
        {
            if (listObject != null)
            {
                listObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Hàm được gọi bởi các Button.
    /// Logic mới:
    /// 1. Nếu list được yêu cầu đang ẩn -> Ẩn tất cả các list khác rồi hiện nó lên.
    /// 2. Nếu list được yêu cầu đang hiện -> Chỉ cần ẩn nó đi.
    /// </summary>
    /// <param name="listIndex">Số thứ tự của list cần bật/tắt (bắt đầu từ 0)</param>
    public void ShowList(int listIndex)
    {
        // Kiểm tra xem số index có hợp lệ không
        if (listIndex < 0 || listIndex >= allMyLists.Count || allMyLists[listIndex] == null)
        {
            // Nếu không hợp lệ thì không làm gì cả
            return;
        }

        // Lấy ra đối tượng list mà button muốn điều khiển
        GameObject targetList = allMyLists[listIndex];

        // Lưu lại trạng thái BẬT/TẮT hiện tại của list đó TRƯỚC KHI làm gì khác
        bool isTargetAlreadyActive = targetList.activeSelf;

        // --- Bắt đầu logic chính ---

        // Bước 1: Luôn luôn ẩn tất cả các list trước tiên. 
        // Điều này đảm bảo giao diện luôn sạch sẽ.
        HideAll();

        // Bước 2: Dựa vào trạng thái đã lưu, quyết định xem có bật lại list hay không.
        if (!isTargetAlreadyActive)
        {
            // Nếu trước đó nó ĐANG TẮT, giờ chúng ta sẽ BẬT nó lên.
            targetList.SetActive(true);
        }
        // Nếu trước đó nó ĐANG BẬT (isTargetAlreadyActive là true), 
        // thì sau khi HideAll() ở Bước 1, nó đã bị tắt. 
        // Chúng ta không cần làm gì nữa, và kết quả là list đó đã được tắt đúng như ý muốn.
    }
}