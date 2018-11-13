using ETV;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace MetaVisualization
{
    public abstract class AMetaVisFactory : MonoBehaviour
    {
        /// <summary>
        /// Generates a flexible parallel coordinates plot, which connects
        /// both given axes and adapts to them automatically.
        /// </summary>
        /// <param name="data">data set to use</param>
        /// <param name="attIDs">attribute ids</param>
        /// <param name="axisA">first axis to connect</param>
        /// <param name="axisB">second axis to connect</param>
        /// <returns></returns>
        public abstract AETV CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB);

        public abstract AETV CreateMetaScatterplot2D(DataSet data, string[] attIDs);

        public abstract AETV CreateMetaHeatmap3D(DataSet data, string[] attIDs, bool manualLength = false, float lengthA = 1f, float lengthB = 1f);

        public abstract AETV CreateMetaFlexibleLinedAxes(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB);
    }
}
