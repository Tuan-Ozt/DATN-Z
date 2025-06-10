//sử dụng hero phải khai báo
using Assets.HeroEditor.Common.CharacterScripts;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
    //sử dụng thông qua character ( sử dụng đồ )
    public Character character; // Gán trong Inspector

    private InventoryItem1 currentItem;

    void Awake()
    {
        Debug.Log("Da chay awake ItemDetailsUI");
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(InventoryItem1 item)
    {
        currentItem = item;
        Debug.Log($"🟢 Show panel: {item.itemId} / {item.quantity}");

        icon.sprite = item.stats?.Icon;
        nameText.text = item.stats?.Name ?? "Không rõ";
        descText.text = $"ID: {item.itemId}\nSố lượng: {item.quantity}";

        if (item.stats != null)
        {
            descText.text = $"<b>{item.stats.Description}</b>\n" +
                            //$"<b>Số lượng:</b> {item.quantity}\n\n" +
                            $"<b>Chỉ số:</b>\n" +
                            $"• Sức mạnh: {item.stats.Strength}\n" +
                            $"• Phòng thủ: {item.stats.Defense}\n" +
                            $"• Nhanh nhẹn: {item.stats.Agility}\n" +
                            $"• Trí tuệ: {item.stats.Intelligence}\n" +
                            $"• Sinh lực: {item.stats.Vitality}";
        }
        else
        {
            descText.text = $"ID: {item.itemId}\nSố lượng: {item.quantity}\n(stats null)";
        }

        panel.SetActive(true);
    }



    public void UseItem()
    {
        // Kiểm tra hợp lệ
        if (currentItem == null || currentItem.stats == null)
        {
            Debug.LogError("❌ currentItem null hoặc thiếu stats.");
            return;
        }

        if (string.IsNullOrEmpty(PlayerDataHolder1.CharacterJson))
        {
            Debug.LogError("❌ Chưa có CharacterJson.");
            return;
        }

        // Lấy dữ liệu trang bị hiện tại của nhân vật (dạng JSON)
        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(PlayerDataHolder1.CharacterJson);

        // Xác định loại slot để trang bị
        switch (currentItem.stats.Type)
        {
            case "Helmet":
            case "Armor":
            case "Gloves":
            case "Boots":
            case "Shield":
            case "PrimaryMeleeWeapon":
            case "SecondaryMeleeWeapon":
            case "Belt":
            case "Cape":
            case "Back":
            case "Vest":
            case "Pauldrons":
                // Ghi đè itemId mới vào đúng slot
                dict[currentItem.stats.Type] = currentItem.itemId;
                break;
            default:
                Debug.LogWarning($" Loại chưa hỗ trợ: {currentItem.stats.Type}");
                return;
        }

        // Lưu lại dữ liệu mới vào JSON
        string updatedJson = JsonConvert.SerializeObject(dict, Formatting.None);
        PlayerDataHolder1.CharacterJson = updatedJson;

        // Cập nhật model nhân vật (ảnh đại diện) cho cả player thật và UI preview
        if (PlayerAvatar.Instance != null)
        {
            PlayerAvatar.Instance.Character.FromJson(updatedJson);
            PlayerAvatar.Instance.Character.Initialize();

        }

        if (CharacterUIManager1.Instance != null)
        {
            CharacterUIManager1.Instance.character.FromJson(updatedJson);
            CharacterUIManager1.Instance.character.Initialize();

        }

        Debug.Log("Đã mặc item và cập nhật cả Player + Character UI.");
        panel.SetActive(false); // Đóng panel chi tiết
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
    private void EquipToCharacter(ItemStats stats)
    {
        switch (stats.Type)
        {
            case "Helmet":
                character.Helmet = stats.Icon;
                break;
            case "Armor":
                character.Armor = new List<Sprite> { stats.Icon }; // nếu là bộ nhiều phần → load danh sách
                break;
            case "Weapon":
                character.PrimaryMeleeWeapon = stats.Icon;
                character.WeaponType = HeroEditor.Common.Enums.WeaponType.Melee1H; // ví dụ
                break;
            case "Shield":
                character.Shield = stats.Icon;
                break;
            case "Bow":
                character.Bow = new List<Sprite> { stats.Icon };
                character.WeaponType = HeroEditor.Common.Enums.WeaponType.Bow;
                break;
            case "Belt":
                character.Back = stats.Icon; // 
                break;
            case "Gloves":
                character.Back = stats.Icon; //
                break;

            // thêm các loại khác nếu có
            default:
                Debug.LogWarning($" Không hỗ trợ loại trang bị: {stats.Type}");
                break;
        }

        character.Initialize(); // ← áp dụng thay đổi
    }

}
