using UnityEngine;
using Unity.Netcode;

public class NetworkPrefabHandler : NetworkBehaviour
{
    // Player spawner prefab to be automatically spawned for each client
    public NetworkObject playerSpawnerPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer && NetworkManager.Singleton.IsServer)
        {
            // Spawn the player spawner prefab for each client
            playerSpawnerPrefab.Spawn();
        }
    }
}
