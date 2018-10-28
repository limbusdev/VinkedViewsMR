using GraphicalPrimitive;
using UnityEngine;

namespace MetaVisualization
{
    /// <summary>
    /// Dummy Implementation for the client.
    /// </summary>
    public class NullMetaVisSystem : AMetaVisSystem
    {
        public override bool CheckIfCompatible(AAxis axisA, AAxis axisB, out int dataSetID)
        {
            dataSetID = 0;
            return false;
        }

        public override bool CheckIfNearEnough(AAxis axisA, AAxis axisB)
        {
            return false;
        }

        public override GameObject GenerateImmersiveAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB)
        {
            return new GameObject("Dummy Axis");
        }

        public override void ObserveAxis(AAxis axis) { }

        public override GameObject SpanMetaVisBetween(AAxis axisA, AAxis axisB, int dataSetID)
        {
            return new GameObject("Dummy MetaVis");
        }

        public override GameObject SpanMetaVisImmersiveAxis(AAxis axisA, AAxis axisB, int dataSetID)
        {
            return new GameObject("Dummy MetaVis Immersive Axis");
        }

        public override void StopObservationOf(AAxis axis) { }

        public override MetaVisType WhichMetaVis(AAxis axisA, AAxis axisB, int dataSetID)
        {
            return MetaVisType.FlexibleLinkedAxis;
        }
    }
}