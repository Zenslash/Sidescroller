using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Mirror;

public class SNetworkManager : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private SNetworkRoomPlayer roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private SNetworkGamePlayer gamePlayerPrefab = null;

    [Header("Events")]
    [SerializeField] private GameEvent OnClientConnected;
    [SerializeField] private GameEvent OnClientDisconnected;

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected.Raise();
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        OnClientDisconnected.Raise();
    }

    public override void OnStartServer()
    {
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }
    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        foreach(var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();

            //TODO Show player limit error

            return;
        }

        if(SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();

            //TODO Show error

            return;
        }
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if(SceneManager.GetActiveScene().name == menuScene)
        {
            SNetworkRoomPlayer roomPlayer = Instantiate(roomPlayerPrefab);

            //Associate network connection with gameobject
            NetworkServer.AddPlayerForConnection(conn, roomPlayer.gameObject);
        }
    }

}
