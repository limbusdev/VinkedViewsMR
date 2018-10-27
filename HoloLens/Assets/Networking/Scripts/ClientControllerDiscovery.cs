using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ClientControllerDiscovery : NetworkDiscovery
{
    [SerializeField]
    public GameObject label;
    public bool clientStarted = false;
    

    // Use this for initialization
    void Start()
    {
        Initialize();
        StartAsClient();
        label = GameObject.Find("NetworkStatusLabel");
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if(clientStarted)
            return;

        try
        {
            NetworkManager.singleton.networkAddress = fromAddress;
            NetworkManager.singleton.StartClient();
            clientStarted = true;
            label.GetComponent<Text>().text = "Network status: connected.";
        } catch (Exception e)
        {
            Debug.LogError(e.Message);
            clientStarted = false;
            label.GetComponent<Text>().text = "Network status: Connection failed. Trying again.";
        }
        



        Debug.Log("Received Broadcast from: " + fromAddress + " with data: " + data);
    }
}
