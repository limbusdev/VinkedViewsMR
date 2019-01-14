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
