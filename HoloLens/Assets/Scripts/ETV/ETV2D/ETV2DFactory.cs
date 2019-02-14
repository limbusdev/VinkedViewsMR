/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
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