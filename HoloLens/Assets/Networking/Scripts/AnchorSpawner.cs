using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnchorSpawner : NetworkBehaviour
{
    public GameObject AnchorPrefab;

    public override void OnStartServer()
    {
        ServiceLocator.VisPlant().StartTestSetup();
    }
}