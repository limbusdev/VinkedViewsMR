using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkAnchor : NetworkBehaviour
{
    public GameObject observedAnchor;


    [SyncVar]
    public int dataSetID;

    [SyncVar]
    public SyncListString attributes;

    [SyncVar]
    public int visType;

    private void Start()
    {
        this.attributes = new SyncListString();
    }

    public void Init(int dataSetID, string[] attributes, VisType visType)
    {
        this.dataSetID = dataSetID;

        for(int i = 0; i < attributes.Length; i++)
        {
            this.attributes.Add(attributes[i]);
        }

        this.visType = (int)visType;
    }

    [ClientRpc]
    public void RpcEnableVis()
    {
        string[] atts = new string[attributes.Count];

        int counter = 0;
        foreach(var a in attributes)
        {
            atts[counter] = a;
            counter++;
        }

        ServiceLocator.VisPlant().GenerateVisFrom(dataSetID, atts, (VisType)visType);
    }

    [TargetRpc]
    public void TargetEnableVisOn(NetworkConnection target, int dataSetID, string[] attributes, int visType)
    {
        ServiceLocator.VisPlant().GenerateVisFrom(dataSetID, attributes, (VisType)visType);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var otherAnchor = collision.gameObject.GetComponent<NetworkAnchor>();
    }
}
