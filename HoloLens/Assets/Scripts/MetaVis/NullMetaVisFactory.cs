using ETV;
using GraphicalPrimitive;
using Model;

namespace MetaVisualization
{
    /// <summary>
    /// Dummy Implementation of AMetaVisFactory, for the client (pETV).
    /// </summary>
    public class NullMetaVisFactory : AMetaVisFactory
    {
        public override AETV CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            return new ETV3DFlexiblePCP();
        }

        public override AETV CreateMetaFlexibleLinedAxes(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            throw new System.NotImplementedException();
        }

        public override AETV CreateMetaHeatmap3D(DataSet data, string[] attIDs, bool manualLength=false, float lengthA=1f, float lengthB=1f)
        {
            throw new System.NotImplementedException();
        }

        public override AETV CreateMetaScatterplot2D(DataSet data, string[] attIDs)
        {
            throw new System.NotImplementedException();
        }
    }
}
