using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class ETV3DFactory : AETVFactory {

    public GameObject etv3DBarChart;
    public GameObject etv3DGroupedBarChart;
    public GameObject barChartLegend3D;

    public override GameObject CreateETVBarChart(DataSet data, int attributeID)
    {
        GameObject barChart = Instantiate(etv3DBarChart);

        barChart.GetComponent<ETV3DBarChart>().Init(data, attributeID);

        return barChart;
    }

    public GameObject Create3DGroupedBarChart(IDictionary<string, DataObject> data)
    {
        GameObject barChart = Instantiate(etv3DGroupedBarChart);

        barChart.GetComponent<ETV3DGroupedBarChart>().Init(data);

        return barChart;
    }

    public GameObject Create3DBarChartLegend(string[] names, Color[] colors)
    {
        GameObject legend = Instantiate(barChartLegend3D);

        legend.GetComponent<BarChartLegend3D>().Init(names, colors);

        return legend;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override GameObject CreateETVLineChart(DataSetLines data, float minX, float maxX, float minY, float maxY, float ticksX, float ticksY)
    {
        throw new System.NotImplementedException();
    }
}
