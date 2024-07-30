using UnityEngine;
using Unity.Netcode;

public class Network_PlayerSpawner : NetworkBehaviour
{
    public GameObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer && NetworkManager.Singleton.IsServer)
        {
            // Spawn the player object for the local client
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);

            // Ensure the spawned player is a networked object
            NetworkObject networkPlayer = player.GetComponent<NetworkObject>();

            // The Spawn method is called on the networked object
            networkPlayer.Spawn();
        }
    }
}
