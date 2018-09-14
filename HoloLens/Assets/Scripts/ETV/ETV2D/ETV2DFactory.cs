using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DFactory : AETVFactory
{
    public GameObject ETVAnchor;
    public GameObject ETV2DBarChartPrefab;
    public GameObject ETV2DLineChartPrefab;
    public GameObject ETV2DVirtualDevicePrefab;
    public GameObject ETV2DScatterPlotPrefab;
    public GameObject ETV2DParallelCoordinatesPlotPrefab;

    public override GameObject CreateETVBarChart(DataSet data, int nominalAttributeID, int numericAttributeID)
    {
        GameObject barChart = Instantiate(ETV2DBarChartPrefab);

        barChart.GetComponent<ETV2DBarChart>().Init(data, nominalAttributeID, numericAttributeID);
        barChart.GetComponent<ETV2DBarChart>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);

        return barChart;
    }

    public override GameObject CreateETVLineChart(DataSet data, int floatAttributeX, int floatAttributeY, bool xAxisBoundToZero, bool yAxisBoundToZero)
    {
        GameObject lineChart = Instantiate(ETV2DLineChartPrefab);

        lineChart.GetComponent<ETV2DLineChart>().Init(data, floatAttributeX, floatAttributeY, xAxisBoundToZero, yAxisBoundToZero);
        lineChart.GetComponent<ETV2DLineChart>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);
        lineChart.GetComponent<ETV2DLineChart>().UpdateETV();
        

        return lineChart;
    }

    public override GameObject CreateETVParallelCoordinatesPlot(DataSet data, int[] floatAttributeIDs, int[] stringAttributeIDs)
    {
        GameObject pcp = Instantiate(ETV2DParallelCoordinatesPlotPrefab);

        pcp.GetComponent<ETV2DParallelCoordinatesPlot>().Init(data, floatAttributeIDs, stringAttributeIDs);
        pcp.GetComponent<ETV2DParallelCoordinatesPlot>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);
        

        return pcp;
    }

    public override GameObject CreateETVScatterPlot(DataSet data, int[] floatAttributeIDs)
    {
        GameObject scatterPlot = Instantiate(ETV2DScatterPlotPrefab);

        scatterPlot.GetComponent<ETV2DScatterPlot>().Init(data, floatAttributeIDs[0], floatAttributeIDs[1]);
        scatterPlot.GetComponent<ETV2DScatterPlot>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);

        return scatterPlot;
    }

    public override GameObject CreateSingleAxis(DataSet data, int attributeID)
    {
        throw new System.NotImplementedException();
    }

    public GameObject CreateVirtualDevice()
    {
        return Instantiate(ETV2DVirtualDevicePrefab);
    }

    public override GameObject PutETVOnAnchor(GameObject ETV)
    {
        GameObject Anchor = Instantiate(ETVAnchor);
        Anchor.GetComponent<ETVAnchor>().PutETVintoAnchor(ETV);
        return Anchor;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
