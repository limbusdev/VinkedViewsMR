using HoloToolkit.Unity;
using UnityEngine;

public class WorldAnchorObject : MonoBehaviour
{
	void Start ()
    {
        // Loads Anchor, if it exists, creates one otherwise.
        WorldAnchorManager.Instance.AttachAnchor(gameObject, "RootWorldAnchor");	
	}
	
	void Update ()
    {
		
	}
}
