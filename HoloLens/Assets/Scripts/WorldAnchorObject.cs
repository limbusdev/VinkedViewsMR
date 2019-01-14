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
#if UNITY_WSA
using HoloToolkit.Unity;
using UnityEngine.XR.WSA.Persistence;
#endif

// UNITY_WSA defines the Universal Windows Platform,
// use those imports only, when compiling for the HoloLens


public class WorldAnchorObject : MonoBehaviour
{
    public string AnchorName;

	void Start ()
    {
        // Loads Anchor, if it exists, creates one otherwise.
        AnchorName = gameObject.name;
        #if UNITY_WSA
        WorldAnchorStore.GetAsync(StoreLoaded);
        #endif
	}

    #if UNITY_WSA
    private void StoreLoaded(WorldAnchorStore store)
    {
        WorldAnchorManager.Instance.AttachAnchor(gameObject, gameObject.name);
        Debug.Log("Anchor attached for: " + this.gameObject.name + " - AnchorID: " + AnchorName);
    }
    #endif
}
