// using System;
// using System.Collections.Generic;
// using Fusion;
// using Fusion.Sockets;
// using UnityEngine;

// public class Spawner : SimulationBehaviour, INetworkRunnerCallbacks
// {
//     [SerializeField] private NetworkObject _playerPrefab;



//     public void OnConnectedToServer(NetworkRunner runner)
//     {
//     }

//     public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
//     {
//     }

//     public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
//     {
//     }

//     public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
//     {
//     }

//     public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
//     {
//     }

//     public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
//     {
//     }

//     public void OnInput(NetworkRunner runner, NetworkInput input)
//     {
//     }

//     public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
//     {
//     }

//     public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
//     {
//     }

//     public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
//     {
//     }

//     public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
//     {
//         if (runner.IsServer)
//         {
//             Debug.Log("OnPlayerJoined this is the server/host/OnIn, spwawning network player");

//             NetworkObject playerObject = runner.Spawn(_playerPrefab.gameObject, Vector3.zero, Quaternion.identity, player);
//         }
//         else
//         {
//             Debug.Log("OnPlayerJoined this is the client");
//         }
//     }

//     public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
//     {
//     }

//     public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
//     {
//     }

//     public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
//     {
//     }

//     public void OnSceneLoadDone(NetworkRunner runner)
//     {
//     }

//     public void OnSceneLoadStart(NetworkRunner runner)
//     {
//     }

//     public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
//     {
//     }

//     public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
//     {
//     }

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }
