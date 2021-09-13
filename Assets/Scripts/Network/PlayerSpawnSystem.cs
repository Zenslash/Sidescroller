using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class PlayerSpawnSystem : NetworkBehaviour
{

    [SerializeField] private GameObject playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }
    public static void RemoveSpawnPoint(Transform transform)
    {
        spawnPoints.Remove(transform);
    }

    public override void OnStartServer()
    {
        SNetworkManager.OnServerReadied += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        SNetworkManager.OnServerReadied -= SpawnPlayer;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if(spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.Spawn(playerInstance, conn);

        nextIndex++;
    }

}
