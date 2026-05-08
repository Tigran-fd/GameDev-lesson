using UnityEngine;
using Mirror;

public class NetworkLauncher : MonoBehaviour
{
    private NetworkManager manager;

    void Start()
    {
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;
        manager = GetComponent<NetworkManager>();
    }

    public void StartHost()
    {
        manager.StartHost();
    }

    public void StartClient()
    {
        manager.networkAddress = "localhost";
        manager.StartClient(); 
    }
}