using Assets.HeroEditor.Common.CharacterScripts;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class PlayerAvatar : NetworkBehaviour
{
    public static PlayerAvatar Instance;
    public Character Character;

    // Các phần nhỏ của JSON  ( khởi tạo nhiều luồng để lưu dữ liệu ) vì dữ liệu quá 512
    [Networked] public NetworkString<_512> CharacterJsonPart1 { get; set; }
    [Networked] public NetworkString<_512> CharacterJsonPart2 { get; set; }
    [Networked] public NetworkString<_512> CharacterJsonPart3 { get; set; }
    [Networked] public NetworkString<_512> CharacterJsonPart4 { get; set; }
    [Networked] public NetworkString<_512> CharacterJsonPart5 { get; set; }
    [Networked] public NetworkString<_512> CharacterJsonPart6 { get; set; }

    private string _lastCharacterJson = "";
    public CinemachineCamera vCam;
    public Camera cam;
    void Awake()
    {
        Instance = this;
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            UpdateCharacterJson(PlayerDataHolder1.CharacterJson);
        }

        _lastCharacterJson = GetFullCharacterJson();
        LoadCharacter(_lastCharacterJson);
        vCam = GetComponentInChildren<CinemachineCamera>();
        cam = GetComponentInChildren<Camera>();
        if (Object.HasInputAuthority)
        {
            // Kích hoạt virtual camera cho player local
            if (vCam != null)
                vCam.enabled = true;
            if(cam != null)
                cam.enabled = true;
        }
        else
        {
            // Tắt virtual camera cho các player không phải local
            if (vCam != null)
                vCam.enabled = false;
            if (cam != null)
                cam.enabled = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        string fullJson = GetFullCharacterJson();

        if (_lastCharacterJson != fullJson)
        {
            _lastCharacterJson = fullJson;
            LoadCharacter(fullJson);
            Debug.Log(" FixedUpdateNetwork gọi LoadCharacter thành công.");

        }
    }
    //xử lý load Character
    public void LoadCharacter(string json)
    {
        if (Character == null)
        {
            Debug.LogError("Character component is missing!");
            return;
        }

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogWarning("Character JSON is null or empty.");
            return;
        }

        try
        {
            Character.FromJson(json);
            Character.Initialize(); //  ép cập nhật lại sprite
            Debug.Log(" Character loaded from JSON.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load character from JSON: {ex.Message}");
        }
    }


    // Cập nhật và chia nhỏ JSON thành các phần nhỏ
    public void UpdateCharacterJson(string fullJson)
    {
        Debug.Log($" UpdateCharacterJson(): fullJson={fullJson.Substring(0, 60)}...");

        if (!HasStateAuthority)
        {
            Debug.LogWarning(" Không có quyền cập nhật CharacterJson (HasStateAuthority = false)");
            return;
        }

        int maxLen = 512;
        int maxTotalLen = maxLen * 6; // 3072 chars

        if (fullJson.Length > maxTotalLen)
        {
            Debug.LogError($"Character JSON too long ({fullJson.Length} chars). Max allowed is {maxTotalLen}.");
            return;
        }

        CharacterJsonPart1 = fullJson.Substring(0, Mathf.Min(maxLen, fullJson.Length));
        CharacterJsonPart2 = fullJson.Length > maxLen ? fullJson.Substring(maxLen, Mathf.Min(maxLen, fullJson.Length - maxLen)) : "";
        CharacterJsonPart3 = fullJson.Length > maxLen * 2 ? fullJson.Substring(maxLen * 2, Mathf.Min(maxLen, fullJson.Length - maxLen * 2)) : "";
        CharacterJsonPart4 = fullJson.Length > maxLen * 3 ? fullJson.Substring(maxLen * 3, Mathf.Min(maxLen, fullJson.Length - maxLen * 3)) : "";
        CharacterJsonPart5 = fullJson.Length > maxLen * 4 ? fullJson.Substring(maxLen * 4, Mathf.Min(maxLen, fullJson.Length - maxLen * 4)) : "";
        CharacterJsonPart6 = fullJson.Length > maxLen * 5 ? fullJson.Substring(maxLen * 5, Mathf.Min(maxLen, fullJson.Length - maxLen * 5)) : "";

        Debug.Log("CharacterJson updated and split into parts.");
    }

    // Lấy lại JSON đầy đủ từ các phần nhỏ
    public string GetFullCharacterJson()
    {
        return CharacterJsonPart1.ToString() +
               CharacterJsonPart2.ToString() +
               CharacterJsonPart3.ToString() +
               CharacterJsonPart4.ToString() +
               CharacterJsonPart5.ToString() +
               CharacterJsonPart6.ToString();
    }
    //check token , kich login nếu trùng token , đẩy client về Scene Login
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_KickToLogin()
    {
        if (Object.HasInputAuthority)
        {
            Debug.Log("Bạn bị đá do đăng nhập trùng!");
            SceneManager.LoadScene("Login");
        }
    }
}
