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

    public override GameObject CreateETVBarChart(DataSet data, string attributeName)
    {
        GameObject barChart = Instantiate(ETV2DBarChartPrefab);

        barChart.GetComponent<ETV2DBarChart>().Init(data, attributeName);
        barChart.GetComponent<ETV2DBarChart>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);

        return barChart;
    }

    public override GameObject CreateETVLineChart(DataSet data, string attributeNameA, string attributeNameB)
    {
        GameObject lineChart = Instantiate(ETV2DLineChartPrefab);

        lineChart.GetComponent<ETV2DLineChart>().Init(data, attributeNameA, attributeNameB);
        lineChart.GetComponent<ETV2DLineChart>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);
        lineChart.GetComponent<ETV2DLineChart>().UpdateETV();
        

        return lineChart;
    }

    public override GameObject CreateETVParallelCoordinatesPlot(DataSet data, string[] attIDs)
    {
        GameObject pcp = Instantiate(ETV2DParallelCoordinatesPlotPrefab);

        int[] nomIDs, ordIDs, ivlIDs, ratIDs;

        AttributeProcessor.ExtractAttributeIDs(data, attIDs, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);

        pcp.GetComponent<ETV2DParallelCoordinatesPlot>().Init(data, nomIDs, ordIDs, ivlIDs, ratIDs);
        pcp.GetComponent<ETV2DParallelCoordinatesPlot>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);
        

        return pcp;
    }

    public override GameObject CreateETVScatterPlot(DataSet data, string[] attIDs)
    {
        GameObject scatterPlot = Instantiate(ETV2DScatterPlotPrefab);

        scatterPlot.GetComponent<ETV2DScatterPlot>().Init(data, attIDs[0], attIDs[1]);
        scatterPlot.GetComponent<ETV2DScatterPlot>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);

        return scatterPlot;
    }

    public override GameObject CreateSingleAxis(DataSet data, int attributeID, LoM lom)
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
    
}
