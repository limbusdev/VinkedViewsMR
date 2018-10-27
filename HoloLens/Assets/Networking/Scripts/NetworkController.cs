using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// NetworkManager derivation for the server
/// </summary>
public class NetworkController : NetworkManager
{
    public GameObject pETVAnchorPrefab; 

    IDictionary<int, GameObject> pETVIDs;

    private int pETVcounter = 0;
    

    // Called on the server, when it starts
    public override void OnStartServer()
    {
        base.OnStartServer();

        pETVIDs = new Dictionary<int, GameObject>();

        Debug.Log("Server started");
    }

    // Called on the server when a new client connects.
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        
        pETVIDs.Add(conn.connectionId, Instantiate(pETVAnchorPrefab)); // Show ConnectionID on anchor and on physical device

        Debug.Log("A new pETV connected, with ID: " + conn.connectionId);
    }

    // Called on the server when a client disconnects.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        Destroy(pETVIDs[conn.connectionId]); // TODO manage removal in VisualizationFactory
        pETVIDs.Remove(conn.connectionId);

        Debug.Log("A pETV disconnected, with ID: " + conn.connectionId);
    }

}
