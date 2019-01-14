/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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