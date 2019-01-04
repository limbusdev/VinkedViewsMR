using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAnchorObject : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        WorldAnchorManager.Instance.AttachAnchor(gameObject, "RootWorldAnchor");	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
