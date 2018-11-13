using Model;

namespace ETV
{
    /// <summary>
    /// Parallel Coordinates Plot (PCP) visualization.
    /// </summary>
    public abstract class AETVPCP : AETV
    {
        public abstract void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs, bool isMetaVis = false);
    }
}