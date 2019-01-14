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
using UnityEngine;

namespace MetaVisualization
{
    public class NullETV : AETV
    {
        public override void DrawGraph() { }
        public override void SetUpAxes() { }
        public override void UpdateETV() { }
    }

    /// <summary>
    /// Dummy Implementation for the client.
    /// </summary>
    public class NullMetaVisSystem : AMetaVisSystem
    {
        public override bool CheckIfCompatible(AxisPair axes, out int dataSetID)
        {
            dataSetID = 0;
            return false;
        }
        
        public override AETV GenerateFlexibleLinkedAxes(int dataSetID, string[] variables, AxisPair axes)
        {
            var mVis = new GameObject();
            var comp = mVis.AddComponent<ETV2DScatterPlot>();
            return comp;
        }

        public override AETV GenerateHeatmap3D(int dataSetID, string[] variables, AxisPair axes, bool duplicateAxes)
        {
            var mVis = new GameObject();
            var comp = mVis.AddComponent<ETV2DScatterPlot>();
            return comp;
        }

        public override AETV GenerateImmersiveAxes(int dataSetID, string[] variables, AxisPair axes)
        {
            var o = new GameObject("Dummy immersive axes");
            return o.AddComponent<ETV3DFlexiblePCP>();
        }

        public override AETV GenerateScatterplot2D(int dataSetID, string[] variables, AxisPair axes, bool duplicateAxes)
        {
            var mVis = new GameObject();
            var comp = mVis.AddComponent<ETV2DScatterPlot>();
            return comp;
        }

        public override AETV SpanMetaVisFor(AxisPair axes, int dataSetID, out MetaVisType type)
        {
            var o = new GameObject("Dummy MetaVis");
            type = MetaVisType.FLEXIBLE_LINKED_AXES;
            return o.AddComponent<NullETV>();
        }

        public override AETV SpanMetaVisFlexibleLinedAxes(AxisPair axes, int dataSetID)
        {
            var mVis = new GameObject();
            var comp = mVis.AddComponent<ETV2DScatterPlot>();
            return comp;
        }

        public override AETV SpanMetaVisHeatmap3D(AxisPair axes, int dataSetID, bool duplicateAxes)
        {
            var mVis = new GameObject();
            var comp = mVis.AddComponent<ETV2DScatterPlot>();
            return comp;
        }

        public override AETV SpanMetaVisImmersiveAxis(AxisPair axes, int dataSetID)
        {
            var o = new GameObject("Dummy MetaVis Immersive Axis");
            return o.AddComponent<ETV3DFlexiblePCP>();
        }

        public override AETV SpanMetaVisScatterplot2D(AxisPair axes, int dataSetID, bool duplicateAxes)
        {
            var mVis = new GameObject();
            var comp = mVis.AddComponent<ETV2DScatterPlot>();
            return comp;
        }

        public override void UseCombination(AxisPair key)
        {
            // Do nothing
        }

        public override void ReleaseCombination(AxisPair key)
        {
            // Do nothing
        }

        public override void Observe(AAxis observable)
        {
            // Do nothing
        }

        public override void Ignore(AAxis observable)
        {
            // Do nothing
        }

        public override void OnDispose(AAxis observable)
        {
            // Do nothing
        }

        public override void OnChange(AAxis observable)
        {
            // Do nothing
        }
    }
}