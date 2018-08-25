using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChartLegend3D : MonoBehaviour {

    public GameObject barChartLegend3DEntry;

    public void Init(string[] names, Color[] colors)
    {
        for(int i=0; i<names.Length; i++)
        {
            var entry = Instantiate(barChartLegend3DEntry);
            entry.GetComponent<BarChartLegend3DEntry>().Init(names[i], colors[i]);
            entry.transform.parent = gameObject.transform;
            entry.transform.localPosition = new Vector3(0,(names.Length-i)*.15f,0);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
