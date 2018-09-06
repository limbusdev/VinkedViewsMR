using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETVAnchor : MonoBehaviour {

    public GameObject VisAnchor;

    public void PutETVintoAnchor(GameObject ETV)
    {
        ETV.transform.parent = VisAnchor.transform;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
