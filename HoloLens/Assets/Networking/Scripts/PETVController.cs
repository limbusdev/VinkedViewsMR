using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PETVController : NetworkBehaviour
{

    public GameObject Anchor;

    private void Start()
    {
        if(isLocalPlayer)
        {
            // Only show the anchor on the server
            Anchor.SetActive(false);
        }
    }
}
