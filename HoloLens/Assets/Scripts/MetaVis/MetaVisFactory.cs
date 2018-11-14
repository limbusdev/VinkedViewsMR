using ETV;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace MetaVisualization
{
    public class MetaVisFactory : AMetaVisFactory
    {
        [SerializeField]
        public ETV3DFlexiblePCP ETV3DFlexiblePCPPrefab;
        
        public override AETV CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            var flexPCP = Instantiate(ETV3DFlexiblePCPPrefab);

            // Get attribute IDs of the given attributes.
            int[] nomIDs, ordIDs, ivlIDs, ratIDs;
            AttributeProcessor.ExtractAttributeIDs(data, attIDs, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);
            
            flexPCP.Init(data, nomIDs, ordIDs, ivlIDs, ratIDs, axisA, axisB, true);
            flexPCP.DrawGraph();

            return flexPCP;
        }

        public override AETV CreateMetaFlexibleLinedAxes(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            throw new System.NotImplementedException();
        }

        public override AETV CreateMetaHeatmap3D(DataSet data, string[] attIDs, bool manualLength = false, float lengthA = 1f, float lengthB = 1f)
        {
            // Create Visualization
            var factory = Services.ETVFactory3D();
            var mVis = factory.CreateETVBarMap(data, attIDs[0], attIDs[1], manualLength, lengthA, lengthB, true);

            return mVis;
        }

        public override AETV CreateMetaScatterplot2D(DataSet data, string[] attIDs)
        {
            // Create Visualization
            var factory = Services.ETVFactory2D();
            var mVis = factory.CreateScatterplot(data, attIDs, true);
            
            return mVis;
        }
    }
}