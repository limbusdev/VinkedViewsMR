using ETV;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace MetaVisualization
{
    public class MetaVisFactory : AMetaVisFactory
    {
        [SerializeField]
        public GameObject ETV3DFlexiblePCPPrefab;
        
        public override GameObject CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            var metaVis = Instantiate(ETV3DFlexiblePCPPrefab);

            // Get attribute IDs of the given attributes.
            int[] nomIDs, ordIDs, ivlIDs, ratIDs;
            AttributeProcessor.ExtractAttributeIDs(data, attIDs, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);

            var flexPCP = metaVis.GetComponent<ETV3DFlexiblePCP>();
            flexPCP.Init(data, nomIDs, ordIDs, ivlIDs, ratIDs, axisA, axisB);
            flexPCP.DrawGraph();
            flexPCP.ChangeColoringScheme(ETVColorSchemes.SplitHSV);

            return metaVis;
        }
    }
}