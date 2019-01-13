using UnityEngine;
using UnityEngine.Networking;

public class NetworkAnchor : NetworkBehaviour
{
    public GameObject ETV;

    // ........................................................................ MonoBehaviour
    void Update()
    {
        if(isServer)
        {
            if(ETV.transform.hasChanged)
            {
                gameObject.transform.position = ETV.transform.position;
                ETV.transform.hasChanged = false;
            }
        }
    }


    // ........................................................................ Server Side Logic

    [SyncVar] public int syncedDataSetID;
    [SyncVar] public int syncedVisType;
    [SyncVar] public string syncedHint;
    
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
    

    // ........................................................................ Helper Methods

    public string[] GetAttributesAsStrings()
    {
        string[] atts = new string[syncedAttributes.Count];
        syncedAttributes.CopyTo(atts,0);
        return atts;
    }
}
