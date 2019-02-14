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
    public class ETV3DFactory : AETVFactory
    {
        public GameObject ETVAnchor;

        public AETVBarChart etv3DBarChart;
        public AETVScatterPlot ETV3DScatterPlotPrefab;
        public AETVPCP ETV3DParallelCoordinatesPlotPrefab;
        public AETVHeatMap ETV3DBarMapPrefab;
        public AETVSingleAxis ETV3DSingleAxisPrefab;

        public override AETVBarChart CreateBarChart(DataSet data, string attributeName, bool isMetaVis = false)
        {
            var barChart = Instantiate(etv3DBarChart);

            barChart.Init(data, attributeName, isMetaVis);
            barChart.ChangeColoringScheme(ETVColorSchemes.SplitHSV);

            return barChart;
        }

        public override AETVSingleAxis CreateSingleAxis(DataSet data, string attributeName, bool isMetaVis = false)
        {
            var singleAxis3D = Instantiate(ETV3DSingleAxisPrefab);
            singleAxis3D.Init(data, attributeName, isMetaVis);

            return singleAxis3D;
        }

        public override AETVLineChart CreateLineChart(DataSet data, string attributeNameA, string attributeNameB, bool isMetaVis = false)
        {
            throw new System.NotImplementedException();
        }

        public override AETVScatterPlot CreateScatterplot(DataSet data, string[] attIDs, bool isMetaVis = false)
        {
            var scatterPlot3D = Instantiate(ETV3DScatterPlotPrefab);
            scatterPlot3D.Init(data, attIDs, isMetaVis);
            scatterPlot3D.ChangeColoringScheme(ETVColorSchemes.SplitHSV);

            return scatterPlot3D;
        }

        public override AETVPCP CreatePCP(DataSet data, string[] attIDs, bool isMetaVis = false)
        {
            var pcp = Instantiate(ETV3DParallelCoordinatesPlotPrefab);

            int[] nomIDs, ordIDs, ivlIDs, ratIDs;

            AttributeProcessor.ExtractAttributeIDs(data, attIDs, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);

            pcp.Init(data, nomIDs, ordIDs, ivlIDs, ratIDs, isMetaVis);
            pcp.ChangeColoringScheme(ETVColorSchemes.SplitHSV);

            return pcp;
        }

        public AETVHeatMap CreateETVBarMap(DataSet data, string a1, string a2, bool manualLength = false, float lengthA = 1f, float lengthB = 1f, bool isMetaVis = false)
        {
            var bm = Instantiate(ETV3DBarMapPrefab);

            bm.Init(data, a1, a2, lengthA, lengthB, isMetaVis);
            bm.ChangeColoringScheme(ETVColorSchemes.SplitHSV);

            return bm;
        }
    }
}