// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Fusion;
// using Fusion.Sockets;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class NetworkRunnerHanndler : MonoBehaviour
// {
//     [SerializeField]
//     private NetworkRunner _networkRunnerPrefab;

//     private NetworkRunner _networkRunner;

//     private void Awake()
//     {
//         _networkRunner = FindAnyObjectByType<NetworkRunner>();
//     }

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     private void Start()
//     {
//         if (_networkRunner == null)
//         {
//             _networkRunner = Instantiate(_networkRunnerPrefab);
//             _networkRunner.name = "NetworkRunner";
//         }

//         var clientTask = InitializeNetworkRunner(_networkRunner, GameMode.AutoHostOrClient, "TestSession", NetAddress.Any(), SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex), null);

//         Debug.Log("InitializeNetworkRunner called");
//     }

//     INetworkSceneManager GetSceneManager(NetworkRunner runner)
//     {
//         INetworkSceneManager sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

//         // Handle networked objects that already exist in the scene
//         sceneManager ??= runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
//         return sceneManager;
//     }

//     protected virtual Task InitializeNetworkRunner(NetworkRunner networkRunner, GameMode gameMode, string sessionName, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
//     {
//         INetworkSceneManager sceneManager = GetSceneManager(networkRunner);

//         networkRunner.ProvideInput = true;
//         return networkRunner.StartGame(new StartGameArgs
//         {
//             GameMode = gameMode,
//             Address = address,
//             Scene = scene,
//             SessionName = sessionName,
//             CustomLobbyName = "OurLobbyID",
//             SceneManager = sceneManager,
//         });
//     }
// }
