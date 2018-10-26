using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public GameObject etvPosition;
    public VisualizationFactory fact;

    private void Start()
    {
        var fact2 = ServiceLocator.instance.Factory2DETV;
        var prim2Dfactory = ServiceLocator.instance.Factory2DPrimitives;

        var etvYearPopulationCrimePCP2D = fact.GeneratePCP2DFrom(0, new string[] { "Year", "Population", "Violent crime", "Rape (legacy)" });
        etvYearPopulationCrimePCP2D.transform.parent = etvPosition.transform;

        etvYearPopulationCrimePCP2D.transform.localScale = Vector3.one;
    }
}
