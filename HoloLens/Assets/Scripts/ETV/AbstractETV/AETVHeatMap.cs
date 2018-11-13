using Model;

namespace ETV
{
    /// <summary>
    /// Heatmap visualization.
    /// </summary>
    public abstract class AETVHeatMap : AETV
    {
        public abstract void Init(DataSet data, string attA, string attB, float lengthA=1f, float lengthB=1f, bool isMetaVis = false);
    }
}