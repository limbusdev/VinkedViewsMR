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
