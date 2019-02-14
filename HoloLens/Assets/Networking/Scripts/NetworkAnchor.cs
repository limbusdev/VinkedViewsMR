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
using UnityEngine;
using UnityEngine.Networking;

public class NetworkAnchor : NetworkBehaviour
{
    public GameObject ETV;

    // ........................................................................ MonoBehaviour
    void Update()
    {
        if(VisualizationFactory.onServer)
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
