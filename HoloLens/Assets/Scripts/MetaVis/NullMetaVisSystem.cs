using ETV;
using GraphicalPrimitive;
using UnityEngine;

namespace MetaVisualization
{
    public class NullETV : AETV
    {
        public override void DrawGraph() { }
        public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory() { return ServiceLocator.PrimitivePlant2D(); }
        public override void SetUpAxes() { }
        public override void UpdateETV() { }
    }

    /// <summary>
    /// Dummy Implementation for the client.
    /// </summary>
    public class NullMetaVisSystem : AMetaVisSystem
    {
        public override bool CheckIfCompatible(AETV etvA, AETV etvB, AAxis axisA, AAxis axisB, out int dataSetID)
        {
            dataSetID = 0;
            return false;
        }

        public override bool CheckIfNearEnough(AAxis axisA, AAxis axisB)
        {
            return false;
        }

        public override AETV GenerateFlexibleLinkedAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB)
        {
            throw new System.NotImplementedException();
        }

        public override AETV GenerateHeatmap3D(int dataSetID, string[] variables, AAxis axisA, AAxis axisB, bool duplicateAxes)
        {
            throw new System.NotImplementedException();
        }

        public override AETV GenerateImmersiveAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB)
        {
            var o = new GameObject("Dummy immersive axes");
            return o.AddComponent<ETV3DFlexiblePCP>();
        }

        public override AETV GenerateScatterplot2D(int dataSetID, string[] variables, AAxis axisA, AAxis axisB, bool duplicateAxes)
        {
            throw new System.NotImplementedException();
        }

        public override void ObserveAxis(AETV etv, AAxis axis) { }

        public override AETV SpanMetaVisBetween(AAxis axisA, AAxis axisB, int dataSetID)
        {
            var o = new GameObject("Dummy MetaVis");
            return o.AddComponent<NullETV>();
        }

        public override AETV SpanMetaVisFlexibleLinedAxes(AAxis axisA, AAxis axisB, int dataSetID)
        {
            throw new System.NotImplementedException();
        }

        public override AETV SpanMetaVisHeatmap3D(AAxis axisA, AAxis axisB, int dataSetID, bool duplicateAxes)
        {
            throw new System.NotImplementedException();
        }

        public override AETV SpanMetaVisImmersiveAxis(AAxis axisA, AAxis axisB, int dataSetID)
        {
            var o = new GameObject("Dummy MetaVis Immersive Axis");
            return o.AddComponent<ETV3DFlexiblePCP>();
        }

        public override AETV SpanMetaVisScatterplot2D(AAxis axisA, AAxis axisB, int dataSetID, bool duplicateAxes)
        {
            throw new System.NotImplementedException();
        }

        public override void StopObservationOf(AETV etv, AAxis axis) { }

        public override MetaVisType WhichMetaVis(AAxis axisA, AAxis axisB, int dataSetID)
        {
            return MetaVisType.FlexibleLinkedAxis;
        }
    }
}