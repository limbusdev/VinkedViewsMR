using UnityEngine.Networking;

public class NetworkDiscoveryServer : NetworkDiscovery
{
	// Use this for initialization
	void Start ()
    {
        Initialize();
        StartAsServer();
        NetworkManager.singleton.StartServer();
	}
}
