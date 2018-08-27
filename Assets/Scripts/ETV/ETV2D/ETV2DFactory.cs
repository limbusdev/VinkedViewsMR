using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DFactory : AETVFactory
{
    public GameObject ETV2DBarChartPrefab;
    public GameObject ETV2DVirtualDevicePrefab;

    public override GameObject CreateETVBarChart(DataSet data, int attributeID)
    {
        GameObject barChart = Instantiate(ETV2DBarChartPrefab);

        barChart.GetComponent<ETV2DBarChart>().Init(data, attributeID);

        return barChart;
    }

    public GameObject CreateVirtualDevice()
    {
        return Instantiate(ETV2DVirtualDevicePrefab);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
