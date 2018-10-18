using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETVAnchor : MonoBehaviour {

    public GameObject VisAnchor;
    public GameObject Rotatable;
    public bool resetRotation = false;
    public Quaternion defaultLocalRotation = Quaternion.Euler(new Vector3(0, 180, 0));

    public void PutETVintoAnchor(GameObject ETV)
    {
        ETV.transform.parent = VisAnchor.transform;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(resetRotation)
        {
            Rotatable.transform.localRotation = Quaternion.RotateTowards(Rotatable.transform.localRotation, defaultLocalRotation, Time.deltaTime*50f);
            if(Vector3.Distance(Rotatable.transform.localRotation.eulerAngles, defaultLocalRotation.eulerAngles) < .1f)
            {
                Rotatable.transform.localRotation = defaultLocalRotation;
                resetRotation = false;
            }
        }
	}
}
