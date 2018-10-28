using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace MetaVisualization
{
    /// <summary>
    /// Dummy Implementation of AMetaVisFactory, for the client (pETV).
    /// </summary>
    public class NullMetaVisFactory : AMetaVisFactory
    {
        public override GameObject CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            return new GameObject("Dummy FlexiblePCP");
        }
    }
}
