using GraphicalPrimitive;
using Model;

namespace ETV
{
    /// <summary>
    /// Scatter plot visualization.
    /// </summary>
    public abstract class AETVScatterPlot : AETV
    {
        public abstract void Init(DataSet data, string[] attributes, bool isMetaVis = false);
    }
}