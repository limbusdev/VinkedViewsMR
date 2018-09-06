using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetButtonIcons : MonoBehaviour {

    public Material material;
    public GameObject[] icons;

	// Use this for initialization
	void Start ()
    {
		foreach(GameObject icon in icons)
        {
            icon.GetComponent<Renderer>().material = material;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
