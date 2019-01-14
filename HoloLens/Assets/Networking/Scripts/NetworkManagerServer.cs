/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
