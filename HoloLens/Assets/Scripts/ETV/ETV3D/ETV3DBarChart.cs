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

using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D Euclidean transformable View: 3D Bar Chart
///
/// A Bar chart which normalizes linearily to 1,
/// calculated from the maximum of the provided
/// values.
/// </summary>
namespace ETV
{
    public class ETV3DBarChart : AETVBarChart
    {
        // ........................................................................ Populate in Editor

        // ........................................................................ Private properties

        private string attributeName;
        private int attributeID;

        private LoM lom;
        string[] uniqueVals;
        int[] distribution;
        int dim;

        private Bar3D[] bars;

        private int[] absMapValues;
        private float[] barHeights;
        private IDictionary<int, string> dict1;
        private IDictionary<string, int> dict2;

        float max;
        private float length;


        // ........................................................................ Initializers
        public override void Init(DataSet data, string attributeName, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.attributeName = attributeName;
            this.attributeID = data.IDOf(attributeName);
            this.lom = data.TypeOf(attributeName);
            this.max = 0;
            this.length = 1f;

            dict1 = new Dictionary<int, string>();
            dict2 = new Dictionary<string, int>();

            SetUpAxes();

            // .................................................................... initialize
            switch(lom)
            {
                case LoM.NOMINAL:
                    var mN = data.nominalStatistics[attributeName];
                    uniqueVals = mN.uniqueValues;
                    dict1 = mN.idValues;
                    dict2 = mN.valueIDs;
                    max = mN.distMax;
                    break;
                case LoM.ORDINAL:
                    var mO = data.ordinalStatistics[attributeName];
                    uniqueVals = mO.uniqueValues;
                    dict1 = mO.orderedValueIDs;
                    dict2 = mO.orderedIDValues;
                    max = mO.distMax;
                    break;
                default:
                    // Nothing
                    break;
            }

            dim = uniqueVals.Length;
            this.absMapValues = new int[dim];
            this.barHeights = new float[dim];

            switch(lom)
            {
                case LoM.NOMINAL:
                    InitNominal(data, attributeName);
                    break;
                case LoM.ORDINAL:
                    InitOrdinal(data, attributeName);
                    break;
                default:
                    break;
            }

            ChangeColoringScheme(ETVColorSchemes.SplitHSV);
        }

        private void InitNominal(DataSet data, string attributeName)
        {
            var measures = data.nominalStatistics[attributeName];

            foreach(var key in measures.distribution.Keys)
            {
                int ID = measures.valueIDs[key];
                absMapValues[ID] = measures.distribution[key];
                barHeights[ID] = measures.distribution[key] / (1f * measures.distMax);
            }

            if(max > 0)
            {
                DrawGraph();

                foreach(var o in data.infoObjects)
                {
                    bool missing = data.IsValueMissing(o, attributeName);

                    if(!missing)
                    {
                        ABar bar = bars[dict2[o.NomValueOf(attributeName)]];
                        RememberRelationOf(o, bar);
                    }
                }
            }
        }

        private void InitOrdinal(DataSet data, string attributeName)
        {
            var measures = data.ordinalStatistics[attributeName];

            foreach(var key in measures.distribution.Keys)
            {
                var name = measures.orderedValueIDs[key];
                absMapValues[key] = measures.distribution[key];
                barHeights[key] = measures.distribution[key] / (1f * measures.distMax);
            }

            if(max > 0)
            {
                DrawGraph();

                foreach(var o in data.infoObjects)
                {
                    bool missing = data.IsValueMissing(o, attributeName);

                    if(!missing)
                    {
                        ABar bar = bars[o.OrdValueOf(attributeName)];
                        RememberRelationOf(o, bar);
                    }
                }
            }
        }

        // ........................................................................ Helper Methods
        private Bar3D CreateBar(float value, float range, float widthA = .1f, float widthB = .1f)
        {
            var factory3D = Services.instance.Factory3DPrimitives;

            var bar = factory3D.CreateBar(value, widthA).GetComponent<Bar3D>();
            bar.Assign(this);

            bar.SetLabelText(value.ToString());

            return bar;
        }

        public override void SetUpAxes()
        {
            var factory2D = Services.PrimFactory2D();

            // Categorical Axis A
            AddAxis(attributeName, AxisDirection.X, length);
            var xAxis = GetAxis(AxisDirection.X);
            xAxis.transform.parent = Anchor.transform;
            xAxis.transform.localRotation = Quaternion.Euler(90, 0, 0);

            var yAxis = factory2D.CreateAutoTickedAxis("Amount", max);
            yAxis.transform.parent = Anchor;

            // Grid
            //var grid = Services.PrimFactory2D().CreateAutoGrid(max, Vector3.right, Vector3.up, length);
            //grid.transform.localPosition = new Vector3(0, 0, .002f);
            //grid.transform.parent = Anchor.transform;
        }

        public override void ChangeColoringScheme(ETVColorSchemes scheme)
        {
            switch(scheme)
            {
                default: // case SplitHSV
                    float H = 0f;
                    for(int bar = 0; bar < dim; bar++)
                    {
                        var color = Color.HSVToRGB((H / dim) / 2f + .5f, 1, 1);
                        bars[bar].SetColor(color, Color.green);
                        H++;
                    }
                    break;
            }
        }

        public override void UpdateETV()
        {
            
        }

        public override void DrawGraph()
        {
            float barGap = .01f;
            float gapWidth = (dim - 1) * barGap;
            float barWidth = (length - gapWidth - .1f) / (dim);


            bars = new Bar3D[dim];

            float margin = .05f + barWidth / 2f;

            for(int i = 0; i < dim; i++)
            {
                bars[i] = CreateBar(barHeights[i], max, barWidth, barWidth);
                bars[i].SetLabelText(absMapValues[i].ToString());

                // Set bar's position
                var barGO = bars[i].gameObject;
                barGO.transform.localPosition = new Vector3(
                    margin + i * (barWidth + barGap),
                    0,
                    .1f);
                barGO.transform.parent = Anchor.transform;
            }
        }
    }
}