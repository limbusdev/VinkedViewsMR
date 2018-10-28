using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkAnchor : NetworkBehaviour
{
    [SyncVar]
    public int syncedDataSetID;
    [SyncVar]
    public int syncedVisType;
    [SyncVar]
    public string syncedHint;
    
    public SyncListString syncedAttributes = new SyncListString();
    

    [Server]
    public void Init(int dataSetID, string[] attributes, VisType visType)
    {
        this.syncedHint = "Sync works!";
        this.syncedDataSetID = dataSetID;

        for(int i = 0; i < attributes.Length; i++)
        {
            syncedAttributes.Add(attributes[i]);
            syncedAttributes.Dirty(i);
        }

        this.syncedVisType = (int)visType;
    }
    

    public string[] GetAttributesAsStrings()
    {
        string[] atts = new string[syncedAttributes.Count];
        syncedAttributes.CopyTo(atts,0);
        return atts;
    }
}
