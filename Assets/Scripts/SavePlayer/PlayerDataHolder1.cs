﻿using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public static class PlayerDataHolder1
{
    //lưu trữ dữ liệu Character , accountid , token  ( dữ liệu sẽ truyền qua )
    //nơi lấy dữ liệu sẽ là PlayerAvatar
    public static string PlayerName;

    private static string _characterJson;
    public static string CharacterJson
    {
        get => _characterJson;
        set
        {
            _characterJson = value;

            Debug.Log("🟢 CharacterJson đã được gán lại.");

            // Cập nhật nhân vật thật (hiển thị tức thì)
            if (PlayerAvatar.Instance != null && PlayerAvatar.Instance.Character != null)
            {
                PlayerAvatar.Instance.Character.FromJson(_characterJson);
                Debug.Log("Player updated.");

                // Gửi lại JSON đã gán xuống network
                if (PlayerAvatar.Instance.HasStateAuthority)
                {
                    PlayerAvatar.Instance.UpdateCharacterJson(_characterJson);
                    Debug.Log(" PlayerAvatar.Instance.UpdateCharacterJson() called.");
                }
            }
            else
            {
                Debug.LogWarning(" PlayerAvatar.Instance hoặc Character null");
            }

            // Cập nhật bảng UI
            if (CharacterUIManager1.Instance != null && CharacterUIManager1.Instance.character != null)
            {
                CharacterUIManager1.Instance.character.FromJson(_characterJson);
                Debug.Log(" Character UI updated.");
            }
            else
            {
                Debug.LogWarning(" CharacterUIManager.Instance hoặc Character null");
            }
        }
    }


    public static int AccountId;
    public static string Token;
    public static CharacterData Character;

}
