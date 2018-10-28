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
        public abstract GameObject CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB);
    }
}
