using GraphicalPrimitive;
using Model;

namespace ETV
{
    /// <summary>
    /// Line chart visualization.
    /// </summary>
    public abstract class AETVLineChart : AETV
    {
        public abstract void Init(DataSet data, string attributeA, string attributeB, bool isMetaVis = false);
    }
}