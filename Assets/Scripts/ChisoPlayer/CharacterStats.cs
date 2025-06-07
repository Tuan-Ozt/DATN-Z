using Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int strength = 10;
    public int defense = 10;
    public int agility = 10;
    public int vitality = 100;

   /* [Header("Final Stats (có thể bị cộng thêm từ đồ)")]
    public int finalStrength;
    public int finalDefense;
    public int finalAgility;
    public int finalVitality;*/

    // Hàm gọi mỗi lần nhân vật mặc đồ mới
    /*public void RecalculateStats(Equipment[] equippedItems)
    {
        finalStrength = strength;
        finalDefense = defense;
        finalAgility = agility;
        finalVitality = vitality;

        foreach (var item in equippedItems)
        {
            if (item == null) continue;

            finalStrength += item.bonusStrength;
            finalDefense += item.bonusDefense;
            finalAgility += item.bonusAgility;
            finalVitality += item.bonusVitality;
        }

        Debug.Log($"[Stats Updated] STR: {finalStrength}, DEF: {finalDefense}, AGI: {finalAgility}, VIT: {finalVitality}");
    }*/
}
