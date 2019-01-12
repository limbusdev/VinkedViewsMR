using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public abstract class AETVFactory : MonoBehaviour
    {
        public GameObject ETVAnchorPrefab;

        public abstract AETVSingleAxis  CreateSingleAxis(DataSet data, string attributeName, bool isMetaVis=false);
        public abstract AETVPCP         CreatePCP(DataSet data, string[] attIDs, bool isMetaVis = false);
        public abstract AETVLineChart   CreateLineChart(DataSet data, string attributeNameA, string attributeNameB, bool isMetaVis = false);
        public abstract AETVBarChart    CreateBarChart(DataSet data, string attributeName, bool isMetaVis = false);
        public abstract AETVScatterPlot CreateScatterplot(DataSet data, string[] attIDs, bool isMetaVis = false);


        public GameObject PutETVOnAnchor(GameObject ETV)
        {
            var Anchor = Instantiate(ETVAnchorPrefab);
            Anchor.GetComponent<ETVAnchor>().PutETVintoAnchor(ETV);
            return Anchor;
        }
    }
}