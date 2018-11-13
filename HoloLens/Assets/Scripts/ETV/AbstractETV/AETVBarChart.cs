using Model;

namespace ETV
{
    /// <summary>
    /// Bar Chart Visualization.
    /// </summary>
    public abstract class AETVBarChart : AETV
    {
        public abstract void Init(DataSet data, string attributeName, bool isMetaVis = false);
    }
}