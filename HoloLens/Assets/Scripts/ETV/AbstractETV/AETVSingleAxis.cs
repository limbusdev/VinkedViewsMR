using GraphicalPrimitive;
using Model;

namespace ETV
{
    /// <summary>
    /// One dimensional scatter plot visualization (single axis).
    /// </summary>
    public abstract class AETVSingleAxis : AETV
    {
        public abstract void Init(DataSet data, string attribute, bool isMetaVis = false);
    }
}