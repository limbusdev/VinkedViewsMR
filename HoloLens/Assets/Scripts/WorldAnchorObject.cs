using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;

public class WorldAnchorObject : MonoBehaviour
{
    public string AnchorName;

	void Start ()
    {
        // Loads Anchor, if it exists, creates one otherwise.
        AnchorName = gameObject.name;
        WorldAnchorStore.GetAsync(StoreLoaded);
	}

    private void StoreLoaded(WorldAnchorStore store)
    {
        WorldAnchorManager.Instance.AttachAnchor(gameObject, gameObject.name);
        Debug.Log("Anchor attached for: " + this.gameObject.name + " - AnchorID: " + AnchorName);
    }
}
