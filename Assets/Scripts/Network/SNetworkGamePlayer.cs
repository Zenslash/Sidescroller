using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SNetworkGamePlayer : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";

    private SNetworkManager room;
    private SNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as SNetworkManager;
        }
    }

    public override void OnStartClient()
    {
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
