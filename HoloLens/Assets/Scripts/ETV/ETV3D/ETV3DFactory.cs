using System.Collections;
using System.Collections.Generic;
using ETV.ETV3D;
using Model;
using UnityEngine;

public class ETV3DFactory : AETVFactory
{
    public GameObject ETVAnchor;

    public GameObject etv3DBarChart;
    public GameObject etv3DGroupedBarChart;
    public GameObject barChartLegend3D;
    public GameObject ETV3DScatterPlotPrefab;
    public GameObject ETV3DParallelCoordinatesPlotPrefab;
    public GameObject ETV3DBarMapPrefab;
    public GameObject ETV3DSingleAxisPrefab;

    public override GameObject CreateETVBarChart(DataSet data, int nominalAttributeID)
    {
        GameObject barChart = Instantiate(etv3DBarChart);

        barChart.GetComponent<ETV3DBarChart>().Init(data, nominalAttributeID);
        barChart.GetComponent<ETV3DBarChart>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);

        return barChart;
    }

    public GameObject Create3DGroupedBarChart(IDictionary<string, InformationObject> data)
    {
        GameObject barChart = Instantiate(etv3DGroupedBarChart);

        //barChart.GetComponent<ETV3DGroupedBarChart>().Init(data);

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

    public override GameObject CreateSingleAxis(DataSet data, int attributeID)
    {
        GameObject singleAxis3D = Instantiate(ETV3DSingleAxisPrefab);
        singleAxis3D.GetComponent<ETV3DSingleAxis>().Init(data, attributeID);

        return singleAxis3D;
    }

    public override GameObject CreateETVLineChart(DataSet data, int floatAttributeX, int floatAttributeY, bool xAxisBoundToZero, bool yAxisBoundToZero)
    {
        throw new System.NotImplementedException();
    }

    public override GameObject CreateETVScatterPlot(DataSet data, int[] floatAttributeIDs)
    {
        GameObject scatterPlot3D = Instantiate(ETV3DScatterPlotPrefab);
        scatterPlot3D.GetComponent<ETV3DScatterPlot>().Init(data, floatAttributeIDs[0], floatAttributeIDs[1], floatAttributeIDs[2]);
        scatterPlot3D.GetComponent<ETV3DScatterPlot>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);

        return scatterPlot3D;
    }

    public override GameObject CreateETVParallelCoordinatesPlot(DataSet data, int[] nomIDs, int[] ordIDs, int[] ivlIDs, int[] ratIDs)
    {
        GameObject pcp = Instantiate(ETV3DParallelCoordinatesPlotPrefab);

        pcp.GetComponent<ETV3DParallelCoordinatesPlot>().Init(data, nomIDs, ordIDs, ivlIDs, ratIDs);
        pcp.GetComponent<ETV3DParallelCoordinatesPlot>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);

        return pcp;
    }

    public GameObject CreateETVBarMap(DataSet data, int a1, int a2)
    {
        GameObject bm = Instantiate(ETV3DBarMapPrefab);

        bm.GetComponent<ETV3DBarMap>().Init(data, a1, a2);
        bm.GetComponent<ETV3DBarMap>().ChangeColoringScheme(ETVColorSchemes.SplitHSV);

        return bm;
    }

    public override GameObject PutETVOnAnchor(GameObject ETV)
    {
        GameObject Anchor = Instantiate(ETVAnchor);
        Anchor.GetComponent<ETVAnchor>().PutETVintoAnchor(ETV);
        return Anchor;
    }
}
