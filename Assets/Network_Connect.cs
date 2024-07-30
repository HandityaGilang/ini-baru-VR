using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections;

public class Network_Connect : MonoBehaviour
{
    [SerializeField]
    private string gameScene;

    public void Create()
    {
        // Load the game scene first
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);

        // Call StartHost after the scene has been loaded
        StartCoroutine(StartHostAfterSceneLoaded());
    }

    private IEnumerator StartHostAfterSceneLoaded()
    {
        // Wait until the next frame after the scene is loaded
        yield return null;

        // Start hosting
        NetworkManager.Singleton.StartHost();
    }

    public void Join()
    {
        // Start as a client
        NetworkManager.Singleton.StartClient();

        // Load the game scene
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }
}