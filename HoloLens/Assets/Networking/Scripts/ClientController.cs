using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientController : NetworkBehaviour
{
    public GameObject shellPrefab;
    public Transform shellSpawn;

    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    // [Command] methods begind with Cmd and are called by the client, but run on the server
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Shell Prefab
        var bullet = (GameObject)Instantiate(shellPrefab, shellSpawn.position, shellSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the shell on all clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        foreach(var c in GetComponentsInChildren<MeshRenderer>())
            c.material.color = Color.blue;
    }
}
