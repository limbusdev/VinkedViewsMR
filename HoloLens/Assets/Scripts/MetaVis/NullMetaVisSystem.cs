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