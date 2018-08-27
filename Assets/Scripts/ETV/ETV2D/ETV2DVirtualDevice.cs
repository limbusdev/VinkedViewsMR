using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DVirtualDevice : MonoBehaviour {

    public GameObject visualization;
    public GameObject VisualizationAnchor;
    public GameObject DeviceAnchor;

    public GameObject borderT, borderB, borderL, borderR, cornerBL, cornerBR, cornerTL, cornerTR, screen;

    public void BindVisualization(GameObject vis)
    {
        SetSize(vis.GetComponent<AETV2D>().bounds[0], vis.GetComponent<AETV2D>().bounds[1]);
        vis.transform.parent = gameObject.transform;
        DeviceAnchor.transform.localPosition = new Vector3(-.3f, -.3f, .005f);
    }

    public void SetSize(float width, float height)
    {
        cornerBL.transform.localPosition = new Vector3(0.02f, 0.02f, 0);
        cornerBR.transform.localPosition = new Vector3(width - 0.02f, 0.02f, 0);
        cornerTL.transform.localPosition = new Vector3(0.02f, height - 0.02f, 0);
        cornerTR.transform.localPosition = new Vector3(width - 0.02f, height - 0.02f, 0);

        borderT.transform.localPosition = new Vector3(0.5f * width, height-0.01f);
        borderB.transform.localPosition = new Vector3(0.5f * width, 0.01f);
        borderL.transform.localPosition = new Vector3(0.01f, 0.5f * height);
        borderR.transform.localPosition = new Vector3(width-0.01f, 0.5f * height);

        borderT.transform.localScale = new Vector3(width-0.04f, 0.02f, 0.01f);
        borderB.transform.localScale = new Vector3(width-0.04f, 0.02f, 0.01f);
        borderL.transform.localScale = new Vector3(0.02f, height - 0.04f, 0.01f);
        borderR.transform.localScale = new Vector3(0.02f, height - 0.04f, 0.01f);

        screen.transform.localPosition = new Vector3(0.5f*width, 0.5f*height);
        screen.transform.localScale = new Vector3(width-0.04f, height-0.04f, 0.0025f);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
