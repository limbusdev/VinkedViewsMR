using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV3DFactory : MonoBehaviour {

    public GameObject etv3DBarChart;

    public GameObject Create3DBarChart(IDictionary<string, DataObject> data, int attributeID)
    {
        GameObject barChart = Instantiate(etv3DBarChart);

        barChart.GetComponent<ETV3DBarChart>().Init(data, attributeID);

        return barChart;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
