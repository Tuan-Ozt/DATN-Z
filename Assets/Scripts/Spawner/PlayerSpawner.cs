using Fusion;
using Fusion.Sockets;
using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerSpawner : SimulationBehaviour, INetworkRunnerCallbacks
{
    //spawn nhân vật với trạng thái local ( các client sẽ mang một dữ liệu riêng , xử lý dữ liệu riêng biệt mà k ảnh hưởng tới ai )
    public NetworkObject playerPrefab;
    public GameObject characterCanvasPrefab;



    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            //Ui Character riêng client
            var canvas = Instantiate(characterCanvasPrefab);
            canvas.SetActive(true);
            Vector3 spawnPosition = new Vector3(0, -7.02f, 0);
            Quaternion spawnRotation = Quaternion.identity;
            //gọi inventory
            //var uiManager = canvas.GetComponentInChildren<InventoryUIManager>();
            //uiManager.gridParent = canvas.transform.Find("Scroll View/Viewport/Content"); // đường dẫn đến Grid
            //uiManager.DisplayInventory(uiManager.testItems);

            NetworkObject obj = runner.Spawn(playerPrefab, spawnPosition, spawnRotation, player);

            var avatar = obj.GetComponent<PlayerAvatar>();
            if (avatar != null)
            {
                Debug.Log("da goi");
                avatar.UpdateCharacterJson(PlayerDataHolder1.CharacterJson);
            }
            //  Gán chỉ số gốc của player ( tất cả player sẽ chung một chỉ số
            var stats = obj.GetComponent<CharacterStats>();
            if (stats != null)
            {
                stats.strength = 50;
                stats.defense = 50;
                stats.agility = 50;
                stats.vitality = 200;
            }
            string token = PlayerDataHolder1.Token;

            if (OnlineAccountManager.OnlineTokens.TryGetValue(token, out PlayerRef oldPlayer))
            {
                if (!oldPlayer.Equals(player))
                {
                    // Đá client cũ
                    if (runner.TryGetPlayerObject(oldPlayer, out NetworkObject oldPlayerObj))
                    {
                        var oldAvatar = oldPlayerObj.GetComponent<PlayerAvatar>();
                        if (oldAvatar != null)
                        {
                            oldAvatar.RPC_KickToLogin();
                        }
                    }
                }
            }

            OnlineAccountManager.OnlineTokens[token] = player;
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        string tokenToRemove = null;

        foreach (var kvp in OnlineAccountManager.OnlineTokens)
        {
            if (kvp.Value == player)
            {
                tokenToRemove = kvp.Key;
                break;
            }
        }

        if (!string.IsNullOrEmpty(tokenToRemove))
        {
            OnlineAccountManager.OnlineTokens.Remove(tokenToRemove);
            Debug.Log("Đã xóa token khi client rời game");
        }
    }




    // Các callback khác giữ nguyên
    //public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
}