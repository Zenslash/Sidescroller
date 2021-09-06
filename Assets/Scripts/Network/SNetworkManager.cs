using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Mirror;

public class SNetworkManager : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private SNetworkRoomPlayer roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private SNetworkGamePlayer gamePlayerPrefab = null;

    [Header("Events")]
    [SerializeField] private GameEvent OnClientConnected;
    [SerializeField] private GameEvent OnClientDisconnected;

    public List<SNetworkRoomPlayer> RoomPlayers { get; } = new List<SNetworkRoomPlayer>();

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
            bool isLeader = (RoomPlayers.Count == 0);

            SNetworkRoomPlayer roomPlayer = Instantiate(roomPlayerPrefab);

            roomPlayer.IsLeader = isLeader;

            //Associate network connection with gameobject
            NetworkServer.AddPlayerForConnection(conn, roomPlayer.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(conn.identity != null)
        {
            var player = conn.identity.GetComponent<SNetworkRoomPlayer>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }
    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach(var Player in RoomPlayers)
        {
            Player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach(var player in RoomPlayers)
        {
            if(!player.IsReady) { return false; }
        }

        return true;
    }

}
