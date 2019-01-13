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
