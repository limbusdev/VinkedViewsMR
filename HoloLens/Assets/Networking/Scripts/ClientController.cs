using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientController : NetworkBehaviour
{
    public GameObject shellPrefab;
    public GameObject Anchor;
    public GameObject Frame;

    private void Start()
    {
        if(isLocalPlayer)
        {
            // Only show the anchor on the server
            Anchor.SetActive(false);
            Frame.SetActive(false);
        }
    }
    
    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
        var z = Input.GetAxis("Fire1") * Time.deltaTime * 3f;
        transform.position += new Vector3(x,z,y);

        var r = Input.GetAxis("Fire2") * Time.deltaTime * 50.0f;
        var s = Input.GetAxis("Fire3") * Time.deltaTime * 50.0f;
        var t = Input.GetAxis("Jump") * Time.deltaTime * 50.0f;
        
        transform.Rotate(r, s, t, Space.World);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CmdDoOnServer();
        }
    }

    // [Command] methods begin with Cmd and are called by the client, but run on the server
    [Command]
    void CmdDoOnServer()
    {
        // Create the Shell from the Shell Prefab
        var shell = (GameObject)Instantiate(shellPrefab);
        
        // Spawn the shell on all clients
        NetworkServer.Spawn(shell);
    }

    public override void OnStartLocalPlayer()
    {
        // Do something only for the local player prefab instance
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision with: " + collider.gameObject.name);
        if(isLocalPlayer)
        {
            Debug.Log("Collision with: " + collider.gameObject.name);
            var otherAnchor = collider.gameObject.GetComponent<NetworkAnchor>();

            if(otherAnchor != null)
            {
                int dataSetID = otherAnchor.syncedDataSetID;
                string[] attributes = otherAnchor.GetAttributesAsStrings();
                var visType = (VisType)otherAnchor.syncedVisType;

                Debug.Log(attributes.Length);
                
                var vis = ServiceLocator.VisPlant().GenerateVisFrom(dataSetID, attributes, visType);

                ServiceLocator.instance.clientManager.CurrentlyBoundETV = vis;
            }
        }
    }
}
