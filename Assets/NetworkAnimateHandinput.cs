using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkAnimateHandinput : NetworkBehaviour
{
    public InputActionProperty pinchAnimateAction;
    public InputActionProperty gripAnimateAction;
    public Animator handAnimator;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOwner)
        {
            float triggerValue = pinchAnimateAction.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", triggerValue);

            float gripValue = gripAnimateAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);

            // Send RPC to synchronize animation parameters across the network
            SetAnimationParametersServerRpc(triggerValue, gripValue);
        }
    }

    [ServerRpc]
    void SetAnimationParametersServerRpc(float triggerValue, float gripValue)
    {
        // Set animation parameters on all clients
        SetAnimationParametersClientRpc(triggerValue, gripValue);
    }

    [ClientRpc]
    void SetAnimationParametersClientRpc(float triggerValue, float gripValue)
    {
        // Update animation parameters on clients
        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);
    }
}
