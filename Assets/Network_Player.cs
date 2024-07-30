using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;

public class Network_Player : NetworkBehaviour
{
    [SerializeField]
    private Vector2 placementArea = new Vector2(-10.0f, 10.0f);

    [Header("Components to Disable")]
    [SerializeField] private NetworkMoveProvider clientMoveProvider;
    [SerializeField] private ActionBasedController[] clientControllers;
    [SerializeField] private ActionBasedSnapTurnProvider clientTurnProvider;
    [SerializeField] private XRDirectInteractor[] clientDirectInteractors;
    [SerializeField] private TrackedPoseDriver clientHead;
    [SerializeField] private Camera clientCamera;

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            EnableLocalPlayerInput();
        }
        else
        {
            DisableOtherPlayersInput();
        }
    }

    public void EnableLocalPlayerInput()
    {
        if (clientCamera != null)
            clientCamera.enabled = true;

        if (clientMoveProvider != null)
            clientMoveProvider.enableInputActions = true;

        if (clientTurnProvider != null)
        {
            clientTurnProvider.enableTurnLeftRight = true;
            clientTurnProvider.enableTurnAround = true;
        }

        if (clientHead != null)
            clientHead.enabled = true;

        if (clientControllers != null)
        {
            foreach (var controller in clientControllers)
            {
                if (controller != null)
                {
                    controller.enableInputActions = true;
                    controller.enableInputTracking = true;
                }
            }
        }

        if (clientDirectInteractors != null)
        {
            foreach (var interactor in clientDirectInteractors)
            {
                if (interactor != null)
                    interactor.enabled = true;
            }
        }
    }

    public void DisableOtherPlayersInput()
    {
        if (clientCamera != null)
            clientCamera.enabled = false;

        if (clientMoveProvider != null)
            clientMoveProvider.enableInputActions = false;

        if (clientTurnProvider != null)
        {
            clientTurnProvider.enableTurnLeftRight = false;
            clientTurnProvider.enableTurnAround = false;
        }

        if (clientHead != null)
            clientHead.enabled = false;

        if (clientControllers != null)
        {
            foreach (var controller in clientControllers)
            {
                if (controller != null)
                {
                    controller.enableInputActions = false;
                    controller.enableInputTracking = false;
                }
            }
        }

        if (clientDirectInteractors != null)
        {
            foreach (var interactor in clientDirectInteractors)
            {
                if (interactor != null)
                    interactor.enabled = false;
            }
        }
    }

    private void Start()
    {
        if (IsLocalPlayer)
        {
            transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y), transform.position.y,
                Random.Range(placementArea.x, placementArea.y));
        }
    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        if (IsLocalPlayer)
        {
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();
            if (networkObjectSelected != null)
            {
                RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected.NetworkObjectId);
            }
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId, ulong networkObjectId)
    {
        var networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null)
        {
            networkObject.ChangeOwnership(newOwnerClientId);
        }
        else
        {
            Debug.LogWarning($"Unable to change ownership for clientId {newOwnerClientId}");
        }
    }
}
