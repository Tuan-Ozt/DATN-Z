[System.Serializable]
public class ItemStatsDTO
{
    public string Name;
    public string Type;
    public string Description;
    public string Rarity;


    public string Value;// tính trong mua bán 0 là k mua bán được . 1 là mua bán được
    public int Strength;  // sức mạnh
    public int Defense;   // phòng thủ
    public int Agility;   // nhanh nhẹn
    public int Intelligence; // tinh thần ( thông minh)
    public int Vitality;   // hp
}

