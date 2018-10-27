using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkControllerDiscovery : NetworkDiscovery
{
	// Use this for initialization
	void Start ()
    {
        Initialize();
        StartAsServer();
        NetworkManager.singleton.StartServer();
	}
}
