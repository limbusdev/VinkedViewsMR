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

using ETV;
using GraphicalPrimitive;
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