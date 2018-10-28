using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Searches for a host / server and connects automatically to it, if in the
/// same local area network.
/// </summary>
public class NetworkDiscoveryClient : NetworkDiscovery
{
    [SerializeField]
    public Text label;
    public bool clientStarted = false;
    
    // Use this for initialization
    void Start()
    {
        Initialize();
        StartAsClient();
        label = GameObject.Find("NetworkStatusLabel").GetComponent<Text>();
    }
    
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        // Do nothing if connection has been established already.
        if(clientStarted)
        {
            return;
        }

        // Try setting up connection
        try
        {
            // Setup connection to server with the data from broadcast
            NetworkManager.singleton.networkAddress = fromAddress;
            NetworkManager.singleton.StartClient();
            clientStarted = true;
            label.GetComponent<Text>().text = "Network status: connected.";
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            clientStarted = false;
            label.GetComponent<Text>().text = "Network status: Connection failed. Trying again.";
        }
        
        // Log received broadcast
        Debug.Log("Received Broadcast from: " + fromAddress + " with data: " + data);
    }
}
