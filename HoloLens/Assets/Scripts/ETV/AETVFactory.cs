using GraphicalPrimitive;
using Model;
using UnityEngine;

public abstract class AETVFactory : MonoBehaviour
{
    public GameObject ETVAnchorPrefab;
    

    public abstract GameObject CreateSingleAxis(DataSet data, int attributeID, LoM lom);
    public abstract GameObject CreateETVParallelCoordinatesPlot(DataSet data, string[] attIDs);
    public abstract GameObject CreateETVLineChart(DataSet data, string attributeNameA, string attributeNameB);
    public abstract GameObject CreateETVBarChart(DataSet data, string attributeName);
    public abstract GameObject CreateETVScatterPlot(DataSet data, string[] attIDs);


    public GameObject PutETVOnAnchor(GameObject ETV)
    {
        GameObject Anchor = Instantiate(ETVAnchorPrefab);
        Anchor.GetComponent<ETVAnchor>().PutETVintoAnchor(ETV);
        return Anchor;
    }

    public void LetMetaVisSystemObserveAxes(AAxis[] axes)
    {
        // Tell MetaVisSystem about new axis
        foreach(var axis in axes)
        {
            ServiceLocator.instance.metaVisSystem.ObserveAxis(axis);
        }
        
    }
}
