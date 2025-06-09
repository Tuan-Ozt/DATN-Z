using UnityEngine;
using System.Collections.Generic;
//nhận dữ liệu từ scripobject để truyền chỉ số vào characterUI . ( check itemid của item và hiện chỉ số của item đó )
public class ItemStatDatabase : MonoBehaviour
{
    public static ItemStatDatabase Instance;
    private Dictionary<string, ItemStats> dict;

    void Awake()
    {
        Instance = this;
        dict = new Dictionary<string, ItemStats>();
        var allStats = Resources.LoadAll<ItemStats>("ItemStats");
        foreach (var stat in allStats)
        {
            dict[stat.itemId] = stat;
        }
    }

    public ItemStats GetStats(string id)
    {
        dict.TryGetValue(id, out var stats);
        return stats;
    }
}
