using Model;
using UnityEngine;

namespace ETV
{
    public class ETV2DFactory : AETVFactory
    {
        public GameObject ETVAnchor;
        public AETVBarChart ETV2DBarChartPrefab;
        public AETVLineChart ETV2DLineChartPrefab;
        public GameObject ETV2DVirtualDevicePrefab;
        public AETVScatterPlot ETV2DScatterPlotPrefab;
        public AETVPCP ETV2DParallelCoordinatesPlotPrefab;

        public override AETVBarChart CreateBarChart(DataSet data, string attributeName, bool isMetaVis = false)
        {
            var barChart = Instantiate(ETV2DBarChartPrefab);

            barChart.Init(data, attributeName, isMetaVis);
            barChart.ChangeColoringScheme(ETVColorSchemes.SplitHSV);

            return barChart;
        }

        public override AETVLineChart CreateLineChart(DataSet data, string attributeNameA, string attributeNameB, bool isMetaVis = false)
        {
            var lineChart = Instantiate(ETV2DLineChartPrefab);

            lineChart.Init(data, attributeNameA, attributeNameB, isMetaVis);
            lineChart.ChangeColoringScheme(ETVColorSchemes.SplitHSV);

            return lineChart;
        }

        public override AETVPCP CreatePCP(DataSet data, string[] attIDs, bool isMetaVis = false)
        {
            var pcp = Instantiate(ETV2DParallelCoordinatesPlotPrefab);

            int[] nomIDs, ordIDs, ivlIDs, ratIDs;

            AttributeProcessor.ExtractAttributeIDs(data, attIDs, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);

            pcp.Init(data, nomIDs, ordIDs, ivlIDs, ratIDs, isMetaVis);
            pcp.ChangeColoringScheme(ETVColorSchemes.SplitHSV);


            return pcp;
        }

        public override AETVScatterPlot CreateScatterplot(DataSet data, string[] attIDs, bool isMetaVis = false)
        {
            var scatterPlot = Instantiate(ETV2DScatterPlotPrefab);

            scatterPlot.Init(data, attIDs, isMetaVis);
            scatterPlot.ChangeColoringScheme(ETVColorSchemes.SplitHSV);

            return scatterPlot;
        }

        public override AETVSingleAxis CreateSingleAxis(DataSet data, string attributeName, bool isMetaVis = false)
        {
            return (new GameObject("Dummy 2D Single Axis ETV Implementation")).AddComponent<ETV3DSingleAxis>();
        }

        public GameObject CreateVirtualDevice()
        {
            return Instantiate(ETV2DVirtualDevicePrefab);
        }
    }
}