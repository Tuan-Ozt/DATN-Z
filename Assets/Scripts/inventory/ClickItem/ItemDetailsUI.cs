//sử dụng hero phải khai báo
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
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
        Debug.Log($"UseItem() called. UIManager instance = {CharacterUIManager1.Instance}");

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
            if (type == "MeleeWeapon2H")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.MeleeWeapon2Hslot, // Index 2 là Gloves
                    itemId,
                    "MeleeWeapon2H"
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
            if (type == "Vest")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.ArmorSlots[4], // Index 2 là Gloves
                    itemId,
                    "Vest"
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
            if (type == "Bow")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.Bowslot, // Index 2 là Gloves
                    itemId,
                    "Bow"
                );
            }
     
            if (type == "Shield")
            {
                CharacterUIManager1.Instance.DisplayItem1(
                    CharacterUIManager1.Instance.Shieldslot, // Index 2 là Gloves
                    itemId,
                    "Shield"
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
                dict["Boots"] = "ArmorCollection1.Shin";
                break;
            case "Gloves":
            case "Pauldrons":
            case "Vest":
            case "Belt":
            case "Shield":
            case "Cape":
            case "Back":
            case "Glasses":
            case "Hair":
      
                //case "Bow":
                dict[currentItem.stats.Type] = currentItem.itemId;
                break;
            case "Bow":
                dict["Bow"] = currentItem.itemId;          
                dict["WeaponType"] = WeaponType.Bow.ToString();

                break;

            case "MeleeWeapon1H":
                dict["PrimaryMeleeWeapon"] = currentItem.itemId;
                dict["WeaponType"] = WeaponType.Melee1H.ToString();


                break;
            case "MeleeWeapon2H":
                dict["SecondaryMeleeWeapon"] = currentItem.itemId;
                break;
            default:
                Debug.LogWarning($"❌ Loại chưa hỗ trợ: {currentItem.stats.Type}");
                return;
        }
        // Serialize lại JSON
        string updatedJson = JsonConvert.SerializeObject(dict, Formatting.None);
        PlayerDataHolder1.CharacterJson = updatedJson;
        Debug.Log($"[JSON AFTER USE] {updatedJson}");

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
            Debug.Log("DA GOI NHA111 ");
          
        }

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
            case "Shield":
                character.Shield = stats.Icon;
                break;
            case "Armor":
                EnsureArmorListSize(0);
                character.Armor.Insert(0, stats.Icon);
                Debug.Log("Debug: " + character.Armor[0]);
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
                character.Armor.Insert(4, stats.Icon);

                break;
            case "Belt":
                EnsureArmorListSize(5);
                character.Armor[8] = stats.Icon;
                break;
            // === Vũ khí ===
            // ===== Secondary Melee (Paired / 2H) =====
            case "MeleeWeapon1H":
                {
                    // Xoá bow/fireram cũ nếu có
                    character.Bow = null;
                    character.Firearms = null;
                    character.SecondaryMeleeWeapon = null;


                    sprite = FindSpriteInCollection(spriteName, character.SpriteCollection.MeleeWeapon1H) ?? stats.Icon;

                    var entry = new SpriteGroupEntry("Custom", "Default", "MeleeWeapon1H", sprite.name, "", sprite, new List<Sprite> { sprite });

                    character.Equip(entry, EquipmentPart.MeleeWeapon1H);

                    break;
                }
            case "MeleeWeapon2H":
                {
                     sprite = FindSpriteInCollection(spriteName, character.SpriteCollection.MeleeWeapon2H) ?? stats.Icon;

                    var entry = new SpriteGroupEntry("Custom", "Default", "MeleeWeapon2H", sprite.name, "", sprite, new List<Sprite> { sprite });

                    character.Equip(entry, EquipmentPart.MeleeWeapon2H);
                    break;
                }
            case "Bow":
                {
                    Debug.Log("👉 Equip Bow: " + spriteName);

                    var entry = character.SpriteCollection.Bow.FirstOrDefault(e => e.Name == spriteName);

                    if (entry != null)
                    {
                        // ⚠️ Đây là bước BẮT BUỘC để gán vào list Bow trong Character (hiện Inspector)
                        character.Bow = new List<Sprite>(entry.Sprites);

                        // 👉 Debug rõ ràng để kiểm tra
                        Debug.Log($"✅ [EQUIP BOW] Gán Bow = {character.Bow.Count} sprites");
                        for (int i = 0; i < character.Bow.Count; i++)
                        {
                            Debug.Log($"🟢 character.Bow[{i}] = {(character.Bow[i] != null ? character.Bow[i].name : "null")}");
                        }

                        // Tiếp theo mới Equip
                        var bowEntry = new SpriteGroupEntry(
                            edition: "Custom",
                            collection: "Default",
                            type: "Bow",
                            name: entry.Name,
                            path: "",
                            sprite: entry.Sprites.FirstOrDefault(),
                            sprites: entry.Sprites.ToList()
                        );

                        character.Equip(bowEntry, EquipmentPart.Bow);
                    }
                    else
                    {
                        Debug.LogError("❌ Không tìm thấy entry Bow: " + spriteName);
                    }

                    break;
                }




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
    //thao trang bi 
    public void UnequipItem()
    {
        if (currentItem == null || currentItem.stats == null)
        {
            Debug.LogError("❌ Không có item để gỡ.");
            return;
        }

        string type = currentItem.stats.Type;

        // 1. Gỡ trang bị khỏi character
        switch (type)
        {
            case "Helmet":
                character.Helmet = null;
                break;
            case "Glasses":
                character.Glasses = null;
                break;
            case "Cape":
                character.Cape = null;
                break;
            case "Back":
                character.Back = null;
                break;
            case "Hair":
                character.Hair = null;
                break;
            case "Shield":
                character.Shield = null;
                break;
            case "Armor":
            case "Boots":
            case "Gloves":
            case "Pauldrons":
            case "Vest":
            case "Belt":
                character.Equip(null, EquipmentPart.Armor); // Gỡ giáp riêng
                break;
            case "Bow":
                character.Bow = null;
                break;
            case "MeleeWeapon1H":
                character.PrimaryMeleeWeapon = null;
                break;
            case "MeleeWeapon2H":
                character.SecondaryMeleeWeapon = null;
                break;
            default:
                Debug.LogWarning($"⚠️ Loại chưa hỗ trợ khi gỡ: {type}");
                return;
        }

        character.Initialize();

        // 2. Gỡ khỏi JSON
        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(PlayerDataHolder1.CharacterJson);

        if (dict.ContainsKey(type)) dict.Remove(type);

        // Nếu là vũ khí → xóa thêm WeaponType
        if (type == "Bow" || type.StartsWith("Melee"))
        {
            dict.Remove("WeaponType");
        }

        if (type == "MeleeWeapon1H") dict.Remove("PrimaryMeleeWeapon");
        if (type == "MeleeWeapon2H") dict.Remove("SecondaryMeleeWeapon");

        string updatedJson = JsonConvert.SerializeObject(dict, Formatting.None);
        PlayerDataHolder1.CharacterJson = updatedJson;

        // 3. Gửi lên server
        if (AuthManager.Instance != null)
        {
            AuthManager.Instance.StartCoroutine(AuthManager.Instance.SaveCharacterToServer(updatedJson));
        }

        // 4. Cập nhật lại nhân vật & UI
        if (PlayerAvatar.Instance != null) PlayerAvatar.Instance.UpdateCharacterJson(updatedJson);
        if (CharacterUIManager1.Instance != null) CharacterUIManager1.Instance.character.FromJson(updatedJson);

        Debug.Log($"✅ Đã gỡ {type}. JSON mới: {updatedJson}");
        panel.SetActive(false);
    }

}
