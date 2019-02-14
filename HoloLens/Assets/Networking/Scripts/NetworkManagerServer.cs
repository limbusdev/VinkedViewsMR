/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// NetworkManager derivation for the server
/// </summary>
public class NetworkManagerServer : NetworkManager
{
    IDictionary<int, GameObject> pETVIDs;
    
    // Called on the server, when it starts
    public override void OnStartServer()
    {
        base.OnStartServer();

        pETVIDs = new Dictionary<int, GameObject>();

        Debug.Log("Server started");
    }

    private void Start()
    {
        StartServer();
        GetComponent<NetworkDiscovery>().Initialize();
        GetComponent<NetworkDiscovery>().StartAsServer();


        // Initialize dependent Services
        VisualizationFactory.onServer = true;
        Services.Persistence().Load();
    }

    // Called on the server when a new client connects.
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        
        pETVIDs.Add(conn.connectionId, null); // Show ConnectionID on anchor and on physical device

        Debug.Log("A new pETV connected, with ID: " + conn.connectionId);
    }

    // Called on the server when a client disconnects.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        pETVIDs.Remove(conn.connectionId);

        Debug.Log("A pETV disconnected, with ID: " + conn.connectionId);
    }
}
