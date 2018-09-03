using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DFactory : AETVFactory
{
    public GameObject ETV2DBarChartPrefab;
    public GameObject ETV2DLineChartPrefab;
    public GameObject ETV2DVirtualDevicePrefab;
    public GameObject ETV2DScatterPlotPrefab;

    public override GameObject CreateETVBarChart(DataSet data, int attributeID)
    {
        GameObject barChart = Instantiate(ETV2DBarChartPrefab);

        barChart.GetComponent<ETV2DBarChart>().Init(data, attributeID);

        return barChart;
    }

    public override GameObject CreateETVLineChart(DataSetLines data, float minX, float maxX, float minY, float maxY, float ticksX, float ticksY)
    {
        GameObject lineChart = Instantiate(ETV2DLineChartPrefab);

        lineChart.GetComponent<ETV2DLineChart>().Init(data, minX, maxX, minY, maxY, ticksX, ticksY);
        lineChart.GetComponent<ETV2DLineChart>().UpdateETV();

        return lineChart;
    }

    public override GameObject CreateETVScatterPlot(DataSetPoints data, float[] mins, float[] maxs, float[] ticks)
    {
        GameObject scatterPlot = Instantiate(ETV2DScatterPlotPrefab);

        scatterPlot.GetComponent<ETV2DScatterPlot>().Init(data, mins[0], maxs[0], mins[1], maxs[1], ticks[0], ticks[1]);
        scatterPlot.GetComponent<ETV2DScatterPlot>().UpdateETV();

        return scatterPlot;
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
