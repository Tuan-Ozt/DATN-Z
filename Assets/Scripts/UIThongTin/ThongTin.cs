using System.Collections;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class ThongTin : MonoBehaviour
{
    public CharacterStats Instane;
    public TextMeshProUGUI Nametext;
    public TextMeshProUGUI strength;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI agility;
    public TextMeshProUGUI vitality;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

        StartCoroutine(WaitForPlayerStats());
    }
    IEnumerator WaitForPlayerStats()
    {
        GameObject player = null;

        while (player == null)
        {
            player = GameObject.FindWithTag("Player"); // ← sửa tại đây!
            yield return null;
        }
        Nametext.text = "Tên: " + PlayerDataHolder1.PlayerName;
        var stats = player.GetComponent<CharacterStats>();
        if (stats != null)
        {
            strength.text = "Sức mạnh: " + stats.strength;
            defense.text = "Phòng thủ: " + stats.defense;
            agility.text = "Nhanh nhẹn: " + stats.agility;
            vitality.text = "Sinh lực: " + stats.vitality;
        }
        else
        {
            Debug.LogWarning("⚠️ Player không có CharacterStats.");
        }
    }





}
