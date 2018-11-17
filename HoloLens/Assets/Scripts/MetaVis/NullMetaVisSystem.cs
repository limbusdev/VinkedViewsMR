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