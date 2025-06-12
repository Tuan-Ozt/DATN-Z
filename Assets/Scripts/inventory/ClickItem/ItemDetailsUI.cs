//sử dụng hero phải khai báo
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common.Enums;
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
        EquipToCharacter(currentItem.stats);
        if (CharacterUIManager1.Instance != null && currentItem != null)
        {
            string type = currentItem.stats.Type;
            string itemId = currentItem.itemId;

            // Nếu là Gloves thì hiển thị lại đúng slot từ ArmorSlots
            if (type == "Gloves")
            {
                CharacterUIManager1.Instance.DisplayItem(
                    CharacterUIManager1.Instance.ArmorSlots[2], // Index 2 là Gloves
                    itemId,
                    "Gloves"
                );
            }
            if (type == "Belt")
            {
                CharacterUIManager1.Instance.DisplayItem(
                    CharacterUIManager1.Instance.ArmorSlots[5], // Index 2 là Gloves
                    itemId,
                    "Belt"
                );
            }
            if (type == "Boots")
            {
                CharacterUIManager1.Instance.DisplayItem(
                    CharacterUIManager1.Instance.ArmorSlots[1], // Index 2 là Gloves
                    itemId,
                    "Boots"
                );
            }
            if (type == "Armor")
            {
                CharacterUIManager1.Instance.DisplayItem(
                    CharacterUIManager1.Instance.ArmorSlots[0], // Index 2 là Gloves
                    itemId,
                    "Armor"
                );
            }
            if (type == "Helmet")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.Helmetslot, // Index 2 là Gloves
                    itemId,
                    "Helmet"
                );
            }

            if (type == "MeleeWeapon1H")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.MeleeWeapon1Hslot, // Index 2 là Gloves
                    itemId,
                    "MeleeWeapon1H"
                );
            }
            if (type == "Cape")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.Capeslot, // Index 2 là Gloves
                    itemId,
                    "Cape"
                );
            }
            if (type == "Pauldrons")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.ArmorSlots[3], // Index 2 là Gloves
                    itemId,
                    "Pauldrons"
                );
            }
            if (type == "Glasses")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.Glassesslot, // Index 2 là Gloves
                    itemId,
                    "Glasses"
                );
            }
            if (type == "Hair")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.Hairslot, // Index 2 là Gloves
                    itemId,
                    "Hair"
                );
            }
            if (type == "Back")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.Backslot, // Index 2 là Gloves
                    itemId,
                    "Back"
                );
            }


            // Có thể làm tương tự với Boots, Vest, Belt, Armor, Pauldrons...
        }

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

        // Parse JSON hiện tại từ nhân vật
        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(PlayerDataHolder1.CharacterJson);

        // Ghi đè item vào đúng slot
        switch (currentItem.stats.Type)
        {
            case "Helmet":
            case "Armor":
            case "Boots":
            case "Gloves":
            case "Pauldrons":
            case "Vest":
            case "Belt":
            case "Shield":
            case "PrimaryMeleeWeapon":
            case "SecondaryMeleeWeapon":
            case "MeleeWeapon1H":
            case "MeleeWeapon2H":
            case "Bow":
            case "Cape":
            case "Back":
            case "Glasses":
            case "Hair":
                //case "Bow":
                dict[currentItem.stats.Type] = currentItem.itemId;
                break;
            default:
                Debug.LogWarning($"❌ Loại chưa hỗ trợ: {currentItem.stats.Type}");
                return;
        }

        // Serialize lại JSON
        string updatedJson = JsonConvert.SerializeObject(dict, Formatting.None);
        PlayerDataHolder1.CharacterJson = updatedJson;

        // Gửi lên server
        if (AuthManager.Instance != null)
        {
            Debug.Log(" Gửi CharacterJson mới lên server...");
            AuthManager.Instance.StartCoroutine(AuthManager.Instance.SaveCharacterToServer(updatedJson));
        }

        // Cập nhật PlayerAvatar
        if (PlayerAvatar.Instance != null)
        {
            PlayerAvatar.Instance.UpdateCharacterJson(updatedJson);
            Debug.Log("PlayerAvatar đã cập nhật JSON.");
        }

        // Cập nhật Character UI
        if (CharacterUIManager1.Instance != null)
        {
            CharacterUIManager1.Instance.character.FromJson(updatedJson);
        }

        Debug.Log("Đã dùng item, cập nhật và lưu dữ liệu thành công.");
        panel.SetActive(false);
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
        Debug.Log(" ĐANG EQUIP: stats.Type = " + stats.Type + ", stats.Icon = " + (stats.Icon != null ? stats.Icon.name : "NULL"));
        string spriteName = ExtractSpriteName(stats.itemId); // Lấy từ itemId
        Sprite sprite = null;
        switch (stats.Type)
        {
            case "Helmet":
                character.Helmet = stats.Icon;
                break;
            case "Glasses":
                character.Glasses = stats.Icon;
                break;
            case "Cape":
                character.Cape = stats.Icon;
                break;
            case "Back":
                character.Back = stats.Icon;
                break;
            case "Hair":
                character.Hair = stats.Icon;
                break;
            case "Weapon":
                character.PrimaryMeleeWeapon = stats.Icon;
                character.WeaponType = HeroEditor.Common.Enums.WeaponType.Firearms1H; // ví dụ
                break;
            case "Shield":
                character.Shield = stats.Icon;
                break;
            case "Armor":
                EnsureArmorListSize(0);
                character.Armor[0] = stats.Icon;
                break;
            case "Boots":
                EnsureArmorListSize(1);
                character.Armor[1] = stats.Icon;
                break;
            case "Gloves":
                EnsureArmorListSize(2);
                character.Armor[2] = stats.Icon;
                break;
            case "Pauldrons":
                EnsureArmorListSize(3);
                character.Armor[3] = stats.Icon;
                break;
            case "Vest":
                EnsureArmorListSize(4);
                character.Armor[4] = stats.Icon;
                break;
            case "Belt":
                EnsureArmorListSize(5);
                character.Armor[5] = stats.Icon;
                break;
            // === Vũ khí ===
            case "MeleeWeapon1H":
            case "PrimaryMeleeWeapon":
                sprite = FindSpriteInCollection(spriteName, character.SpriteCollection.MeleeWeapon1H);
                if (sprite == null) sprite = stats.Icon; // fallback nếu không tìm được
                character.PrimaryMeleeWeapon = sprite;
                character.WeaponType = WeaponType.Melee1H;
                break;


            // thêm các loại khác nếu có
            default:
                Debug.LogWarning($" Không hỗ trợ loại trang bị: {stats.Type}");
                break;
        }

        character.Initialize(); // ← áp dụng thay đổi
    }

    private void EnsureArmorListSize(int index)
    {
        while (character.Armor.Count <= index)
        {
            character.Armor.Add(null);
        }
    }
    private string ExtractSpriteName(string itemId)
    {
        if (string.IsNullOrEmpty(itemId)) return "";
        int lastDot = itemId.LastIndexOf('.');
        return lastDot >= 0 ? itemId.Substring(lastDot + 1) : itemId;
    }

    private Sprite FindSpriteInCollection(string spriteName, List<HeroEditor.Common.SpriteGroupEntry> groupEntries)
    {
        foreach (var entry in groupEntries)
        {
            if (entry.Sprites == null) continue;
            foreach (var sprite in entry.Sprites)
            {
                if (sprite != null && sprite.name == spriteName)
                    return sprite;
            }
        }

        Debug.LogError($"❌ Không tìm thấy sprite có tên: {spriteName}");
        return null;
    }
}
